#include <iostream>
#include <string>
#include <algorithm>
#define _USE_MATH_DEFINES
#include <math.h>
#include <Windows.h>
#include <vector>
#include <conio.h>

using namespace std;

void gotoxy(int x, int y)
{
    const COORD pos = { static_cast<short>(x), static_cast<short>(y) };
    const HANDLE output = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleCursorPosition(output, pos);
}

struct Bod2d
{
    double x;
    double y;

    Bod2d(const double x, const double y) : x(x), y(y)
    {

    }

    Bod2d& operator += (const Bod2d& other)
    {
        x += other.x;
        y += other.y;

        return *this;
    }

    // 🚀 vytvorte operator -=
    Bod2d& operator -= (const Bod2d& other)
    {
        x -= other.x;
        y -= other.y;

        return *this;
    }

    friend Bod2d operator - (const Bod2d& A, const Bod2d& B)
    {
        return Bod2d(A.x - B.x, A.y - B.y);
    }

    friend Bod2d operator + (const Bod2d& A, const Bod2d& B)
    {
        return Bod2d(A.x + B.x, A.y + B.y);
    }
};

class Platno
{
private:
    string retezec;
public:
    const int sirka;
    const int vyska;

    Platno(const int sirka, const int vyska) : retezec((sirka + 1)* vyska, '-'), sirka(sirka), vyska(vyska)
    {
        Vymaz();
    }

    void Vymaz()
    {
        fill(retezec.begin(), retezec.end(), '-');

        for (int i = sirka; i < retezec.length(); i += sirka + 1)
        {
            retezec[i] = '\n';
        }

    }

    void Zobraz() const
    {
        cout << retezec << endl;
    }

    void NakresliBod(const double x, const double y)
    {
        const int ix = static_cast<int>(round(x));
        const int iy = static_cast<int>(round(y));

        if (ix < 0.0 || ix >= sirka || iy < 0.0 || iy >= vyska)
        {
            return;
        }

        const int pos = (vyska - iy - 1) * (sirka + 1) + ix;

        retezec[pos] = 'x';
    }

    void NakresliUsecku(const Bod2d& A, const Bod2d& B)
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
            NakresliBod(x, y);

            x += stepx;
            y += stepy;
        }
    }
};

Bod2d rotace(const Bod2d A, const double uhelStupne)
{
    const double uhelRadiany = (uhelStupne * M_PI) / 180;

    const double xt = A.x * cos(uhelRadiany) - A.y * sin(uhelRadiany);
    const double yt = A.x * sin(uhelRadiany) + A.y * cos(uhelRadiany);

    const Bod2d At(xt, yt);

    return At;
}

// 🚀 Vytvorte rodicovskou tridu GrafickyObjekt,
// která bude obsahovat to co mají
// Usecka a Trojuhelnik spolecneho
// Usecka a Trojuhelnik budou potomci teto tridy.
class GrafickyObjekt 
{
public:
    Bod2d S;
    double uhelStupne;
    GrafickyObjekt (const Bod2d S) : S(S), uhelStupne(0.0) 
    {
    }
    virtual ~GrafickyObjekt() = default;
    virtual void Nakresli(Platno* platno) = 0;
    virtual void ZmenRotaci(double novyUhel) = 0;
};
    
class Usecka : public GrafickyObjekt
{
public:    
    double delka;
    
    Usecka(Bod2d S, double delka) : GrafickyObjekt(S), delka(delka)
    {
    }

    ~Usecka()
    {
        cout << "Destruuji usecku" << endl;
    }

    void ZmenRotaci(double novyUhel) override
    {
        uhelStupne = novyUhel;
    }

    void Nakresli(Platno* platno) override
    {
        Bod2d A(S.x - delka / 2, S.y);
        Bod2d B(S.x + delka / 2, S.y);

        Bod2d At = A - S;
        Bod2d Bt = B - S;

        At = rotace(At, uhelStupne);
        Bt = rotace(Bt, uhelStupne);

        At += S;
        Bt += S;

        platno->NakresliUsecku(At, Bt);
    }
};

class Trojuhelnik : public GrafickyObjekt
{
private:
    double a;

public:
    Trojuhelnik(const Bod2d S, const double a) : GrafickyObjekt(S), a(a)
    {
    }

    ~Trojuhelnik()
    {
        cout << "Destruuji trojuhelnik" << endl;
    }

    void ZmenRotaci(double novyUhel) override
    {
        uhelStupne = novyUhel;
    }

    void Nakresli(Platno* platno) override
    {
        Bod2d A(S.x - a / 2, S.y - sqrt(3.0) * a / 6);
        Bod2d B(S.x + a / 2, S.y - sqrt(3.0) * a / 6);
        Bod2d C(S.x, S.y + sqrt(3.0) * a / 3);

        Bod2d At = rotace(A - S, uhelStupne) + S;
        Bod2d Bt = rotace(B - S, uhelStupne) + S;
        Bod2d Ct = rotace(C - S, uhelStupne) + S;

        platno->NakresliUsecku(At, Bt);
        platno->NakresliUsecku(Bt, Ct);
        platno->NakresliUsecku(Ct, At);
    }
};


int main()
{
    Platno platno(40, 20);

    vector<GrafickyObjekt*> objekty;

    double stupne = 0.0;
    bool konec = false;
    
    while (!konec)
    {
        if (_kbhit())
        {
            int znak = _getch();

            switch (znak)
            {
            case 'k':
                konec = true;
                break;
            case 't':
            {
                system("cls");
                cout << "Novy trojuhelnik" << endl;
                cout << "zadej x: ";
                int x;
                cin >> x;

                cout << "zadej y: ";
                int y;
                cin >> y;

                cout << "zadej a: ";
                int a;
                cin >> a;

                Trojuhelnik* trojuhelnik = new Trojuhelnik(Bod2d(x, y), a);
                objekty.push_back(trojuhelnik);
            }
                break;
            default:
                break;
            }
        }

        platno.Vymaz();

        for (GrafickyObjekt* objekt : objekty)
        {
            objekt->ZmenRotaci(stupne);
            objekt->Nakresli(&platno);
        }

        gotoxy(0, 0);

        platno.Zobraz();

        stupne += 0.02;
    }

    for (GrafickyObjekt* objekt : objekty)
    {
        delete objekt;
    }

    objekty.clear();

    cout << "Konec programu" << endl;

    return 0;
}
