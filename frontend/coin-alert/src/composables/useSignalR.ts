import { connection, startConnection } from '@/services/signalr'
import type {  OpportunityTriggeredEvent } from '@/types/opportunity'
import type { CryptoPrice } from '@/types/crypto'
import { useCryptoStore } from '@/stores/cryptoStore'
import { useNotifications } from '@/composables/useNotifications'

export function useSignalR() {
  const cryptoStore = useCryptoStore()
  const { requestPermission, notifyTriggered } = useNotifications()

  async function init() {
    await requestPermission()

    connection.on('ReceivePriceUpdate', (update: CryptoPrice) => {
      cryptoStore.updatePrice(update)
    })

    connection.on('OpportunityTriggered', (event: OpportunityTriggeredEvent) => {
      notifyTriggered(event)
    })

    await startConnection()
  }

  return { init }
}
