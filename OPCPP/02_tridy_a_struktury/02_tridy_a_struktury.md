# Třídy a struktury
--

* Třídu definujeme pomocí klíčového slova `class`. Pokud chceme přistupovat k členským proměnným mimo třídu, tak je musíme deklarovat jako `public`. Následující příklad definuje třídu pro dvourozměrný bod s public členskými proměnnými `x` a `y`. Na rozdíl od ostatních jazyků, například Java, nebo C# musíme v jazyce C++ uvést za definicí třídy středník `;`.

```c++
class Bod
{
public:
	int x;
	int y;
};
```

* Proměnnou typu třída definujeme stejným způsobem jako zabudované typy. V následujícím příkladu definujeme proměnnou `b1` typu `Bod`.
```cs 
Bod b1;
```
* K členským proměnným potom přistupujeme pomocí operátoru přímého přístupu `.`. 
```cs 
Bod b1;
b1.x = 2;
b1.y = 3;

printf("Bod b1 ma souradnice x: %d y: %d", b1.x, b1.y);
```
* Stejně jako u zabudovaných typů se při přiřazení hodnoty kopíruje hodnota v paměti. Relační, aritmetické a další operátory bychom ale museli sami definovat.

```cs 
Bod b1;
b1.x = 2;
b1.y = 3;

Bod b2 = b1;
```
