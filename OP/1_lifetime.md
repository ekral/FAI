
# Životní cyklus objektu a správa paměti

Životní cyklus objektu (lifetime) představuje čas mezi vytvoření a zničením objektu. 

- Lokální proměnnná je alokovaná na zásobníku a existuje od své definice do konce bloku definovaného složenými závorkami.
- Statická promnná (field) existuje po celou dobu běhu programu.
- Objekt na haldě alokovaný pomocí operátoru `new` existuje od své alokace po uvolnění paměti. V jazyce C# uvolňuje tuto paměť automaticky Garbagge Collecor potom co zjistí, že na objekt na haldě už není žádná reference. Naproti tomu, například v jazyce C nebo C++ musíme paměť uvolňovat manuálně pomocí příkazu `free` respektive `delete`

TODO: příklady na lokální proměnnou, alokaci na haldě a na zásobníku, odkaz na prezentaci z A1ZAP.
