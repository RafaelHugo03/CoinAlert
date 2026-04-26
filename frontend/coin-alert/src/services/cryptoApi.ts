import type { CryptoPrice } from '@/types/crypto'

const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5273/api'


async function getAllCryptoPrices(): Promise<CryptoPrice[]> {
  const res = await fetch(`${BASE_URL}/crypto/prices/fetch`, {
    method: 'POST',
  })

  if (!res.ok) throw new Error('Failed to post crypto prices')

  return res.json()
}

export const api = { getAllCryptoPrices }
