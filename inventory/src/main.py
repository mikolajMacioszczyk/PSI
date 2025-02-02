from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from starlette.websockets import WebSocket, WebSocketDisconnect
from src.common import active_connections
from src.entrypoints.http.product_routes import router as product_router
from src.entrypoints.http.notification_routes import router as notification_router
from src.entrypoints.http.warehouse_routes import router as warehouse_router


from src.repositories.relational_db import init_db
app = FastAPI(
    title="Warehouse API",
    description="API do zarządzania magazynem i powiadomieniami",
    version="1.0"
)
# Konfiguracja CORS, aby umożliwić zapytania z określonego źródła (np. http://localhost:4200)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:4200"],  # Zezwala tylko na zapytania z tej domeny
    allow_credentials=True,
    allow_methods=["*"],  # Możesz ograniczyć do określonych metod jak GET, POST
    allow_headers=["*"],  # Możesz określić, które nagłówki są dozwolone
)

init_db()
app.include_router(product_router, prefix="/api")
app.include_router(notification_router, prefix="/api")

app.include_router(warehouse_router, prefix="/api")

@app.websocket("/ws/notifications")
async def websocket_endpoint(
    websocket: WebSocket
):
    try:
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

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="127.0.0.1", port=5000)

