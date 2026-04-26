# CoinAlert

Monitoramento de preços de criptomoedas em tempo real com alertas configuráveis de compra e venda. Acompanha Bitcoin, Ethereum e Solana — transmite atualizações de preço ao vivo para o navegador e dispara uma notificação nativa no momento em que um preço-alvo é atingido.

---

## Visão Geral

O CoinAlert consulta a API pública do [CoinGecko](https://www.coingecko.com/) a cada **5 segundos** e transmite as atualizações de preço para todos os clientes conectados via **SignalR**. O usuário pode cadastrar preços-alvo (oportunidades) para qualquer moeda monitorada. O backend avalia cada oportunidade a cada ciclo de preços e a marca como disparada quando a condição é satisfeita, enviando um evento SignalR em tempo real e uma notificação push no navegador.

Moedas monitoradas: **Bitcoin (BTC)**, **Ethereum (ETH)**, **Solana (SOL)**  
---

## Arquitetura

```
Navegador
  │
  ├── HTTP /api/*   ──► ASP.NET Core 8 REST API
  └── WS   /hubs/*  ──► SignalR Hub (tempo real)
              │
              ├── MongoDB   — persistência de oportunidades
              ├── Redis     — cache da lista de oportunidades
              └── CoinGecko — fonte de preços (API externa)

Jaeger UI (porta 16686) — visualizador de rastreamento distribuído
```

Todo o tráfego do navegador passa por um **Nginx** atuando como reverse proxy dentro do container do frontend. O Nginx encaminha `/api/` e `/hubs/` para o container do backend e serve o SPA Vue para todo o resto.

---

## Tecnologias

### Frontend

| Tecnologia | Versão |
|---|---|
| Vue 3 | ^3.5.32 |
| Vite | ^8.0.8 |
| Pinia | ^3.0.4 |
| Vue Router | ^5.0.4 |
| @microsoft/signalr | ^10.0.0 |
| TypeScript | ~6.0.0 |
| Nginx (runtime) | `nginx:alpine` |
| Node (build) | `node:22-alpine` |

### Backend

| Tecnologia | Versão |
|---|---|
| .NET / ASP.NET Core | 8.0 |
| MongoDB.Driver | 2.28.0 |
| Microsoft.Extensions.Caching.StackExchangeRedis | 10.0.7 |
| OpenTelemetry SDK | 1.15.x |
| OpenTelemetry ASP.NET Core instrumentation | 1.15.2 |
| OpenTelemetry HTTP client instrumentation | 1.15.1 |
| OpenTelemetry OTLP exporter | 1.15.3 |
| Swashbuckle (Swagger) | 6.6.2 |

### Infraestrutura (imagens Docker)

| Serviço | Imagem |
|---|---|
| MongoDB | `mongo:7.0` |
| Redis | `redis:7.2-alpine` |
| Jaeger | `jaegertracing/all-in-one:1.57` |
| Backend (runtime) | `mcr.microsoft.com/dotnet/aspnet:8.0` |
| Backend (build) | `mcr.microsoft.com/dotnet/sdk:8.0` |

---

## Mapa de Serviços

| Serviço | Endereço | Descrição |
|---|---|---|
| Frontend | http://localhost:3000 | SPA Vue + Nginx reverse proxy |
| Backend API | http://localhost:5001 | REST API + SignalR hub |
| MongoDB | localhost:27017 | Persistência das oportunidades |
| Redis | localhost:6379 | Cache da lista de oportunidades |
| Jaeger UI | http://localhost:16686 | Dashboard de rastreamento distribuído |

O backend também está acessível em `http://localhost:5001/swagger` para exploração interativa da API.

---

## Oportunidades

Uma **oportunidade** é um preço-alvo que o usuário define para uma moeda e uma direção (Compra ou Venda).

### Como cadastrar

No Dashboard, clique no ícone de alerta em qualquer card de moeda e preencha:

- **Tipo** — `Compra` (comprar quando o preço cair até o alvo) ou `Venda` (vender quando o preço subir até o alvo)
- **Preço-alvo** — o valor em USD que vai disparar o alerta

A oportunidade é salva no MongoDB com status `Ativa`.

### Como o disparo funciona

O worker em background `PriceMonitorService` executa a cada **5 segundos**:

1. Busca os preços mais recentes no CoinGecko (ou gera preços mockados em caso de rate limit — veja abaixo).
2. Carrega todas as oportunidades com status `Ativo` do banco de dados.
3. Para cada oportunidade, avalia a condição de disparo:
   - **Compra** → disparada quando `preçoAtual <= preçoAlvo`
   - **Venda** → disparada quando `preçoAtual >= preçoAlvo`
4. Quando disparada, a oportunidade é atualizada para `Triggered` no MongoDB, o cache do Redis é invalidado e dois eventos são emitidos em paralelo:
   - Evento **SignalR** (`OpportunityTriggered`) enviado a todos os clientes conectados — a página de Oportunidades atualiza instantaneamente.
   - **Notificação push no navegador** com o nome da moeda, tipo do alerta, preço-alvo e preço atual.

---

## Preços em Tempo Real

1. O `PriceMonitorService` consulta o CoinGecko a cada 5 segundos.
2. Cada atualização de preço é transmitida a todos os clientes SignalR via evento `ReceivePriceUpdate`.
3. A `cryptoStore` (Pinia) atualiza seu estado reativo e cada card do Dashboard re-renderiza automaticamente — sem polling no navegador.

Na primeira carga, o frontend chama `POST /api/crypto/prices/fetch` para popular a store com os preços atuais antes de chegar o primeiro evento SignalR, evitando um flash de dashboard vazio.

### Fallback para rate limit do CoinGecko

O plano gratuito do CoinGecko impõe limite de requisições (HTTP 429). Quando um 429 é detectado, o backend registra um aviso e retorna **preços mockados** com valores aleatórios dentro de faixas realistas:

| Moeda | Faixa (USD) |
|---|---|
| Bitcoin | $10.000 – $100.000 |
| Ethereum | $1.000 – $3.000 |
| Solana | $1 – $100 |

Isso mantém a aplicação totalmente funcional durante janelas de rate limit — os cards continuam atualizando e a avaliação de oportunidades prossegue normalmente.

---

## Cache com Redis

O Redis é utilizado como camada **cache-aside** para a lista de oportunidades, reduzindo acessos ao MongoDB a cada requisição `GET /api/opportunities`.

| Evento | Ação no cache |
|---|---|
| `GET /api/opportunities` (cache miss) | Lê do MongoDB, grava no Redis (TTL de 10 minutos) |
| `GET /api/opportunities` (cache hit) | Retorna direto do Redis |
| Criação de oportunidade | Invalida a chave de cache |
| Exclusão de oportunidade | Invalida a chave de cache |
| Oportunidade disparada | Invalida a chave de cache |

A chave de cache é `CoinAlert:opportunities:all`. A interface `ICacheService` expõe métodos genéricos `GetAsync<T>`, `SetAsync<T>` e `InvalidateAsync`, mantendo a lógica de cache desacoplada do MongoDB.

---

## Observabilidade

Rastreamentos distribuídos são coletados via **OpenTelemetry** e exportados ao **Jaeger** pelo protocolo OTLP gRPC (porta 4317).

Fontes instrumentadas:
- **ASP.NET Core** — todas as requisições HTTP de entrada, incluindo o negotiate do SignalR
- **HttpClient** — chamadas de saída para a API do CoinGecko

Acesse a UI do Jaeger em http://localhost:16686 e selecione o serviço `CoinAlertApi` para inspecionar traces, latência e erros.

---

## Executando com Docker Compose

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (já inclui o Docker Compose)

### Primeira execução

```bash
docker compose up --build -d
```

O flag `--build` compila o backend e gera o bundle do frontend dentro do Docker — obrigatório na primeira execução ou após alterações no código.

### Execuções seguintes

```bash
docker compose up -d
```

### Parar tudo

```bash
docker compose down
```

### Ordem de inicialização

O Docker Compose aguarda MongoDB, Redis e Jaeger passarem em seus health checks antes de iniciar o backend. O Nginx do frontend utiliza o resolver DNS embutido do Docker (`127.0.0.11`) com resolução de hostname por requisição, portanto inicia de forma independente e começa a rotear para o backend assim que ele estiver disponível — não há dependência de inicialização fixa entre frontend e backend.

---