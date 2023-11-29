#include <cstdio>
#define _USE_MATH_DEFINES
#include <cmath>
#include <vector>
#include <iostream>
#include <sstream>
#include <conio.h>
#include <tuple>
#include <windows.h>
#include "Platno.h"

// na CLion dat Emulovat terminal
using namespace std;

void gotoxy(int x, int y)
{
    COORD pos = { (SHORT)x, (SHORT)y };
    HANDLE output = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleCursorPosition(output, pos);
}



// 🚀 Do tridy kamera pridejte posunuti a rotaci kamery, kdy budete ve skutecnosti rotovat vykreslovane objekty.

template<typename T1, typename T2>
struct Body
{
    T1 A;
    T2 B;

    Body(T1 A, T2 B) : A(A), B(B)
    {

    }
};





Bod2d Rotuj(Bod2d bod, double stupne)
{
    double uhelRadiany = (stupne * M_PI) / 180.0;

    double xt = (bod.x * cos(uhelRadiany)) - (bod.y * sin(uhelRadiany));
    double yt = (bod.x * sin(uhelRadiany)) + (bod.y * cos(uhelRadiany));

    return Bod2d{ xt, yt };
}

Bod2d Rotuj(Bod2d bod, double stupne, Bod2d S)
{
    bod.x -= S.x;
    bod.y -= S.y;

    bod = Rotuj(bod, stupne);

    bod.x += S.x;
    bod.y += S.y;

    return bod;
}

class GrafickyObjekt
{
public:
    virtual void Rotuj() = 0;
    virtual void Nakresli(Platno& platno) const = 0;
};

class RovnostrannyTrojuhelnik : public GrafickyObjekt
{
private:
    double a;
    Bod2d S;
    double uhelStupne;
    double zmenaUhlu;

public:
    RovnostrannyTrojuhelnik(Bod2d S, int a, double zmenaUhlu) : S(S), a(a), uhelStupne(0.0), zmenaUhlu(zmenaUhlu)
    {

    }

    void ZmenaUhlu(double stupne)
    {
        zmenaUhlu = stupne;
    }

    void Rotuj() override
    {
        uhelStupne += zmenaUhlu;
    }

    void Nakresli(Platno& platno) const override
    {
        double R = (a * sqrt(3.0)) / 3;
        double r = R / 2.0;

        Bod2d A(S.x - a / 2, S.y - r);
        Bod2d B(S.x + a / 2, S.y - r);
        Bod2d C(S.x, S.y + R);


        A = ::Rotuj(A, uhelStupne, S); // :: odlisi clenskou funkci od globalni funkce
        B = ::Rotuj(B, uhelStupne, S);
        C = ::Rotuj(C, uhelStupne, S);

        platno.NakresliDvojrozmernouUsecku(A, B);
        platno.NakresliDvojrozmernouUsecku(B, C);
        platno.NakresliDvojrozmernouUsecku(C, A);

        platno.NakresliBod(S);
    }
};

class Ctverec : public GrafickyObjekt
{
private:
    double a;
    Bod2d S;
    double uhelStupne;
    double zmenaUhlu;

public:
    Ctverec(Bod2d S, int a, double zmenaUhlu) : S(S), a(a), uhelStupne(0.0), zmenaUhlu(zmenaUhlu)
    {

    }

    void ZmenaUhlu(double stupne)
    {
        zmenaUhlu = stupne;
    }

    void Rotuj() override
    {
        uhelStupne += zmenaUhlu;
    }

    void Nakresli(Platno& platno) const override
    {
        double aPul = a / 2;

        Bod2d A(S.x - aPul, S.y - aPul);
        Bod2d B(S.x + aPul, S.y - aPul);
        Bod2d C(S.x + aPul, S.y + aPul);
        Bod2d D(S.x - aPul, S.y + aPul);


        A = ::Rotuj(A, uhelStupne, S); // :: odlisi clenskou funkci od globalni funkce
        B = ::Rotuj(B, uhelStupne, S);
        C = ::Rotuj(C, uhelStupne, S);
        D = ::Rotuj(D, uhelStupne, S);

        platno.NakresliDvojrozmernouUsecku(A, B);
        platno.NakresliDvojrozmernouUsecku(B, C);
        platno.NakresliDvojrozmernouUsecku(C, D);
        platno.NakresliDvojrozmernouUsecku(D, A);

        platno.NakresliBod(S);
    }
};

class Krychle : public GrafickyObjekt
{
private:
    double a;
    Bod3d S;
    double f;
public:

    Krychle(Bod3d S, double a, double f) : S(S), a(a), f(f)
    {

    }

    void Rotuj() override
    {

    }

    void Nakresli(Platno& platno) const override
    {
        Bod3d A(S.x - a / 2.0, S.y - a / 2.0, S.z - a / 2.0);
        Bod3d B(S.x + a / 2.0, S.y - a / 2.0, S.z - a / 2.0);
        Bod3d C(S.x + a / 2.0, S.y - a / 2.0, S.z + a / 2.0);
        Bod3d D(S.x - a / 2.0, S.y - a / 2.0, S.z + a / 2.0);

        Bod3d E(S.x - a / 2.0, S.y + a / 2.0, S.z - a / 2.0);
        Bod3d F(S.x + a / 2.0, S.y + a / 2.0, S.z - a / 2.0);
        Bod3d G(S.x + a / 2.0, S.y + a / 2.0, S.z + a / 2.0);
        Bod3d H(S.x - a / 2.0, S.y + a / 2.0, S.z + a / 2.0);

        platno.NakresliTrojrozmernouUsecku(A, B);
        platno.NakresliTrojrozmernouUsecku(B, C);
        platno.NakresliTrojrozmernouUsecku(C, D);
        platno.NakresliTrojrozmernouUsecku(D, A);

        platno.NakresliTrojrozmernouUsecku(E, F);
        platno.NakresliTrojrozmernouUsecku(F, G);
        platno.NakresliTrojrozmernouUsecku(G, H);
        platno.NakresliTrojrozmernouUsecku(H, E);

        platno.NakresliTrojrozmernouUsecku(A, E);
        platno.NakresliTrojrozmernouUsecku(B, F);
        platno.NakresliTrojrozmernouUsecku(C, G);
        platno.NakresliTrojrozmernouUsecku(D, H);
    }
};

int main()
{
    Kamera kamera(6.0);
    tuple<int, int> body = kamera.VratInty();
    printf("%d %d\n", get<0>(body), get<1>(body));

    int columnCount = 30;
    int rowCount = 20;

    Platno platno(columnCount, rowCount, '-', 'x');

    RovnostrannyTrojuhelnik trojuhelnik(Bod2d(20.0, 16.0), 16, 0.1);
    Ctverec ctverec(Bod2d(10.0, 5.0), 10, -0.05);
    Krychle krychle(Bod3d(20.0, 20, 40.0), 20.0, 10.0);

    vector<GrafickyObjekt*> objekty = { /*&trojuhelnik, &ctverec,*/ &krychle };

    bool konec = false;

    do
    {
        platno.Vymaz();

        for (GrafickyObjekt* objekt : objekty)
        {
            objekt->Nakresli(platno);
            objekt->Rotuj();
        }

        gotoxy(0, 0);

        platno.Zobraz();

        // 🐱‍👤 zde muzete detekovat stisk klaves

        if (_kbhit())
        {
            char znak = _getch();

            switch (znak)
            {
            case 'k':
                konec = true;
                break;
            case 'w':
                platno.kamera.z += 1.0;
                break;
            case 's':
                platno.kamera.z -= 1.0;
                break;
            case 'a':
                platno.kamera.x -= 1.0;
                break;
            case 'd':
                platno.kamera.x += 1.0;
                break;
            case 'r':
                platno.kamera.y += 1.0;
                break;
            case 'f':
                platno.kamera.y -= 1.0;
                break;
            }
        }

    } while (!konec);
}