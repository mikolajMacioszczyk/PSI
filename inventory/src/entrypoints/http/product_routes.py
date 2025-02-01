from fastapi import APIRouter, Depends, HTTPException, Query
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from sqlalchemy.orm import Session
from typing import List
from src.models import ProductSchema, ProductUpdate, ProductCreate
from src.repositories.relational_db.product_repository import get_products_by_skus, get_product_by_sku
from src.repositories.relational_db import get_db
from src.common.auth import get_role_from_token
from src.usecases import notify_low_stock
router = APIRouter()
security = HTTPBearer()

@router.get("/products/batch", response_model=List[ProductSchema])
async def get_products(
    skus: List[str] = Query(...),
    db: Session = Depends(get_db),
    token: HTTPAuthorizationCredentials = Depends(security)
):
    role = get_role_from_token(token)
    if "WarehouseEmployee" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    products = get_products_by_skus(db, skus)
    if not products:
        raise HTTPException(status_code=404, detail="No products found")

    return products

@router.put("/product/{product_sku}", response_model=ProductCreate)
async def update_product_stock(
        product_sku: str, product_update: ProductUpdate, db: Session = Depends(get_db),
        token: HTTPAuthorizationCredentials = Depends(security)
):
    role = get_role_from_token(token)
    if "WarehouseEmployee" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    product = get_product_by_sku(db, product_sku)
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")

    if product_update.stock is not None:
        product.stock = product_update.stock

    db.commit()
    db.refresh(product)

    await notify_low_stock(product_sku, db)  # Pass sku instead of product_id

    return product