<script setup lang="ts">
import { ref, computed } from 'vue'
import { useCryptoStore } from '@/stores/cryptoStore'
import CryptoCard from '@/components/crypto/CryptoCard.vue'
import OpportunityModal from '@/components/opportunity/OpportunityModal.vue'

const cryptoStore = useCryptoStore()
const coins = computed(() => Object.values(cryptoStore.prices))
const modalCryptoId = ref<string | null>(null)
</script>

<template>
  <div class="dashboard">
    <div class="section-header">
      <h1 class="section-title">Live Prices</h1>
      <span class="live-badge">● LIVE</span>
    </div>

    <div class="grid">
      <CryptoCard
        v-for="coin in coins"
        :key="coin.cryptoId"
        :crypto="coin"
        @create-opportunity="modalCryptoId = $event"
      />
    </div>

    <OpportunityModal
      v-if="modalCryptoId"
      :crypto-id="modalCryptoId"
      @close="modalCryptoId = null"
    />
  </div>
</template>

<style scoped>
.dashboard {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.section-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.section-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--color-text);
}

.live-badge {
  font-size: 0.75rem;
  font-weight: 700;
  color: var(--color-success);
  letter-spacing: 1px;
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0%,
  100% {
    opacity: 1;
  }
  50% {
    opacity: 0.35;
  }
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.25rem;
}
</style>
