import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { CryptoPrice } from '@/types/crypto'
import { api } from '@/services/cryptoApi'

export const useCryptoStore = defineStore('crypto', () => {
  const prices = ref<Record<string, CryptoPrice>>({})

  async function init() {
    const data = await api.getAllCryptoPrices()
    for (const price of data) {
      prices.value[price.cryptoId] = price
    }
  }

  function updatePrice(update: CryptoPrice) {
    prices.value[update.cryptoId] = update
  }

  return { prices, init, updatePrice }
})
