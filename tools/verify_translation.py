import json
import re
from pathlib import Path

root = Path(__file__).resolve().parents[1] / 'src/CAERO/Content'

def metadata(value):
    if isinstance(value, dict):
        return {k: ('<text>' if k in ('name', 'theme', 'language') else
                    ['<page>'] * len(v) if k in ('pages', 'yesPages', 'noPages') else metadata(v))
                for k, v in value.items()}
    if isinstance(value, list):
        return [metadata(v) for v in value]
    return value

for name in ['dialogue', 'special-islands']:
    ja = json.loads((root / f'{name}.ja.json').read_text(encoding='utf-8-sig'))
    en = json.loads((root / f'{name}.en.json').read_text(encoding='utf-8-sig'))
    assert metadata(ja) == metadata(en), f'{name}: metadata or page/branch structure changed'
    serialized = json.dumps(en, ensure_ascii=False)
    assert serialized.isascii(), f'{name}: unexpected non-ASCII characters'
    assert not re.search(r'\b(holy|divine|blessed)\b', serialized, re.I), f'{name}: inconsistent sacred terminology'
    print(f'{name}: metadata, page counts, choice branches, ASCII text and sacred terminology verified')

special = json.loads((root / 'special-islands.en.json').read_text(encoding='utf-8'))
entries = {e['id']: e for e in special['objects']}
assert 'sixteenth night' in entries['island-7-01']['pages'][0]
assert 'elder twin' in entries['island-7-01']['pages'][0]
assert 'Srebmun dercas.' in entries['island-3-02']['pages'][4]
assert '**615*257' in entries['island-5419-04']['pages'][2]
assert 'exactly a million will not fit' in entries['island-90263-04']['pages'][0]
print('Numeric, reversal and strict upper-bound clues preserved')
