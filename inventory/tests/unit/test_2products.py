import pytest
from fastapi.testclient import TestClient
from src.main import app
from unittest.mock import MagicMock
from sqlalchemy.orm import Session

client = TestClient(app)
WAREHOUSE_TOKEN ="eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJJeU5iYVo5d0JJREJnN05lVXgxdEEtdlU0M2VmakFIU2U0emExTGd6bTNZIn0.eyJleHAiOjE3Mzg1MzMxNjgsImlhdCI6MTczODUzMjg2OCwiYXV0aF90aW1lIjoxNzM4NTMyODY3LCJqdGkiOiI2NGRlZDM1MS04MjBkLTRiMzAtODMyZC1jZmNiNTliZGY4NjEiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjgwMDEvcmVhbG1zL1Nob3AiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNzRmZWE0NzMtY2Y1My00YWZmLWFkNDQtMWY3NmVjNGIyZGJjIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiU2hvcC1CYWNrZW5kIiwic2lkIjoiODliMWFmNjAtOTQxNi00MjI2LWIwZDQtMDkxYTdiOTJmMjdiIiwiYWNyIjoiMSIsImFsbG93ZWQtb3JpZ2lucyI6WyIqIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJXYXJlaG91c2VFbXBsb3llZSIsIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy1zaG9wIiwidW1hX2F1dGhvcml6YXRpb24iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6ImVtYWlsIHByb2ZpbGUiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsIm5hbWUiOiJNYWdhIFp5biIsInByZWZlcnJlZF91c2VybmFtZSI6Im1hZ2F6eW5AZ21haWwuY29tIiwiYXBwUm9sZSI6WyJXYXJlaG91c2VFbXBsb3llZSIsIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy1zaG9wIiwidW1hX2F1dGhvcml6YXRpb24iXSwiZ2l2ZW5fbmFtZSI6Ik1hZ2EiLCJmYW1pbHlfbmFtZSI6Ilp5biIsImVtYWlsIjoibWFnYXp5bkBnbWFpbC5jb20ifQ.Ow7tvTJ4nh8Y0TngcS521cF0QkQCPXXyJEmz8ifYQ0tZQEyPrfdAqCV21bQZbX5gbX32GbU9166E_h5Dq_c8c6slrk56mcOLfZW24x48yCMgjjJRwEnbI4CB-P9wnbW1OpAhnPyHQIBC9EV10qYnbAIqiGgGRB0by6WwAb9VUIgd8y_rF9tgKL_kV6BWWTg12zbmDqUwBn_ELFq9xUQ0kbMyMJyuGNX8TNPoer6OZWsCLsxbxpC3htTiPMG23dSajY2ZyTFJE6yRxQSQZLXbQREzBxh4VpOROoaflb2CkI0kDC2wnA12so3NyJ1v4auLGieSuXIFd6G_pkP7i-9BDg"

def test_create_product():
    product_data = {
        "index": 900,
        "stock": 100,
        "price": 10.0,
        "warehouse_id": 1,
        "wage": 15.0,
        "rozmiarX": 10,
        "rozmiarY": 20,
        "rozmiarZ": 30,
        "kolor": "red"
    }
    response = client.post("/api/product/", json=product_data, headers={"Authorization": f"Bearer {WAREHOUSE_TOKEN}"})

    assert response.status_code == 200
    assert response.json()["stock"] == 100


def test_update_product_stock():
    product_update_data = {
        "stock": 50
    }
    response = client.put("/api/product/00000000-0000-0000-0000-000000000900", json=product_update_data, headers={"Authorization": f"Bearer {WAREHOUSE_TOKEN}"})
    assert response.status_code == 200
    assert response.json()["stock"] == 50


def test_get_product():
    response = client.get("/api/product/00000000-0000-0000-0000-000000000900", headers={"Authorization": f"Bearer {WAREHOUSE_TOKEN}"})

    assert response.status_code == 200
    assert response.json()["sku"] == "00000000-0000-0000-0000-000000000900"


def test_get_products_by_warehouse():
    response = client.get("/api/products/warehouse/1", headers={"Authorization": f"Bearer {WAREHOUSE_TOKEN}"})

    assert response.status_code == 200
    assert "00000000-0000-0000-0000-000000000900" in response.json()


def test_get_all_products():
    response = client.get("/api/products")

    assert response.status_code == 200
    assert len(response.json()) > 0
