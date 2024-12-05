# Histogram znaků v txt souboru

Napište program, který spočítá a zobrazí četnost výskytu znaků v txt souboru. 

1) Vypište znak a za ním četnost, například

a: 1\
b: 2\
c: 4\
...\
z: 1

2) Pomocí ASCI znaku zobrazte procentualni četnost, maximum znaků bude 10.

a: #\
b: ##\
c: ####\
...\
z: #


Příklad na práci s asociativní typem `map`:

```cpp
#include <iostream>
#include <string>
#include <map>

using namespace std;

int main()
{
    map<string, int> mp;
    mp["Matyas"] = 2;
    mp["Samuel"] = 1;
    mp["Maksym"] = 1;

    if(mp.contains("Matyas")) {
        ++mp["Matyas"];
    }

    for(pair<string, int> p : mp) {
        cout << p.first << ": " << p.second << endl;
    }

    return 0;
}
```

Příklad na práci se souborem:

```cpp
#include <fstream>
#include <iostream>

using namespace std;

int main()
{
    ifstream file("C:\\Users\\ekral\\CLionProjects\\untitled17\\text.txt");
    file >> std::noskipws;

    if(file.is_open())
    {
        char ch;
        while(file >> ch)
        {
            cout << ch;
        }
    }

    return 0;
}
```
