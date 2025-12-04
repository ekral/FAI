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

struct Bod3d
{
    double x;
    double y;
    double z;

    Bod3d(const double x, const double y, const double z) : x(x), y(y), z(z)
    {

    }

    Bod3d& operator += (const Bod3d& other)
    {
        x += other.x;
        y += other.y;
        z += other.z;

        return *this;
    }

    Bod3d& operator -= (const Bod3d& other)
    {
        x -= other.x;
        y -= other.y;
        z -= other.z;

        return *this;
    }

    friend Bod3d operator - (const Bod3d& A, const Bod3d& B)
    {
        return Bod3d(A.x - B.x, A.y - B.y, A.z - B.z);
    }

    friend Bod3d operator + (const Bod3d& A, const Bod3d& B)
    {
        return Bod3d(A.x + B.x, A.y + B.y, A.z + B.z);
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

Bod3d rotace(const Bod3d A, const double uhelStupne)
{
    const double uhelRadiany = (uhelStupne * M_PI) / 180;

    double c = cos(uhelRadiany);
    double s = sin(uhelRadiany);

    const double xt = A.x * c + A.z * s;
    const double yt = A.y;
    const double zt = ((-A.x) * s) + A.z * c;

    const Bod3d At(xt, yt, zt);

    return At;
}

class GrafickyObjekt
{
public:
    Bod2d S;
    double uhelStupne;
    GrafickyObjekt(const Bod2d S) : S(S), uhelStupne(0.0)
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

class Ctverec : public GrafickyObjekt
{
private:
    double a; //dlzka strany

public:
    Ctverec(const Bod2d S, const double a) : GrafickyObjekt(S), a(a)
    {
    }

    ~Ctverec()
    {
        cout << "Destruuji ctverec" << endl;
    }

    void ZmenRotaci(double novyUhel) override
    {
        uhelStupne = novyUhel;
    }

    void Nakresli(Platno* platno) override
    {
        Bod2d A(S.x - a / 2, S.y - a / 2);
        Bod2d B(S.x + a / 2, S.y - a / 2);
        Bod2d C(S.x + a / 2, S.y + a / 2);
        Bod2d D(S.x - a / 2, S.y + a / 2);

        Bod2d At = rotace(A - S, uhelStupne) + S;
        Bod2d Bt = rotace(B - S, uhelStupne) + S;
        Bod2d Ct = rotace(C - S, uhelStupne) + S;
        Bod2d Dt = rotace(D - S, uhelStupne) + S;

        platno->NakresliUsecku(At, Bt);
        platno->NakresliUsecku(Bt, Ct);
        platno->NakresliUsecku(Ct, Dt);
        platno->NakresliUsecku(Dt, At);
    }
};

class Jehlan : public GrafickyObjekt
{
private:
    double a;
    Bod3d stred;

public:
    // TODO odstranit stred z Grafickeho Objektu
    Jehlan(const Bod3d stred, const double a) : GrafickyObjekt(Bod2d(0,0)), stred(stred), a(a)
    {
    }

    ~Jehlan()
    {
        cout << "Destruuji jehlan" << endl;
    }

    void ZmenRotaci(double novyUhel) override
    {
        uhelStupne = novyUhel;
    }

    void Nakresli(Platno* platno) override
    {
        Bod3d S = stred;

        Bod3d A(S.x - a / 2, S.y - a / 2, S.z + a / 2);
        Bod3d B(S.x + a / 2, S.y - a / 2, S.z + a / 2);
        Bod3d C(S.x + a / 2, S.y + a / 2, S.z + a / 2);
        Bod3d D(S.x - a / 2, S.y + a / 2, S.z + a / 2);
        Bod3d V(S.x, S.y, S.z - a / 2);
           
        Bod3d At = rotace(A - S, uhelStupne) + S;
        Bod3d Bt = rotace(B - S, uhelStupne) + S;
        Bod3d Ct = rotace(C - S, uhelStupne) + S;
        Bod3d Dt = rotace(D - S, uhelStupne) + S;
        Bod3d Vt = rotace(V - S, uhelStupne) + S;

        double f = 30.0;

        Bod2d Ap = Bod2d(f * At.x / (At.z + f), f * At.y / (At.z + f));
        Bod2d Bp = Bod2d(f * Bt.x / (Bt.z + f), f * Bt.y / (Bt.z + f));
        Bod2d Cp = Bod2d(f * Ct.x / (Ct.z + f), f * Ct.y / (Ct.z + f));
        Bod2d Dp = Bod2d(f * Dt.x / (Dt.z + f), f * Dt.y / (Dt.z + f));
        Bod2d Vp = Bod2d(f * Vt.x / (Vt.z + f), f * Vt.y / (Vt.z + f));

        platno->NakresliUsecku(Ap, Bp);
        platno->NakresliUsecku(Bp, Cp);
        platno->NakresliUsecku(Cp, Dp);
        platno->NakresliUsecku(Dp, Ap);
        platno->NakresliUsecku(Ap, Vp);
        platno->NakresliUsecku(Bp, Vp);
        platno->NakresliUsecku(Cp, Vp);
        platno->NakresliUsecku(Dp, Vp);
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
            case 'j':
            {
                system("cls");
                cout << "Novy jehlan" << endl;
                cout << "zadej x: ";
                int x;
                cin >> x;

                cout << "zadej y: ";
                int y;
                cin >> y;

                cout << "zadej z: ";
                int z;
                cin >> z;

                cout << "zadej a: ";
                int a;
                cin >> a;

                Jehlan* trojuhelnik = new Jehlan(Bod3d(x, y, z), a);
                objekty.push_back(trojuhelnik);
            }
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

            case 'u':
            {
                system("cls");
                cout << "Nova usecka" << endl;
                cout << "zadej x: ";
                int x;
                cin >> x;

                cout << "zadej y: ";
                int y;
                cin >> y;

                cout << "zadej a: ";
                int a;
                cin >> a;

                Usecka* usecka = new Usecka(Bod2d(x, y), a);
                objekty.push_back(usecka);
            }

            break;
            case 'c':
            {
                system("cls");
                cout << "Novy ctvrec" << endl;
                cout << "zadej x: ";
                int x;
                cin >> x;

                cout << "zadej y: ";
                int y;
                cin >> y;

                cout << "zadej a: ";
                int a;
                cin >> a;

                Ctverec* ctverec = new Ctverec(Bod2d(x, y), a);
                objekty.push_back(ctverec);
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
