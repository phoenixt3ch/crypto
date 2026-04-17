from __future__ import annotations
import math
from dataclasses import dataclass, field
from typing import Iterator


@dataclass
class GeneratorSpec:
    generator_id: int
    params: dict = field(default_factory=dict)


def iter_keystream_bytes(spec: GeneratorSpec, seed: int) -> Iterator[int]:
    for R in _make_generator(spec, seed):
        T = int(R * (2 ** 32)) & 0xFFFFFFFF
        yield T & 0xFF
        yield (T >> 8) & 0xFF
        yield (T >> 16) & 0xFF
        yield (T >> 24) & 0xFF


def _make_generator(spec: GeneratorSpec, seed: int):
    gid = spec.generator_id
    p = spec.params
    dispatch = {
        1: _gen1, 2: _gen2, 3: _gen3, 4: _gen4, 5: _gen4,
        6: _gen6, 7: _gen7, 8: _gen8, 9: _gen9, 10: _gen10,
    }
    yield from dispatch[gid](p, seed)


def _frac(x: float) -> float:
    return x - math.floor(x)


def _gen1(params: dict, seed: int):
    a = int(params['a'])
    b = int(params['b'])
    scale_a = 10 ** a
    scale_b = 10 ** b
    x = seed % scale_a
    R = x / scale_a
    while True:
        yield R
        x = int(R * scale_a)
        t = (x * x) // scale_a
        x = t % scale_b
        R = x / scale_b


def _gen2(params: dict, seed: int):
    p = int(params['p'])
    M = int(params['M'])
    C = int(params.get('C', 0))
    U = (seed % (p - 1)) + 1
    while True:
        yield U / p
        U = (U * M + C) % p


def _gen3(params: dict, seed: int):
    p = int(params['p'])
    M = int(params['M'])
    U = (seed % (p - 1)) + 1
    while True:
        yield U / p
        U = (U * M) % p


def _gen4(params: dict, seed: int):
    p = int(params['p'])
    M = int(params['M'])
    u = (seed % (p - 1)) + 1
    while True:
        yield u / p
        u = (M * u) % p


def _gen6(params: dict, seed: int):
    R = ((seed % 1_000_000) + 1) / 1_000_001
    while True:
        yield R
        R = _frac(11 * R + math.pi)


def _gen7(params: dict, seed: int):
    n = int(params['n'])
    z = float(params['z0'])
    step = 10 ** (-n)
    R = ((seed % 1_000_000) + 1) / 1_000_001
    while True:
        yield R
        z = z + step
        R = _frac(R / z + math.pi)


def _gen8(params: dict, seed: int):
    n = int(params['n'])
    scale = 10 ** n
    R = ((seed % 1_000_000) + 1) / 1_000_001
    while True:
        yield R
        R = (1 / math.pi) * math.acos(math.cos(scale * R))


def _gen9(params: dict, seed: int):
    R0 = ((seed % 1_000_000) + 1) / 1_000_001
    yield R0
    i = 0
    while True:
        yield (1 / math.pi) * math.acos(math.cos((i + 100) * R0))
        i += 1


def _gen10(params: dict, seed: int):
    n = int(params['n'])
    coeff = (10 ** n) - 1
    R = ((seed % 1_000_000) + 1) / 1_000_001
    while True:
        yield R
        R = (1 / math.pi) * math.acos(math.cos(coeff * R))
