# Komplexní fyzikální model digitálního oscilátoru

Tento model simuluje chování hmotného bodu ovlivněného pružinou, pohonem a vnějšími vlivy (šum, tření). Výsledkem je organický zvuk s analogovým charakterem.

## 1. Fyzikální základy

### Harmonický pohyb a „Jitter“ (Pružina)
Základem je vratná síla, která vrací systém do rovnováhy. Zanesením šumu do tuhosti $k$ vzniká **fázový jitter**:
$$k_{jitter} = k_{base} + \text{random}(-\Delta, \Delta)$$
$$F_{pružina} = -k_{jitter} \cdot x$$

### Tvarování (Pohon pro Triangle)
Konstantní impuls dodávaný podle polohy vytváří lineární náběhy:
$$F_{pohon} = \text{sign}(-x) \cdot F_{drive}$$

### Brownův pohyb (Tepelný šum)
$$F_{brown} = \text{random}(-\Delta, \Delta)$$


### Opotřebení a odpor (Tlumení)
Simulujeme ztrátu energie (tření). Opotřebení zaneseme jako drobnou nestabilitu v koeficientu tlumení $b$:
$$v_{nová} = v_{stará} \cdot (1 - b)$$

---

## 2. Pseudo-kód simulace

```python
# --- Parametry systému ---
m = 1.0                # Hmotnost
k_base = 0.05          # Základní tuhost (zaoblení rohů)
pohon_sila = 0.5       # Síla lineárního pohonu (strmost stran)
jitter_amount = 0.001  # Jitter tuhosti pružiny (fázová nestabilita)
brown_amount = 0.0005  # Intenzita Brownova pohybu (šum v síle)
wear_base = 0.001      # Základní tření/tlumení (0.0 = bez tření)

# --- Počáteční stav ---
x = 0.0                # Poloha (audio vzorek)
v = 1.0                # Rychlost
t_wear = 0             # Pomocná proměnná pro simulaci dýchání

while True:
    # 1. Jitter tuhosti pružiny (ovlivňuje frekvenci a fázi)
    k = k_base + random(-jitter_amount, jitter_amount)

    # 2. Simulace opotřebení (Kolísavé tření / Dýchání amplitudy)
    current_wear = wear_base + (sin(t_wear) * 0.0001)
    t_wear += 0.01

    # 3. Určení směru pohonu (pro tvarování vlny)
    f_pohon = pohon_sila if x <= 0 else -pohon_sila

    # 4. Brownův pohyb (Náhodná síla / Zrnitost zvuku)
    f_brown = random(-brown_amount, brown_amount)

    # 5. Výpočet celkové síly
    F_total = (-k * x) + f_pohon + f_brown

    # 6. Integrace (Newton + Tlumení/Opotřebení)
    a = F_total / m        # Zrychlení
    v = (v + a) * (1 - current_wear) # Aktualizace rychlosti + vliv tření
    x = x + v              # Aktualizace polohy (audio vzorek)

    # Výstup vzorku
    render(x)
```

## 3. Výsledný charakter zvuku
*   **Jitter:** Způsobuje, že oscilátor "nedrží" dokonale ladění (analogový drift).
*   **Zaoblený trojúhelník:** Pružina plynule brzdí lineární pohon (přirozený anti-aliasing).
*   **Brownův šum:** Dodává zvuku organickou zrnitost (grit).
*   **Opotřebení:** Simuluje dýchání a nedokonalost reálných komponent.
