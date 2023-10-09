# Ascii Rotace úsečky

S využitím kódu z úkolu [AsciiKresleníUsecky](../AsciiKresleniUsecky) vytvořte program, který:

- Zarotuje bod kolem počátku souřadnic (0,0) a vykreslete úsečku z počátku souřacnic do zarotovaného bodu. Použijte vzorec:

$$\begin{align*}
x' &= x \cdot \cos(\theta) - y \cdot \sin(\theta) \\
y' &= x \cdot \sin(\theta) + y \cdot \cos(\theta)
\end{align*}$$

$$\begin{align*}
\text{ Kde} (x', y') \text{ jsou souřadnice zarotovaného bodu, }
(x, y) \text{ jsou souřadnice původního bodu a }
\theta \text{ je úhel rotace v radiánech.}
\end{align*}$$

- Zarotuje úsečku kolem středu úsečky.

Pokud chceme zarotovat úsečku, tak musíme:

 1) Zjistit střed úsečky.
 2) Posunout úsečku tak aby její střed byl v počátku souřadnic.
 3) Zarotovat krajní body úsečky kolem počátku souřadnic.
 4) Posunout úsečku zpět aby její střed byl opět na původním místě.
