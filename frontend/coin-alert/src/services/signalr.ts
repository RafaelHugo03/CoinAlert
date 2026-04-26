import * as signalR from '@microsoft/signalr'

const HUB_URL = import.meta.env.VITE_HUB_URL ?? 'http://localhost:5273/hubs/crypto-price'

export const connection = new signalR.HubConnectionBuilder()
  .withUrl(HUB_URL)
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Warning)
  .build()

export async function startConnection(): Promise<void> {
  if (connection.state === signalR.HubConnectionState.Disconnected) {
    await connection.start()
  }
}
