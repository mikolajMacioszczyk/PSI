from fastapi import HTTPException
from fastapi.security import HTTPAuthorizationCredentials
import jwt

def get_role_from_token(authorization: HTTPAuthorizationCredentials):
    token = authorization.credentials  # Wyciąganie tokena z nagłówka autoryzacji
    try:
        # Dekodowanie tokena bez weryfikacji podpisu
        payload = jwt.decode(token, options={"verify_signature": False}, algorithms=["RS256"])
        # Debugowanie: Sprawdź zawartość tokena
        print("Decoded payload:", payload)
        # Sprawdź, czy token zawiera rolę
        roles = payload.get("realm_access", {}).get("roles", [])
        print("Roles from token:", roles)
        if roles:
            return roles
        raise HTTPException(status_code=403, detail="No role found in the token")

    except jwt.ExpiredSignatureError:
        raise HTTPException(status_code=401, detail="Token has expired")
    except jwt.JWTError:
        raise HTTPException(status_code=401, detail="Invalid token")