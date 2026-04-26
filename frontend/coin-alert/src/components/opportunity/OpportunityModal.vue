<script setup lang="ts">
import { ref } from 'vue'
import { OpportunityType } from '@/types/opportunity'
import { api } from '@/services/opportunityApi'
import { useOpportunityStore } from '@/stores/opportunityStore'

const props = defineProps<{
  cryptoId: string
}>()

const emit = defineEmits<{
  close: []
}>()

const opportunityStore = useOpportunityStore()

const type = ref<OpportunityType>(OpportunityType.Buy)
const targetPrice = ref<number | null>(null)
const loading = ref(false)
const error = ref<string | null>(null)

async function submit() {
  if (!targetPrice.value || targetPrice.value <= 0) {
    error.value = 'Enter a valid target price'
    return
  }

  loading.value = true
  error.value = null

  try {
    const opportunity = await api.createOpportunity({
      cryptoId: props.cryptoId,
      type: type.value,
      targetPrice: targetPrice.value,
    })
    opportunityStore.addOpportunity(opportunity)
    emit('close')
  } catch {
    error.value = 'Failed to create opportunity. Try again.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="overlay" @click.self="emit('close')">
    <div class="modal">
      <div class="modal-header">
        <h2>New Oportunity — <span class="crypto-label">{{ cryptoId }}</span></h2>
        <button class="btn-close" @click="emit('close')">✕</button>
      </div>

      <div class="modal-body">
        <div class="field">
          <label>Type</label>
          <div class="toggle-group">
            <button
              :class="['toggle-btn', type === OpportunityType.Buy ? 'active-buy' : '']"
              @click="type = OpportunityType.Buy"
            >
              Buy
            </button>
            <button
              :class="['toggle-btn', type === OpportunityType.Sell ? 'active-sell' : '']"
              @click="type = OpportunityType.Sell"
            >
              Sell
            </button>
          </div>
        </div>

        <div class="field">
          <label for="target-price">Target Price (USD)</label>
          <input
            id="target-price"
            v-model.number="targetPrice"
            type="number"
            step="0.01"
            min="0"
            placeholder="e.g. 90000"
          />
        </div>

        <p v-if="error" class="error">{{ error }}</p>
      </div>

      <div class="modal-footer">
        <button class="btn-cancel" @click="emit('close')">Cancel</button>
        <button class="btn-submit" :disabled="loading" @click="submit">
          {{ loading ? 'Creating…' : 'Create Opportunity' }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.65);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
  backdrop-filter: blur(2px);
}

.modal {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: 16px;
  padding: 2rem;
  width: 100%;
  max-width: 420px;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h2 {
  font-size: 1.1rem;
  font-weight: 700;
  color: var(--color-text);
}

.crypto-label {
  color: var(--color-primary);
  text-transform: capitalize;
}

.btn-close {
  background: none;
  border: none;
  color: var(--color-text-muted);
  font-size: 1rem;
  cursor: pointer;
  padding: 0.25rem 0.5rem;
  border-radius: 6px;
  transition: color 0.15s;
  line-height: 1;
}

.btn-close:hover {
  color: var(--color-text);
}

.modal-body {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

label {
  font-size: 0.82rem;
  color: var(--color-text-soft);
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.4px;
}

.toggle-group {
  display: flex;
  gap: 0.5rem;
}

.toggle-btn {
  flex: 1;
  padding: 0.6rem;
  border-radius: 8px;
  border: 1px solid var(--color-border);
  background: var(--color-bg);
  color: var(--color-text-muted);
  font-weight: 600;
  cursor: pointer;
  transition: all 0.15s;
  font-size: 0.9rem;
}

.toggle-btn.active-buy {
  background: rgba(34, 197, 94, 0.12);
  border-color: var(--color-success);
  color: var(--color-success);
}

.toggle-btn.active-sell {
  background: rgba(239, 68, 68, 0.12);
  border-color: var(--color-danger);
  color: var(--color-danger);
}

input[type='number'] {
  padding: 0.65rem 0.9rem;
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: 8px;
  color: var(--color-text);
  font-size: 1rem;
  outline: none;
  transition: border-color 0.15s;
  width: 100%;
}

input[type='number']:focus {
  border-color: var(--color-primary);
}

input[type='number']::placeholder {
  color: var(--color-text-muted);
}

.error {
  color: var(--color-danger);
  font-size: 0.85rem;
}

.modal-footer {
  display: flex;
  gap: 0.75rem;
  justify-content: flex-end;
}

.btn-cancel {
  padding: 0.6rem 1.2rem;
  border-radius: 8px;
  border: 1px solid var(--color-border);
  background: none;
  color: var(--color-text-soft);
  cursor: pointer;
  font-weight: 600;
  font-size: 0.9rem;
  transition: all 0.15s;
}

.btn-cancel:hover {
  background: var(--color-border);
  color: var(--color-text);
}

.btn-submit {
  padding: 0.6rem 1.4rem;
  border-radius: 8px;
  border: none;
  background: var(--color-primary);
  color: #fff;
  cursor: pointer;
  font-weight: 700;
  font-size: 0.9rem;
  transition: background 0.15s;
}

.btn-submit:hover:not(:disabled) {
  background: var(--color-primary-dark);
}

.btn-submit:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
