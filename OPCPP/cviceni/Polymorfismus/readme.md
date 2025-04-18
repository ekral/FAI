# Polymorfismus - překrývání virtuálních funkcí

1. Změnte funkci `Draw` na překrytou (override) pure virtual function.
    - Přidejte pure virtual function `Draw` do třídy `Shape`.
    - Přidejte klíčové slovo `override` k funcím `Draw` do tříd `Rectangle`, `Triangle` a `Line`.

3. Vytvořte menu, pro každý objekt zadejte id a jeho parametry (například délku strany).

   Objekty vkládejte do vektoru `shapes` jako typ `Shape*`.
   
    1. Create triangle.
    2. Create rectangle.
    3. Create line.
    4. Change angle - zadá se id objektu a nový úhel rotace objektu se zadaným id.
    
    

5. Vykreslete všechny objekty na plátno `canvas`.

    

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

class Shape {
    protected:
    Point2d S;
    double angle;

    public:
    Shape (Point2d S, const double angle) : S(S), angle(angle){};




};

class Line : public Shape {
private:
    double length;
public:
    Line(const Point2d S, const double lenght, const double angle) : Shape(S, angle), length(lenght)
    {
    };

    void Draw(Canvas& canvas) const
    {
        Point2d A(S.x - length / 2, S.y );
        Point2d B(S.x + length / 2, S.y);

        A = rotate(A, S, angle);
        B = rotate(B, S, angle);

        canvas.DrawLine(A, B);
    }
};

class Triangle : public Shape
{
private:
    double a;
public:
    Triangle(const Point2d S, const double a, const double angle) : Shape (S , angle), a(a)
    {
    }

    void Draw(Canvas& canvas) const
    {
        Point2d A(S.x - a / 2, S.y - sqrt(3.0) * a / 6);
        Point2d B(S.x + a / 2, S.y - sqrt(3.0) * a / 6);
        Point2d C(S.x, S.y + sqrt(3.0) * a / 3);

        A = rotate (A, S, angle);
        B = rotate (B, S, angle);
        C = rotate (C, S, angle);

        canvas.DrawLine(A, B);
        canvas.DrawLine(B, C);
        canvas.DrawLine(C, A);
    }
};

class Rectangle : public Shape {
private:
    double width;
    double height;
public:
    Rectangle(const Point2d S, const double width, const double height, const double angle) : Shape(S, angle), width(width), height(height) {

    }

    void Draw(Canvas& canvas) const
    {
        Point2d A(S.x - width / 2, S.y - height / 2);
        Point2d B(S.x - width / 2, S.y + height / 2);
        Point2d C(S.x + width / 2, S.y + height / 2);
        Point2d D(S.x + width / 2, S.y - height / 2);


        A = rotate (A, S, angle);
        B = rotate (B, S, angle);
        C = rotate (C, S, angle);
        D = rotate (D, S, angle);

        canvas.DrawLine(A, B);
        canvas.DrawLine(B, C);
        canvas.DrawLine(C, D);
        canvas.DrawLine(D, A);
    }
};

int main()
{
    Canvas canvas(60, 15);

    canvas.Erase();

    Line line(Point2d(10.0, 7.5), 10.0, 5.0);
    line.Draw(canvas);

    Triangle triangle(Point2d(20, 7.5), 10.0, 0.0);
    triangle.Draw(canvas);

    Rectangle rectangle(Point2d(40, 7.5), 10.0, 8.0, 45.0);
    rectangle.Draw(canvas);

    canvas.Show();

    return 0;
}
```
