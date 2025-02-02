from sqlalchemy.orm import Session
from src.models import Product

def get_products_by_skus(db: Session, skus: list):
    return db.query(Product).filter(Product.sku.in_(skus)).all()

def get_product_by_sku(db: Session, sku: str):
    return db.query(Product).filter(Product.sku == sku).first()
