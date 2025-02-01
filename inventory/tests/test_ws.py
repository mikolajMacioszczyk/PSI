import asyncio
import websockets


async def listen_to_websocket():
    token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI3ZlVNa1AxNUZ6akZ5ZHdLVXFXbElvYVN1VE1KdWc4X1haQnc2STlXb3hVIn0.eyJleHAiOjE3MzgzNTg0NTIsImlhdCI6MTczODM1ODE1MiwiYXV0aF90aW1lIjoxNzM4MzU3MjkyLCJqdGkiOiJhZDdlZmZiOC0xNWM1LTRjYzQtODgyMC1hNTliYzg2NDRiMDQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjgwMDEvcmVhbG1zL1Nob3AiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNjg1ZWQyNDgtOTZhMi00ZmY3LWFiYjQtMmE5OTMzNDcxNTgwIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiU2hvcC1CYWNrZW5kIiwic2lkIjoiM2ExOTlhNGQtZDJmOC00Y2Y3LWJjYzktNzk0ZWU4NzBiZDk1IiwiYWNyIjoiMCIsImFsbG93ZWQtb3JpZ2lucyI6WyIqIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJXYXJlaG91c2VFbXBsb3llZSIsIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy1zaG9wIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6ImVtYWlsIHByb2ZpbGUiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsIm5hbWUiOiJNYWdhIFp5biIsInByZWZlcnJlZF91c2VybmFtZSI6Im1hZ2F6eW5pZXJAdGVzdC5jb20iLCJhcHBSb2xlIjpbIldhcmVob3VzZUVtcGxveWVlIiwib2ZmbGluZV9hY2Nlc3MiLCJkZWZhdWx0LXJvbGVzLXNob3AiLCJ1bWFfYXV0aG9yaXphdGlvbiJdLCJnaXZlbl9uYW1lIjoiTWFnYSIsImZhbWlseV9uYW1lIjoiWnluIiwiZW1haWwiOiJtYWdhenluaWVyQHRlc3QuY29tIn0.XlrFlpf3suGOMkGBjf6HvOZwPlrV4XnCZt0iLc7iiRHmz-rukC0X3oHwwUJ_tNGgFq_9YkdtkfbAhw3DdwSt5EHvmbZevul4IiltAY4b2r2xc5Qy-WEnB4-GEWTO669s-ujcBWtIwL21WW9E3v7lOpWVE4TpwP4lWiV09IvI6pWdnDJqg565ns0R0_sKqCF59S-V03MxAsdg9ZjTlV5wjeMKhxDUO5HGJ4guXqIC6v6kmBc0NWHZJGkL0vB9pIRR2i-nzqBsvV8m-4uByoXj9EkKO8HDx9gIM2s2EWJNI0dNZWSZ6mW_SA6SC6L6sXcMq4x4otRMNSaF5KnKEt49Xg"
    uri = f"ws://127.0.0.1:5000/ws/notifications?token={token}"

    # Połączenie z WebSocket
    async with websockets.connect(uri) as websocket:
        print(f"Connected to {uri}")

        while True:
            try:
                # Oczekiwanie na dane
                message = await websocket.recv()
                print(f"Received message: {message}")

                # Możesz dodać dodatkową logikę do przetwarzania wiadomości
            except websockets.exceptions.ConnectionClosed:
                print("Connection closed by server.")
                break


async def main():
    await listen_to_websocket()


if __name__ == "__main__":
    asyncio.run(main())