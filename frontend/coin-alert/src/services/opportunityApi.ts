import type { CreateOpportunityDto, Opportunity } from '@/types/opportunity'

const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5273/api'

async function getOpportunities(): Promise<Opportunity[]> {
  const res = await fetch(`${BASE_URL}/opportunity`)
  if (!res.ok) throw new Error('Failed to fetch opportunities')
  return res.json()
}

async function createOpportunity(dto: CreateOpportunityDto): Promise<Opportunity> {
  const res = await fetch(`${BASE_URL}/opportunity`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  })
  if (!res.ok) throw new Error('Failed to create opportunity')
  return res.json()
}

export const api = { getOpportunities, createOpportunity }
