from fastapi import FastAPI, HTTPException, WebSocket, WebSocketDisconnect
from pydantic import BaseModel

app = FastAPI()

# In-memory storage for inventory items
inventory = {
    "item1": {"name": "Widget", "quantity": 10, "threshold": 5},
    "item2": {"name": "Gadget", "quantity": 20, "threshold": 10},
}


class UpdateInventoryRequest(BaseModel):
    item_id: str
    quantity: int


# WebSocket connection manager
class ConnectionManager:
    def __init__(self):
        self.active_connections: list[WebSocket] = []

    async def connect(self, websocket: WebSocket):
        await websocket.accept()
        self.active_connections.append(websocket)

    def disconnect(self, websocket: WebSocket):
        self.active_connections.remove(websocket)

    async def send_message(self, message: str):
        for connection in self.active_connections:
            await connection.send_text(message)


manager = ConnectionManager()


async def send_notification(item_id: str, item_name: str, quantity: int):
    """
    Send notifications about low stock using WebSocket.
    """
    message = f"ALERT: Low stock for {item_name} ({item_id}). Remaining: {quantity} units."
    await manager.send_message(message)


@app.get("/inventory")
def get_inventory():
    """Retrieve the current inventory status."""
    return inventory


@app.put("/inventory/update")
async def update_inventory(request: UpdateInventoryRequest):
    """
    Update the quantity of a specific item.
    If the quantity falls below the threshold, trigger a notification.
    """
    if request.item_id not in inventory:
        raise HTTPException(status_code=404, detail="Item not found")

    # Update the inventory
    inventory[request.item_id]["quantity"] = request.quantity

    # Check for low stock and send notification
    if request.quantity <= inventory[request.item_id]["threshold"]:
        await send_notification(
            request.item_id,
            inventory[request.item_id]["name"],
            request.quantity,
        )

    return {"message": f"Updated {request.item_id} to quantity {request.quantity}"}


@app.get("/inventory/low-stock")
def check_low_stock():
    """
    Retrieve a list of items with low stock (below or equal to the threshold).
    """
    low_stock_items = {
        item_id: item for item_id, item in inventory.items()
        if item["quantity"] <= item["threshold"]
    }
    return low_stock_items


@app.post("/inventory/notify-low-stock")
async def notify_low_stock():
    """
    Check for items with low stock and send notifications for each.
    """
    low_stock_items = check_low_stock()
    for item_id, item in low_stock_items.items():
        await send_notification(item_id, item["name"], item["quantity"])

    return {"message": "Notifications sent for low-stock items"}


@app.websocket("/ws/notifications")
async def websocket_endpoint(websocket: WebSocket):
    """
    WebSocket endpoint to send real-time notifications.
    """
    await manager.connect(websocket)
    try:
        while True:
            # Keep the WebSocket connection alive
            await websocket.receive_text()
    except WebSocketDisconnect:
        manager.disconnect(websocket)
