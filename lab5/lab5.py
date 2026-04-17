from __future__ import annotations

import argparse
import json
import string
import sys
from pathlib import Path

SCRIPTS_DIR = Path(__file__).resolve().parent / "scripts"
sys.path.insert(0, str(SCRIPTS_DIR))

from crypto import b64decode, xor_bytes  # noqa: E402
from prng import GeneratorSpec, iter_keystream_bytes  # noqa: E402


def load_variants(path: Path) -> list[dict[str, object]]:
    data = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(data, list):
        raise ValueError("variants.json must be a list")
    return data


def xor_decrypt(*, ciphertext: bytes, spec: GeneratorSpec, seed: int) -> bytes:
    it = iter_keystream_bytes(spec=spec, seed=seed)
    ks = bytes(next(it) for _ in range(len(ciphertext)))
    return xor_bytes(ciphertext, ks)


def looks_plausible_plaintext(b: bytes) -> bool:
    if not b.startswith(b"IB{"):
        return False
    try:
        s = b.decode("utf-8")
    except UnicodeDecodeError:
        return False

    printable = set(string.printable) | set("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя")
    good = sum(1 for ch in s if ch in printable or ch in "\n\r\t")
    return good / max(1, len(s)) > 0.85


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Пример решения: перебор seed по секундам в окне времени."
    )
    parser.add_argument("--id", type=int, required=True, help="Номер варианта (1..20)")
    parser.add_argument(
        "--variants",
        type=Path,
        default=Path(__file__).resolve().parent / "output" / "variants.json",
        help="Путь к variants.json",
    )
    args = parser.parse_args()

    variants = load_variants(args.variants)
    v = next((x for x in variants if int(x["variant_id"]) == args.id), None)
    if v is None:
        raise SystemExit(f"Variant id={args.id} not found in {args.variants}")

    spec = GeneratorSpec(
        generator_id=int(v["generator_id"]),
        params=dict(v.get("generator_params", {})),
    )
    t_from = int(v["time_from_unix"])
    t_to = int(v["time_to_unix"])
    ciphertext = b64decode(str(v["ciphertext_b64"]))

    for seed in range(t_from, t_to + 1):
        plain = xor_decrypt(ciphertext=ciphertext, spec=spec, seed=seed)
        if looks_plausible_plaintext(plain):
            print(seed)
            print(plain.decode("utf-8"))
            return 0

    return 1


if __name__ == "__main__":
    raise SystemExit(main())

