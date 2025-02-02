import asyncio
import websockets


async def listen_to_websocket():
    uri = f"ws://127.0.0.1:5000/ws/notifications"
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