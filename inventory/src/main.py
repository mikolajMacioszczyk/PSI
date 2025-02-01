from fastapi import FastAPI
from src.entrypoints.http.product_routes import router as product_router
from src.entrypoints.http.notification_routes import router as notification_router
from src.common import active_connections
from fastapi import WebSocket, WebSocketDisconnect
import jwt
from src.repositories.relational_db import init_db
app = FastAPI(
    title="Warehouse API",
    description="API do zarządzania magazynem i powiadomieniami",
    version="1.0"
)
init_db()
app.include_router(product_router, prefix="/api")
app.include_router(notification_router, prefix="/api")

# @app.websocket("/ws/notifications")
# async def websocket_notifications(websocket):
#     print("dupa")
#     await websocket_endpoint(websocket)

@app.websocket("/ws/notifications")
async def websocket_endpoint(
    websocket: WebSocket
):
    try:
        token = websocket.query_params.get("token")

        print(f"Token received: {token}")
        decoded_token = jwt.decode(token, options={"verify_signature": False})
        print(f"decoded_token: {decoded_token}")
        role = decoded_token.get("realm_access", {}).get("roles", [])

        print(f"User role: {role}")
        if "WarehouseEmployee" not in role:
            await websocket.close()
            return

        # Akceptowanie połączenia WebSocket
        await websocket.accept()
        active_connections.add(websocket)

        # Nasłuchiwanie połączenia do zakończenia
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

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="127.0.0.1", port=5000)

