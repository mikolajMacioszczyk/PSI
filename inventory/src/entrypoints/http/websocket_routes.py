from fastapi import WebSocket, WebSocketDisconnect
import jwt
from src.common import active_connections

async def websocket_endpoint(websocket: WebSocket):
    try:
        token = websocket.query_params.get("token")
        if not token:
            await websocket.close()
            return

        decoded_token = jwt.decode(token, options={"verify_signature": False})
        print(f"Decoded token: {decoded_token}")  # <-- SPRAWDŹ TO

        role = decoded_token.get("realm_access", {}).get("roles", [])
        print(f"User roles: {role}")  # <-- I TO

        if "WarehouseEmployee" not in role:
            print("User does not have required role.")
            await websocket.close()
            return

        await websocket.accept()
        active_connections.add(websocket)

        while True:
            try:
                data = await websocket.receive_text()
                print(f"Received data: {data}")
            except WebSocketDisconnect:
                active_connections.remove(websocket)
                break

    except Exception as e:
        print(f"WebSocket error: {e}")
        await websocket.close()