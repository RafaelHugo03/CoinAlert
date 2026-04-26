using CoinAlertApi.Application.Hubs;
using CoinAlertApi.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.BindEnvironmentVariables(builder.Configuration);
builder.Services.RegisterDatabase();
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterHttpClients();
builder.Services.RegisterCache(builder.Configuration);
builder.Services.RegisterObservability(builder.Configuration);
builder.Services.RegisterSignalR();
builder.Services.RegisterHostedServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapControllers();
app.MapHub<CryptoPriceHub>("/hubs/crypto-price");

app.Run();

