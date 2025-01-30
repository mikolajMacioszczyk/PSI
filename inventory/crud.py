from sqlalchemy.orm import Session
import models, schemas
from typing import Optional

# Funkcja do aktualizacji stanu magazynowego produktu
def update_product_stock(db: Session, product_id: int, stock: int, warehouse_id: Optional[int] = None):
    product = db.query(models.Product).filter(models.Product.id == product_id).first()
    if product:
        if stock is not None:
            product.stock = stock
        if warehouse_id is not None:
            product.warehouse_id = warehouse_id
        db.commit()
        db.refresh(product)
    return product

# Funkcja do tworzenia nowego produktu
def create_product(db: Session, product: schemas.ProductCreate):
    db_product = models.Product(
        name=product.name,
        stock=product.stock,
        price=product.price,
        warehouse_id=product.warehouse_id  # Przypisanie produktu do magazynu
    )
    db.add(db_product)
    db.commit()
    db.refresh(db_product)
    return db_product

# Funkcja do tworzenia nowego powiadomienia
def create_notification(db: Session, notification: schemas.NotificationCreate):
    db_notification = models.Notification(
        product_id=notification.product_id,
        message=notification.message,
        minimum_stock=notification.minimum_stock
    )
    db.add(db_notification)
    db.commit()
    db.refresh(db_notification)
    return db_notification

# Funkcja do edytowania istniejącego powiadomienia
def update_notification(db: Session, notification_id: int, notification: schemas.NotificationUpdate):
    db_notification = db.query(models.Notification).filter(models.Notification.id == notification_id).first()
    if db_notification:
        if notification.product_id is not None:
            db_notification.product_id = notification.product_id
        if notification.message is not None:
            db_notification.message = notification.message
        if notification.minimum_stock is not None:
            db_notification.minimum_stock = notification.minimum_stock
        if notification.active is not None:
            db_notification.active = notification.active
        db.commit()
        db.refresh(db_notification)
    return db_notification
