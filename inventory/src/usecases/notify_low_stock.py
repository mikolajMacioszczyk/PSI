from sqlalchemy.orm import Session
from src.models import Product, Notification
from src.common import active_connections

async def notify_low_stock(product_sku: str, db: Session):
    product = db.query(Product).filter(Product.sku == product_sku).first()

    if not product:
        print(f"Product with SKU {product_sku} not found")
        return

    notification = db.query(Notification).filter(Notification.product_id == product_sku).first()

    if notification and product.stock < notification.minimum_stock:
        message = {
            "type": "low_stock",
            "product_sku": product.sku,
            "message": notification.message
        }

        print(f"Sending low stock notification: {message}")

        for connection in active_connections:
            try:
                await connection.send_json(message)
            except Exception as e:
                print(f"Error sending WebSocket message: {e}")
                active_connections.remove(connection)  # Usunięcie uszkodzonych połączeń