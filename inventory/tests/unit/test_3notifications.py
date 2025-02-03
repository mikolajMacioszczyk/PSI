import pytest
from fastapi.testclient import TestClient
from src.main import app
from unittest.mock import MagicMock
from sqlalchemy.orm import Session

client = TestClient(app)

ADMIN_TOKEN = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJJeU5iYVo5d0JJREJnN05lVXgxdEEtdlU0M2VmakFIU2U0emExTGd6bTNZIn0.eyJleHAiOjE3Mzg1MzE1NDAsImlhdCI6MTczODUzMTI0MCwiYXV0aF90aW1lIjoxNzM4NTMxMjM5LCJqdGkiOiI0ZTVkYjlhNS05NjQ0LTRhMGQtODhiZS03MWQ4YmY2NjI2NDkiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjgwMDEvcmVhbG1zL1Nob3AiLCJhdWQiOiJhY2NvdW50Iiwic3ViIjoiNWU4N2YyMGYtODM3OS00ZDU1LTlkZTgtYWQ2OTFhZGRjZDMxIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiU2hvcC1CYWNrZW5kIiwic2lkIjoiNzRlOTBjMTgtM2UxMC00OWQwLWE2ZDItYjVlNGRlNmMwMjgzIiwiYWNyIjoiMSIsImFsbG93ZWQtb3JpZ2lucyI6WyIqIl0sInJlYWxtX2FjY2VzcyI6eyJyb2xlcyI6WyJvZmZsaW5lX2FjY2VzcyIsImRlZmF1bHQtcm9sZXMtc2hvcCIsInVtYV9hdXRob3JpemF0aW9uIiwiQWRtaW4iXX0sInJlc291cmNlX2FjY2VzcyI6eyJhY2NvdW50Ijp7InJvbGVzIjpbIm1hbmFnZS1hY2NvdW50IiwibWFuYWdlLWFjY291bnQtbGlua3MiLCJ2aWV3LXByb2ZpbGUiXX19LCJzY29wZSI6ImVtYWlsIHByb2ZpbGUiLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsIm5hbWUiOiJhZG1pbiBhZG1pbiIsInByZWZlcnJlZF91c2VybmFtZSI6ImFkbWluQGdtYWlsLmNvbSIsImFwcFJvbGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJkZWZhdWx0LXJvbGVzLXNob3AiLCJ1bWFfYXV0aG9yaXphdGlvbiIsIkFkbWluIl0sImdpdmVuX25hbWUiOiJhZG1pbiIsImZhbWlseV9uYW1lIjoiYWRtaW4iLCJlbWFpbCI6ImFkbWluQGdtYWlsLmNvbSJ9.t838SQkQ5oF-evH-tavYZrldsKOQVs6_G6RpJKsnwvXySv8Nel_vUll4CXrbSulWd_p9gzD0eFtp2yrO-gFnnIvGrJPf_HUPQje3QCjiOH3lAdACncX54Ct_4VPB6WmdNKPStfMuzRBx7lw2PEczQAf8wT4xqaw-ECtHrjuqHvTrF-EXXluVs6hZ-gFiGEC5mO7uRCtSCuMcPWcZWojNYfuInP9rZW7yDCb0AhxDy_DCMhivW7widJp2mSL70nXuOOVuiL8sH5CGjyBv57oKxmwVXYxQA34Fycn6qjhL9CRjyemkWBIis7bIxskw1I0OrObbpTwHPlxLgo1xoKaGtw"

def test_create_notification():
    notification_data = {
        "product_id": "00000000-0000-0000-0000-000000000900",
        "message": "Test message",
        "minimum_stock": 10,
        "active": True
    }

    response = client.post("/api/notification/", json=notification_data, headers={"Authorization": f"Bearer {ADMIN_TOKEN}"})

    assert response.status_code == 200
    assert response.json()["product_id"] == "00000000-0000-0000-0000-000000000900"
    assert response.json()["message"] == "Test message"
    assert response.json()["minimum_stock"] == 10
    assert response.json()["active"] is True

def test_update_notification():
    notification_update_data = {
        "product_id": "00000000-0000-0000-0000-000000000900",
        "message": "Updated message",
        "minimum_stock": 15
    }

    response = client.put("/api/notification/", json=notification_update_data,
                          headers={"Authorization": f"Bearer {ADMIN_TOKEN}"})

    assert response.status_code == 200
    assert response.json()["message"] == "Updated message"
    assert response.json()["minimum_stock"] == 15

def test_get_notifications():
    response = client.get("/api/notifications", headers={"Authorization": f"Bearer {ADMIN_TOKEN}"})

    assert response.status_code == 200
    assert len(response.json()) > 0
    assert response.json()[-1]["product_id"] == "00000000-0000-0000-0000-000000000900"
    assert response.json()[-1]["message"] == "Updated message"

def test_get_notification_by_id():
    response = client.get("/api/notifications/2", headers={"Authorization": f"Bearer {ADMIN_TOKEN}"})

    assert response.status_code == 200
    assert response.json()["product_id"] == "00000000-0000-0000-0000-000000000002"
    assert response.json()["message"] == "Low stock alert for product 00000000-0000-0000-0000-000000000002"
    assert response.json()["minimum_stock"] == 20
    assert response.json()["active"] is True
