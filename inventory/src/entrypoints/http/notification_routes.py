from fastapi import APIRouter, Depends, HTTPException
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from sqlalchemy.orm import Session
from src.models import NotificationCreate
from src.repositories.relational_db.notification_repository import get_notification_by_product_id
from src.repositories.relational_db import get_db
from src.common.auth import get_role_from_token

router = APIRouter()
security = HTTPBearer()

@router.post("/notification/", response_model=NotificationCreate)
async def create_notification(
    notification: NotificationCreate,
    db: Session = Depends(get_db),
    token: HTTPAuthorizationCredentials = Depends(security)
):
    role = get_role_from_token(token)
    if "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    existing_notification = get_notification_by_product_id(db, notification.product_id)

    if existing_notification:
        existing_notification.message = notification.message
        existing_notification.minimum_stock = notification.minimum_stock
        db.commit()
        db.refresh(existing_notification)
        return existing_notification
    else:
        new_notification = NotificationCreate(
            product_id=notification.product_id,
            message=notification.message,
            minimum_stock=notification.minimum_stock
        )
        db.add(new_notification)
        db.commit()
        db.refresh(new_notification)
        return new_notification
