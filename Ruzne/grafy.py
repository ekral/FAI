import numpy as np
import matplotlib.pyplot as plt
from scipy.special import sici

def blamp_kernel(t):
    """
    Vypočítá ideální BLAMP kernel (druhý integrál sinc).
    Vzorec pro normalizovaný sinc(pi*t):
    Integral(Integral(sinc(pi*t))) = (pi*t*Si(pi*t) + cos(pi*t) - 1) / pi^2
    """
    # Vyhneme se dělení nulou v bodě 0 (limitní hodnota je 0)
    t = np.asanyarray(t)
    tau = np.pi * t
    si, ci = sici(tau)
    
    # Samotný výpočet druhého integrálu
    val = (tau * si + np.cos(tau) - 1) / (np.pi**2)
    return val

# 1. Definice časové osy (v jednotkách vzorků)
t = np.linspace(-400, 400, 1000)
y = blamp_kernel(t)

# 2. Vykreslení
plt.figure(figsize=(10, 6))
plt.plot(t, y, label='BLAMP Kernel (2. integrál sinc)', color='blue', lw=2)

# Zvýraznění bodu zlomu (0)
plt.axvline(0, color='red', linestyle='--', alpha=0.5)
plt.axhline(0, color='black', lw=1)

# Přidání popisků
plt.title('Průběh BLAMP kernelu v okolí bodu 0')
plt.xlabel('Čas (vzdálenost od zlomu ve vzorcích)')
plt.ylabel('Amplituda korekce')
plt.grid(True, linestyle=':', alpha=0.6)
plt.legend()

plt.show()

# --- PRAKTICKÁ POZNÁMKA ---
# Pro reálnou implementaci (např. v C++) se tento kernel ořezává 
# (windowing) a aproximuje polynomy, aby se nemusely volat 
# drahé funkce sici() a cos() pro každý vzorek.
