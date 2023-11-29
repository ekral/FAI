#pragma once

#include <tuple>
#include "Bod2d.h"
#include "Bod3d.h"

using namespace std;

class Kamera
{
public:
    double f;
    double z;
    double x;
    double y;

    Kamera(double f) :f(f), z(0.0), x(0.0), y(0.0)
    {

    }

    Bod2d Projekce(Bod3d bod)
    {
        bod.z -= z;
        bod.x -= x;
        bod.y -= y;

        Bod2d projekce = Bod2d(f * bod.x / bod.z, f * bod.y / bod.z);

        return projekce;
    }

    tuple<Bod2d, Bod2d> ProjekceUsecky(Bod3d A, Bod3d B)
    {
        Bod2d Ap = Projekce(A);
        Bod2d Bp = Projekce(B);

        return tuple<Bod2d, Bod2d>(Ap, Bp);
    }

    tuple<int, int> VratInty()
    {
        return tuple<int, int>(1, 2);
    }
};
