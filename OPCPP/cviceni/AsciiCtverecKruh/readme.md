# Příklad na dědičnost: Tvary rovnostranný trojuhelnik, čtverec a úsečka

Cílem příkladu je procvičení dědičnosti kódu. 

1. Třída Shape

Definujte rodičovskou třídu `Shape`, která bude mít `protected` členské prvky `S` (souřadnice středu) a `angle` (úhel rotace kolem středu).

2. Potomci třídy Shape

    - třída `Triangle` - rovnostranný trojúhelník zadaný středem a délkou strany a úhlem rotace kolem středu.
    - třída `Rectangle` - obdelník zadaný délkami stran, středem a úhlem rotace kolem středu.
    - třída `Line` - úsečka zadaná delkou, středem a úhlem rotace kolem středu.

3. Vyreslení obrazců na Ascii plátno.

Vytvořte menu:
1. Create triangle.
2. Create rectangle.
3. Create line.
4. Exit

Po vytvoření obrazce jej vykreslete na Ascii plátno.

Výchozí kód:

```cpp
#include <iostream>
#include <string>
#include <algorithm>
#include <cmath>
#include <ranges>

using namespace std;

struct Point2d
{
    double x;
    double y;

    Point2d(const double x, const double y) : x(x), y(y)
    {

    }
};

Point2d rotate(const Point2d A, const double angleDegrees)
{
    const double angleRadians = (angleDegrees * M_PI) / 180;

    const double xt = A.x * cos(angleRadians) - A.y * sin(angleRadians);
    const double yt = A.x * sin(angleRadians) + A.y * cos(angleRadians);

    const Point2d At(xt, yt);

    return At;
}

Point2d rotate(Point2d A, const Point2d S, const double angleDegrees)
{
    A.x -= S.x;
    A.y -= S.y;

    Point2d At = rotate(A, angleDegrees);

    At.x += S.x;
    At.y += S.y;

    return At;
}

class Canvas
{
private:
    string data;
public:
    const int width;
    const int height;

    Canvas(const int width, const int height) : data((width + 1) * height, '-'), width(width), height(height)
    {
        Erase();
    }

    void Erase()
    {
        fill(data.begin(), data.end(), '-');

        for (int i = width; i < data.length(); i += width + 1)
        {
            data[i] = '\n';
        }

    }

    void Show() const
    {
        cout << data << endl;
    }

    void DrawPoint(const double x, const double y)
    {
        const int ix = static_cast<int>(round(x));
        const int iy = static_cast<int>(round(y));

        if(ix < 0.0 || ix >= width || iy < 0.0 || iy >= height)
        {
            return;
        }

        const int pos = (height - iy - 1) * (width + 1) + ix;

        data[pos] = 'x';
    }

    void DrawLine(const Point2d& A, const Point2d& B)
    {
        double dx = B.x - A.x;
        double dy = B.y - A.y;

        double dmax = max(abs(dx), abs(dy));

        double stepx = dx / dmax;
        double stepy = dy / dmax;

        double x = A.x;
        double y = A.y;

        for (double t = 0.0; t <= dmax; t += 1.0)
        {
            DrawPoint(x, y);

            x += stepx;
            y += stepy;
        }
    }
};

class Triangle
{
private:
    Point2d S;
    double a;
public:
    Triangle(const Point2d S, const double a) : S(S), a(a)
    {
    }

    void Draw(Canvas* canvas) const
    {
        Point2d A(S.x - a / 2, S.y - sqrt(3.0) * a / 6);
        Point2d B(S.x + a / 2, S.y - sqrt(3.0) * a / 6);
        Point2d C(S.x, S.y + sqrt(3.0) * a / 3);

        canvas->DrawLine(A, B);
        canvas->DrawLine(B, C);
        canvas->DrawLine(C, A);
    }
};

int main()
{
    Canvas canvas(60, 15);

    canvas.Erase();


    const Point2d center(9.5, 4.5);
    canvas.DrawPoint(center.x, center.y);

    Triangle triangle(center, 10.0);
    triangle.Draw(&canvas);

    canvas.Show();

    return 0;
}
```





