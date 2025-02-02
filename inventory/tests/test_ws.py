import asyncio
import websockets


async def listen_to_websocket():
    token = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI3ZlVNa1AxNUZ6akZ5ZHdLVXFXbElvYVN1VE1KdWc4X1haQnc2STlXb3hVIn0.eyJleHAiOjE3Mzg0MjA4MDcsImlhdCI6MTczODQyMDUwNywiYXV0aF90aW1lIjoxNzM4NDE4OTQ3LCJqdGkiOiI2MzA5NzE5OC03NWZiLTRjNjgtYWVkNC1lMjM2NmJmZWU0OGEiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjgwMDEvcmVhbG1zL1Nob3AiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiOGQ4Nzc1YjYtYzRjYS00NGRlLTllNzctNjI2MGY0MWFjMDFiIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiU2hvcC1CYWNrZW5kIiwic2lkIjoiNGE1NDA1MDYtODdkMy00N2JlLWJhN2QtODI5YzZiMGI0MWExIiwiYWNyIjoiMCIsImFsbG93ZWQtb3JpZ2lucyI6WyIqIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJXYXJlaG91c2VFbXBsb3llZSIsIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy1zaG9wIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6ImVtYWlsIHByb2ZpbGUiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsIm5hbWUiOiJNYWdhIFp5biIsInByZWZlcnJlZF91c2VybmFtZSI6Im1hZ2F6eW5AZ21haWwuY29tIiwiYXBwUm9sZSI6WyJXYXJlaG91c2VFbXBsb3llZSIsIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy1zaG9wIiwidW1hX2F1dGhvcml6YXRpb24iXSwiZ2l2ZW5fbmFtZSI6Ik1hZ2EiLCJmYW1pbHlfbmFtZSI6Ilp5biIsImVtYWlsIjoibWFnYXp5bkBnbWFpbC5jb20ifQ.QyJwi6J1Sc0nOxcPesQmbCCXv5Hhb_OmOSejkis9_nI316xE-9d-ABmUNUZuKM-cN3Ps7nasyIGNh0hmXN58g0q4Yy-xF0IMdPs2lRNGPvuDLR0HxJaFdrouJD4KBtdu22k4zSFUsT-WqOYxjXiGyBGkTBxmmrl3j9s3Xap3AUkmizXdxZMyVTdqdThNJigThfiAW4fLnqx7wdrwMR0a5XOSq796tDmTOzGROmWNN6Zmckh_TN09zbcbO-i0BwV6y6lACv78pq4F7GWH7wpF6CnSGdXWWUrigi-VJ--DcOju2VhJdmFiEx9n-_Oql6HjnGzimbDVkp_TDOiO76za_A"
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