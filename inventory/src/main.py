from fastapi import FastAPI, WebSocket, WebSocketDisconnect, Depends, HTTPException
from fastapi.security import HTTPBearer
from sqlalchemy.orm import Session
import jwt
import re
from fastapi.security import HTTPAuthorizationCredentials
from typing import List
from src.models import Product, Warehouse, Notification, ProductCreate
from src.models import ProductUpdate, NotificationCreate, ProductSchema, SKURequest
from src.repositories.relational_db import get_db, init_db
app = FastAPI(
    title="Warehouse API",
    description="API do zarządzania magazynem i powiadomieniami",
    version="1.0",
    docs_url="/docs",
    redoc_url="/redoc"
)
# Define HTTPBearer for Bearer token authentication
security = HTTPBearer()

# Zbiór aktywnych połączeń WebSocket
active_connections = set()


def get_role_from_token(authorization: HTTPAuthorizationCredentials):
    token = authorization.credentials  # Wyciąganie tokena z nagłówka autoryzacji
    try:
        # Dekodowanie tokena bez weryfikacji podpisu
        payload = jwt.decode(token, options={"verify_signature": False}, algorithms=["RS256"])
        # Debugowanie: Sprawdź zawartość tokena
        print("Decoded payload:", payload)
        # Sprawdź, czy token zawiera rolę
        roles = payload.get("realm_access", {}).get("roles", [])
        print("Roles from token:", roles)
        if roles:
            return roles
        raise HTTPException(status_code=403, detail="No role found in the token")

    except jwt.ExpiredSignatureError:
        raise HTTPException(status_code=401, detail="Token has expired")
    except jwt.JWTError:
        raise HTTPException(status_code=401, detail="Invalid token")


async def notify_low_stock(product_sku: str, db: Session):
    # Retrieve the product using SKU instead of id
    product = db.query(Product).filter(Product.sku == product_sku).first()

    # Check if product is found
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")

    # Retrieve the notification using product.id (product_id in Notification)
    notification = db.query(Notification).filter(Notification.product_id == product_sku).first()

    # If the product and notification exist and the stock is low
    if product and notification and product.stock < notification.minimum_stock:
        message = {
            "type": "low_stock",
            "product_sku": product.sku,
            "message": notification.message
        }

        # Send notification to all active WebSocket connections
        for connection in active_connections:
            await connection.send_json(message)


@app.put("/product/{product_sku}", response_model=ProductCreate)
async def update_product_stock(
        product_sku: str, product_update: ProductUpdate, db: Session = Depends(get_db),
        token: HTTPAuthorizationCredentials = Depends(security)
):
    # Extract role from the Bearer token
    role = get_role_from_token(token)  # Pass the token here
    if "WarehouseEmployee" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    # Retrieve the current product using SKU instead of id
    product = db.query(Product).filter(Product.sku == product_sku).first()  # Use sku instead of id
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")

    # Update stock and other fields if provided
    if product_update.stock is not None:
        product.stock = product_update.stock  # Allow increasing the stock

    # Commit changes to the database
    db.commit()
    db.refresh(product)

    # Notify about low stock
    await notify_low_stock(product_sku, db)  # Pass sku instead of product_id

    return product


@app.post("/products/batch", response_model=List[ProductSchema])
async def get_products_by_skus(
        sku_request: SKURequest,
        db: Session = Depends(get_db),
        token: HTTPAuthorizationCredentials = Depends(security)
):
    # Pobranie roli użytkownika z tokena
    role = get_role_from_token(token)
    if "WarehouseEmployee" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    # Pobranie produktów na podstawie listy SKU
    products = db.query(Product).filter(Product.sku.in_(sku_request.skus)).all()

    if not products:
        raise HTTPException(status_code=404, detail="No products found")

    return products

@app.post("/notification/", response_model=NotificationCreate)
async def create_notification(notification: NotificationCreate, db: Session = Depends(get_db),
                              token: HTTPAuthorizationCredentials = Depends(security)  ):
    # Extract role from the Bearer token
    role = get_role_from_token(token)
    if "Admin" not in role:
        print(f"Role from token: {role}")
        raise HTTPException(status_code=403, detail="Permission denied")

    # Check if the notification for the product already exists
    existing_notification = db.query(Notification).filter(
        Notification.product_id == notification.product_id).first()

    if existing_notification:
        # Update the existing notification
        existing_notification.message = notification.message
        existing_notification.minimum_stock = notification.minimum_stock
        db.commit()
        db.refresh(existing_notification)
        return existing_notification
    else:
        # Create a new notification
        new_notification = Notification(
            product_id=notification.product_id,  # This assumes product_id is still the correct field in Notification
            message=notification.message,
            minimum_stock=notification.minimum_stock
        )
        db.add(new_notification)
        db.commit()
        db.refresh(new_notification)
        return new_notification


# Funkcja pobierająca token
def get_token_from_websocket(headers: dict) -> str:
    # Użyj nagłówka "Authorization" w celu pozyskania tokenu
    auth_header = headers.get("Authorization")
    if auth_header is None:
        raise HTTPException(status_code=400, detail="Token not provided")

    # Zwykle w nagłówku Authorization mamy "Bearer <token>"
    match = re.match(r"Bearer (\S+)", auth_header)
    if match is None:
        raise HTTPException(status_code=400, detail="Invalid token format")

    return match.group(1)

@app.websocket("/ws/notifications")
async def websocket_endpoint(
    websocket: WebSocket,
    db: Session = Depends(get_db)
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
    init_db()
    uvicorn.run(app, host="127.0.0.1", port=5000)
