from fastapi import FastAPI, WebSocket, WebSocketDisconnect, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from sqlalchemy.orm import Session
from jose import jwt
import crud, models, schemas, dependencies, database
from database import engine
from database import SessionLocal

app = FastAPI(
    title="Warehouse API",
    description="API do zarządzania magazynem i powiadomieniami",
    version="1.0",
    docs_url="/docs",
    redoc_url="/redoc"
)

models.Base.metadata.create_all(bind=engine)

# OAuth2
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

# Zbiór aktywnych połączeń WebSocket
active_connections = set()


def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Funkcja do weryfikacji tokenu JWT i wyciągania roli
def get_role_from_token(token: str):
    try:
        # Klucz sekretu używany do podpisania tokenu
        secret_key = "SECRET_KEY"  # Zastąp tym, który masz w swoim systemie
        payload = jwt.decode(token, secret_key, algorithms=["HS256"])
        return payload.get("role")
    except jwt.ExpiredSignatureError:
        raise HTTPException(status_code=401, detail="Token has expired")
    except jwt.JWTError:
        raise HTTPException(status_code=401, detail="Invalid token")

async def notify_low_stock(product_id: int, db: Session):
    product = db.query(models.Product).filter(models.Product.id == product_id).first()
    notification = db.query(models.Notification).filter(models.Notification.product_id == product_id).first()

    if product and notification and product.stock < notification.minimum_stock:
        message = {
            "type": "low_stock",
            "product_id": product.id,
            "product_name": product.name,
            "message": notification.message
        }

        # Wysyłanie powiadomienia do wszystkich połączonych WebSocketów
        for connection in active_connections:
            await connection.send_json(message)


@app.put("/product/{product_id}", response_model=schemas.Product)
async def update_product_stock(
        product_id: int, product_update: schemas.ProductUpdate, db: Session = Depends(get_db),
        token: str = Depends(oauth2_scheme)
):
    role = dependencies.get_user_role(db, token)
    if role != "warehouse_worker":
        raise HTTPException(status_code=403, detail="Permission denied")

    # Aktualizacja stanu magazynowego
    product = crud.update_product_stock(db, product_id, product_update)
    await notify_low_stock(product_id, db)
    return product


@app.post("/notification/", response_model=schemas.NotificationCreate)
async def create_notification(notification: schemas.NotificationCreate, db: Session = Depends(get_db),
                              token: str = Depends(oauth2_scheme)):
    role = dependencies.get_user_role(db, token)
    if role != "admin":
        raise HTTPException(status_code=403, detail="Permission denied")
    return crud.create_notification(db, notification)


@app.websocket("/ws/notifications")
async def websocket_endpoint(websocket: WebSocket, token: str, db: Session = Depends(get_db)):
    try:
        payload = jwt.decode(token, "SECRET_KEY", algorithms=["HS256"])
        role = payload.get("role")

        if role != "warehouse_worker":
            await websocket.close()
            return

        # Akceptowanie połączenia WebSocket
        await websocket.accept()
        active_connections.add(websocket)

        # Nasłuchiwanie połączenia do zakończenia
        while True:
            try:
                # Możemy tu dodać jakąkolwiek logikę w przypadku zapytań z WebSocket, np. odpowiedź na wiadomości
                data = await websocket.receive_text()
                print(f"Received data: {data}")
            except WebSocketDisconnect:
                active_connections.remove(websocket)
                break

    except Exception as e:
        print(f"WebSocket error: {e}")
        await websocket.close()
