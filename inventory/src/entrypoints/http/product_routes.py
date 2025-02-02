from fastapi import APIRouter, Depends, HTTPException, Query
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from sqlalchemy.orm import Session
from typing import List
from src.models import ProductSchema, ProductUpdate, ProductCreate, Product
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
):
    products = get_products_by_skus(db, skus)
    if not products:
        raise HTTPException(status_code=404, detail="No products found")

    return products

@router.put("/product/{product_sku}", response_model=ProductUpdate)
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

@router.get("/product/{product_sku}", response_model=ProductSchema)
async def get_product(
        product_sku: str, db: Session = Depends(get_db),
        token: HTTPAuthorizationCredentials = Depends(security)
):
    role = get_role_from_token(token)
    if "WarehouseEmployee" not in role and "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    product = get_product_by_sku(db, product_sku)
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")
    return product

@router.get("/products")
async def get_all_products(db: Session = Depends(get_db)):
    # Pobieramy wszystkie produkty z bazy danych
    products = db.query(Product).all()

    if not products:
        raise HTTPException(status_code=404, detail="No products found")

    return products

@router.post("/product/", response_model=ProductCreate)
async def create_product(
        product: ProductCreate, db: Session = Depends(get_db), token: HTTPAuthorizationCredentials = Depends(security)
):
    # Check user role
    role = get_role_from_token(token)
    if "WarehouseEmployee" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    # Create new product (ensure index is passed correctly)
    new_product = Product(
        sku=Product.generate_sku(product.index),
        stock=product.stock,
        price=product.price,
        warehouse_id=product.warehouse_id,
        wage=product.wage,
        rozmiarX=product.rozmiarX,
        rozmiarY=product.rozmiarY,
        rozmiarZ=product.rozmiarZ,
        kolor=product.kolor,
    )

    # Add product to DB
    db.add(new_product)
    db.commit()
    db.refresh(new_product)

    # Convert the ORM object to the Pydantic response model
    return ProductCreate.model_validate(new_product)