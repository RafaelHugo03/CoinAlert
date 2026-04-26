export enum OpportunityType {
  Buy = 0,
  Sell = 1,
}

export enum OpportunityStatus {
  Active = 0,
  Triggered = 1,
}

export interface Opportunity {
  id: string
  cryptoId: string
  type: OpportunityType
  targetPrice: number
  currentPrice: number
  status: OpportunityStatus
  createdAt: string
  triggeredAt: string | null
}

export interface CreateOpportunityDto {
  cryptoId: string
  type: OpportunityType
  targetPrice: number
}

export interface OpportunityTriggeredEvent {
  id: string
  cryptoId: string
  type: OpportunityType
  targetPrice: number
  currentPrice: number
  triggeredAt: string
}
