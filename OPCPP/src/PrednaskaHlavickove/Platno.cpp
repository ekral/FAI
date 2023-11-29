#include "Platno.h"

void Platno::NakresliBod(double x, double y)
{
    int rowIndex = (int)round(y);
    int columnIndex = (int)round(x);

    if ((rowIndex < 0) || (rowIndex > maxRowIndex) || (columnIndex < 0) || (columnIndex > maxColumnIndex))
    {
        return;
    }

    int pos = ((rowCount - rowIndex - 1) * columnCount) + columnIndex;

    data[pos] = popredi;
}
