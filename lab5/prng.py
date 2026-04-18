from __future__ import annotations
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
    p = spec.params
    yield from _gen4(p, seed)


def _gen4(params: dict, seed: int):
    p = int(params['p'])
    M = int(params['M'])
    u = (seed % (p - 1)) + 1
    while True:
        yield u / p
        u = (M * u) % p
