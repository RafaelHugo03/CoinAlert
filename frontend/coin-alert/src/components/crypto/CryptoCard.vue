<script setup lang="ts">
import type { CryptoPrice } from '@/types/crypto'

defineProps<{
  crypto: CryptoPrice
}>()

const emit = defineEmits<{
  createOpportunity: [cryptoId: string]
}>()

const COIN_LABELS: Record<string, string> = {
  bitcoin: 'Bitcoin',
  ethereum: 'Ethereum',
  solana: 'Solana',
}

function label(id: string): string {
  return COIN_LABELS[id] ?? id.charAt(0).toUpperCase() + id.slice(1)
}

function formatPrice(value: number): string {
  if (value === 0) return '—'
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    maximumFractionDigits: 2,
  }).format(value)
}

function formatChange(value: number): string {
  const sign = value >= 0 ? '+' : ''
  return `${sign}${value.toFixed(2)}%`
}
</script>

<template>
  <div class="card">
    <div class="card-header">
      <div>
        <div class="coin-name">{{ label(crypto.cryptoId) }}</div>
        <div class="coin-id">{{ crypto.cryptoId }}</div>
      </div>
      <button class="btn-opportunity" @click="emit('create-opportunity', crypto.cryptoId)">+ Opportunity</button>
    </div>

    <div class="card-price">{{ formatPrice(crypto.usd) }}</div>

    <div class="card-change" :class="crypto.usd24hChange >= 0 ? 'positive' : 'negative'">
      {{ formatChange(crypto.usd24hChange) }}
      <span class="change-label">24h</span>
    </div>
  </div>
</template>

<style scoped>
.card {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  transition: border-color 0.2s;
}

.card:hover {
  border-color: var(--color-primary);
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.coin-name {
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--color-text);
}

.coin-id {
  font-size: 0.75rem;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-top: 0.1rem;
}

.btn-opportunity {
  background: rgba(59, 130, 246, 0.12);
  border: 1px solid var(--color-primary);
  color: var(--color-primary);
  padding: 0.3rem 0.75rem;
  border-radius: 6px;
  font-size: 0.8rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.15s;
}

.btn-opportunity:hover {
  background: var(--color-primary);
  color: #fff;
}

.card-price {
  font-size: 1.9rem;
  font-weight: 700;
  color: var(--color-text);
  font-variant-numeric: tabular-nums;
  letter-spacing: -0.5px;
}

.card-change {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.95rem;
  font-weight: 600;
}

.card-change.positive {
  color: var(--color-success);
}

.card-change.negative {
  color: var(--color-danger);
}

.change-label {
  font-size: 0.75rem;
  font-weight: 400;
  color: var(--color-text-muted);
}
</style>
