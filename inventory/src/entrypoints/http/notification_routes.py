from fastapi import APIRouter, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from sqlalchemy.orm import Session
from src.models import NotificationCreate, Notification, NotificationUpdate
from src.repositories.relational_db.notification_repository import get_notification_by_product_id
from src.repositories.relational_db import get_db
from src.common.auth import get_role_from_token

router = APIRouter()
security = HTTPBearer()

@router.put("/notification/", response_model=NotificationUpdate)
async def update_notification(
    notification: NotificationUpdate,
    db: Session = Depends(get_db),
    token: HTTPAuthorizationCredentials = Depends(security)
):
    role = get_role_from_token(token)
    if "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    existing_notification = get_notification_by_product_id(db, notification.product_id)

    existing_notification.message = notification.message
    existing_notification.minimum_stock = notification.minimum_stock
    db.commit()
    db.refresh(existing_notification)
    return existing_notification

@router.post("/notification/", response_model=NotificationCreate)
async def create_notification(
    notification: NotificationCreate,
    db: Session = Depends(get_db),
    token: HTTPAuthorizationCredentials = Depends(security)
):
    role = get_role_from_token(token)
    if "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    # Sprawdzamy, czy powiadomienie dla danego produktu już istnieje
    existing_notification = get_notification_by_product_id(db, notification.product_id)

    if existing_notification:
        # Jeśli istnieje, aktualizujemy
        existing_notification.message = notification.message
        existing_notification.minimum_stock = notification.minimum_stock
        db.commit()
        db.refresh(existing_notification)
        return existing_notification  # Zwracamy zaktualizowane powiadomienie
    else:
        # Jeśli nie istnieje, tworzymy nowe
        new_notification = Notification(  # Tworzymy instancję klasy Notification (nie NotificationCreate)
            product_id=notification.product_id,
            message=notification.message,
            minimum_stock=notification.minimum_stock
        )
        db.add(new_notification)
        db.commit()
        db.refresh(new_notification)
        return new_notification  # Zwracamy utworzone powiadomienie

@router.get("/notifications")
async def get_notifications(
    db: Session = Depends(get_db),
    token: HTTPAuthorizationCredentials = Depends(security)
):
    # Sprawdzamy rolę użytkownika
    role = get_role_from_token(token)
    if "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    # Pobieramy wszystkie powiadomienia z bazy danych
    notifications = db.query(Notification).all()

    if not notifications:
        raise HTTPException(status_code=404, detail="No notifications found")

    return notifications

@router.get("/notifications/{notification_id}")
async def get_notification_by_id(
    notification_id: int,  # Parametr id powiadomienia
    db: Session = Depends(get_db),
    token: HTTPAuthorizationCredentials = Depends(security)
):
    # Sprawdzamy rolę użytkownika
    role = get_role_from_token(token)
    if "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    # Pobieramy powiadomienie po id
    notification = db.query(Notification).filter(Notification.id == notification_id).first()

    if not notification:
        raise HTTPException(status_code=404, detail="Notification not found")

    return notification

