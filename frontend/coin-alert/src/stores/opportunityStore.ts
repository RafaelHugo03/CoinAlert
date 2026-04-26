import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Opportunity, OpportunityTriggeredEvent } from '@/types/opportunity'
import { api } from '@/services/opportunityApi'

export const useOpportunityStore = defineStore('opportunity', () => {
  const opportunities = ref<Opportunity[]>([])
  const triggered = ref<OpportunityTriggeredEvent[]>([])

  async function fetchAll() {
    opportunities.value = await api.getOpportunities()
  }

  function addOpportunity(o: Opportunity) {
    opportunities.value.unshift(o)
  }

  return { opportunities, triggered, fetchAll, addOpportunity }
})
