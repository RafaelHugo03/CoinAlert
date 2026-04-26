<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useOpportunityStore } from '@/stores/opportunityStore'
import { OpportunityType, OpportunityStatus } from '@/types/opportunity'

const store = useOpportunityStore()

onMounted(() => store.fetchAll())

const active = computed(() => store.opportunities.filter((o) => o.status === OpportunityStatus.Active))
const triggered = computed(() => store.opportunities.filter((o) => o.status === OpportunityStatus.Triggered))

function formatPrice(v: number): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    maximumFractionDigits: 2,
  }).format(v)
}

function formatDate(d: string): string {
  return new Date(d).toLocaleString()
}
</script>

<template>
  <div class="opportunities">
    <section class="section">
      <h2 class="section-title">Triggered Opportunities</h2>
      <div v-if="triggered.length === 0" class="empty">No opportunities triggered yet.</div>
      <div v-else class="list">
        <div v-for="item in triggered" :key="item.id" class="item">
          <div class="item-left">
            <span class="coin">{{ item.cryptoId }}</span>
            <span :class="['badge', item.type === OpportunityType.Buy ? 'badge-buy' : 'badge-sell']">
              {{ item.type === OpportunityType.Buy ? 'BUY' : 'SELL' }}
            </span>
          </div>
          <div class="item-col">
            <span class="label">Target</span>
            <span class="value">{{ formatPrice(item.targetPrice) }}</span>
          </div>
          <div class="item-col">
            <span class="label">Current</span>
            <span class="value">{{ formatPrice(item.currentPrice) }}</span>
          </div>
          <div class="item-col ml-auto">
            <span class="label">Triggered at</span>
            <span class="value muted">{{ formatDate(item.triggeredAt!) }}</span>
          </div>
        </div>
      </div>
    </section>

    <section class="section">
      <h2 class="section-title">Active Opportunities</h2>
      <div v-if="active.length === 0" class="empty">No active opportunities.</div>
      <div v-else class="list">
        <div v-for="item in active" :key="item.id" class="item">
          <div class="item-left">
            <span class="coin">{{ item.cryptoId }}</span>
            <span :class="['badge', item.type === OpportunityType.Buy ? 'badge-buy' : 'badge-sell']">
              {{ item.type === OpportunityType.Buy ? 'BUY' : 'SELL' }}
            </span>
          </div>
          <div class="item-col">
            <span class="label">Target</span>
            <span class="value">{{ formatPrice(item.targetPrice) }}</span>
          </div>
          <div class="item-col ml-auto">
            <span class="label">Created</span>
            <span class="value muted">{{ formatDate(item.createdAt) }}</span>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.opportunities {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 2.5rem;
}

.section-title {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--color-text);
  margin-bottom: 1rem;
}

.empty {
  color: var(--color-text-muted);
  font-size: 0.9rem;
  padding: 1rem 0;
}

.list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.item {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: 10px;
  padding: 1rem 1.5rem;
  display: flex;
  align-items: center;
  gap: 2rem;
  flex-wrap: wrap;
}

.item.triggered {
  border-left: 3px solid var(--color-warning);
}

.item-left {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  min-width: 160px;
}

.coin {
  font-weight: 700;
  color: var(--color-text);
  text-transform: capitalize;
  font-size: 0.95rem;
}

.badge {
  padding: 0.2rem 0.5rem;
  border-radius: 4px;
  font-size: 0.7rem;
  font-weight: 700;
  letter-spacing: 0.5px;
}

.badge-buy {
  background: rgba(34, 197, 94, 0.12);
  color: var(--color-success);
}

.badge-sell {
  background: rgba(239, 68, 68, 0.12);
  color: var(--color-danger);
}

.item-col {
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
  min-width: 110px;
}

.ml-auto {
  margin-left: auto;
}

.label {
  font-size: 0.7rem;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.value {
  font-size: 0.9rem;
  font-weight: 600;
  color: var(--color-text);
}

.value.muted {
  color: var(--color-text-soft);
  font-weight: 400;
}
</style>
