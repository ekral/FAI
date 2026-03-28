# Fyzikální model digitálního oscilátoru

Tento model vychází z **lineárního harmonického oscilátoru**, který místo čisté matematické funkce (sinus) používá simulaci sil působících na hmotný bod (míček).

## 1. Základní model (Harmonický kmit)
Základem je **Hookeův zákon**, kde síla $F$ působí vždy proti směru výchylky $x$. Tím vzniká přirozený sinusový pohyb.

$$F_{pružina} = -k \cdot x$$

*   **$k$ (tuhost):** Určuje frekvenci kmitání.
*   **$m$ (hmotnost):** Určuje setrvačnost (v simulaci často $m = 1$).
*   **Vztah pro frekvenci:** $f = \frac{1}{2\pi} \sqrt{\frac{k}{m}}$

## 2. Analogový charakter (Nestabilita parametrů)
Pro dosažení "živého" analogového zvuku nepracujeme s fixními hodnotami, ale zanášíme šum přímo do fyzikálních parametrů. Tím se šum stává integrální součástí pohybu (je přirozeně filtrován modelem).

*   **Jitter frekvence:** $k_{t} = k + \text{random}(\Delta)$ (způsobuje jemné fázové chvění).
*   **Tepelný šum:** Přidání malé náhodné síly $F_{noise}$ do výpočtu zrychlení (způsobuje mikroskopickou "zubatost" trajektorie).

## 3. Tvarování vln (Zaoblený trojúhelník)
Abychom se odklonili od sinu směrem k trojúhelníku s "analogově" zaoblenými rohy, přidáváme ke slabé pružině **konstantní pohon** ($F_{drive}$), který mění směr podle polohy.

*   **Lineární část:** Konstantní síla pohonu dominuje ve středu a vytváří lineární nárůst/pokles rychlosti.
*   **Zaoblení:** V krajních polohách naroste síla pružiny $-kx$ natolik, že pohyb plynule zabrzdí a otočí. Tím se eliminují ostré zlomy a vysokofrekvenční aliasing.

---

## Pseudo-kód simulace

Tento kód kombinuje fyzikální pohyb, analogovou nestabilitu a tvarování vlny do zaobleného trojúhelníku.

```python
# --- Parametry systému ---
m = 1.0              # Hmotnost
k_base = 0.05        # Základní tuhost (určuje "zaoblení" rohů)
pohon_sila = 0.5     # Síla lineárního pohonu (určuje strmost stran)
jitter_amount = 0.001 # Míra analogové nestability

# --- Počáteční stav ---
x = 0.0              # Poloha (výchylka)
v = 1.0              # Počáteční rychlost

while True:
    # 1. Analogová nestabilita (změna tuhosti v každém kroku)
    k = k_base + random(-jitter_amount, jitter_amount)

    # 2. Určení směru konstantního pohonu (podle aktuální polohy)
    if x > 0:
        f_pohon = -pohon_sila  # Tlačíme dolů
    else:
        f_pohon = pohon_sila   # Tlačíme nahoru

    # 3. Výpočet celkové síly (Pružina + Pohon)
    # F = -k*x (vratná síla pružiny) + f_pohon (pohon směrem ke středu)
    F_total = (-k * x) + f_pohon

    # 4. Integrace pohybu (Eulerova metoda)
    a = F_total / m      # Zrychlení (Newtonův zákon)
    v = v + a            # Aktualizace rychlosti
    x = x + v            # Aktualizace polohy (toto je náš audio vzorek)

    # Výstup vzorku
    render(x)
```