from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from src.models import Warehouse
from src.repositories.relational_db import get_db
from fastapi.security import HTTPAuthorizationCredentials
from src.common.auth import get_role_from_token
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials

router = APIRouter()
security = HTTPBearer()

@router.get("/warehouses")
async def get_all_warehouses(db: Session = Depends(get_db), token: HTTPAuthorizationCredentials = Depends(security)):
    role = get_role_from_token(token)
    if "WarehouseEmployee" not in role and "Admin" not in role:
        raise HTTPException(status_code=403, detail="Permission denied")

    warehouses = db.query(Warehouse).all()

    if not warehouses:
        raise HTTPException(status_code=404, detail="No warehouses found")

    return warehouses