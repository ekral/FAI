# Ascii Rotace úsečky

S využitím kódu z úkolu [AsciiKresleníUsecky](../AsciiKresleniUsecky) vytvořte program, který:

1) Zarotuje bod kolem počátku souřadnic (0,0) a vykreslete úsečku z počátku souřacnic do zarotovaného bodu. Použijte vzorec:

$$\begin{align*}
x' &= x \cdot \cos(\theta) - y \cdot \sin(\theta) \\
y' &= x \cdot \sin(\theta) + y \cdot \cos(\theta)
\end{align*}$$

kde:

$$\begin{align*}
(x', y') \text{ jsou souřadnice zarotovaného bodu.}\\
(x, y) \text{ jsou souřadnice původního bodu.}\\
\theta \text{ je úhel rotace v radiánech.}
\end{align*}$$

2) Zarotuje úsečku kolem středu úsečky.