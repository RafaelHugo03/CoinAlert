import type { OpportunityTriggeredEvent } from '@/types/opportunity'
import { OpportunityType } from '@/types/opportunity'

export function useNotifications() {
  async function requestPermission(): Promise<void> {
    if (!('Notification' in window)) return
    if (Notification.permission === 'default') {
      await Notification.requestPermission()
    }
  }

  function notifyTriggered(event: OpportunityTriggeredEvent): void {
    if (!('Notification' in window) || Notification.permission !== 'granted') return

    const typeLabel = event.type === OpportunityType.Buy ? 'BUY' : 'SELL'
    const coin = event.cryptoId.charAt(0).toUpperCase() + event.cryptoId.slice(1)
    const fmt = (v: number) =>
      new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 2 }).format(v)

    new Notification(`${typeLabel} Opportunity Triggered — ${coin}`, {
      body: `Target: ${fmt(event.targetPrice)}  |  Current: ${fmt(event.currentPrice)}`,
      icon: '/favicon.ico',
      tag: event.id,
    })
  }

  return { requestPermission, notifyTriggered }
}
