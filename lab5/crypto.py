from __future__ import annotations
import base64


def b64decode(s: str) -> bytes:
    return base64.b64decode(s)


def xor_bytes(a: bytes, b: bytes) -> bytes:
    return bytes(x ^ y for x, y in zip(a, b))
