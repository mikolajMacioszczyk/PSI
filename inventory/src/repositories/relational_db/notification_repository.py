from sqlalchemy.orm import Session
from src.models import Notification

def get_notification_by_product_id(db: Session, product_id: str):
    return db.query(Notification).filter(Notification.product_id == product_id).first()
