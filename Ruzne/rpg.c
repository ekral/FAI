#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main() {
    // Inicializace generátoru náhodných čísel podle aktuálního času
    srand(time(NULL));

    // Proměnné pro Hrdinu
    char jmeno[30];
    int hrdina_hp = 100;
    int hrdina_utok = 0;
    int volba_zbrane = 0;

    // Pole s názvy zbraní a jejich bonusovým útokem
    char zbrane[3][20] = {"Meč", "Sekera", "Kouzelná hůl"};
    int utok_zbrani[3] = {12, 15, 10};

    // Proměnné pro Monstrum
    int monstrum_hp = 120;
    int monstrum_utok_max = 14;

    // 1. UVÍTÁNÍ A VSTUPY
    printf("=== VÍTEJTE V RPG ARENĚ ===\n");
    printf("Zadej jméno svého hrdiny: ");
    scanf("%29s", jmeno);

    printf("\nVyber si zbraň:\n");
    for (int i = 0; i < 3; i++) {
        printf("%d) %s (Základní poškození: %d)\n", i + 1, zbrane[i], utok_zbrani[i]);
    }
    printf("Tvoje volba (1-3): ");
    scanf("%d", &volba_zbrane);

    // Ošetření vstupu zbraně (indexování pole od 0)
    if (volba_zbrane < 1 || volba_zbrane > 3) {
        printf("Neplatná volba, dostáváš pěsti!\n");
        hrdina_utok = 5;
    } else {
        hrdina_utok = utok_zbrani[volba_zbrane - 1];
        printf("Vyzbrojil jsi se: %s\n", zbrane[volba_zbrane - 1]);
    }

    printf("\nZ temnoty se vynořilo Monstrum (%d HP)! Souboj začíná...\n\n", monstrum_hp);

    // 2. CYKLUS SOUBOJE
    int kolo = 1;
    while (hrdina_hp > 0 && monstrum_hp > 0) {
        printf("--- KOLO %d ---\n", kolo);

        // --- TAH HRDINY ---
        // Náhodný útok: základní útok zbraně + náhodné číslo 0 až 5
        int skutecny_utok_hrdiny = hrdina_utok + (rand() % 6);
        
        // Ukázka podmínky: Šance 20% na kritický zásah
        if ((rand() % 5) == 0) {
            skutecny_utok_hrdiny *= 2;
            printf("⭐ KRITICKÝ ZÁSAH! ");
        }

        monstrum_hp -= skutecny_utok_hrdiny;
        printf("%s útočí za %d poškození. Monstru zbývá %d HP.\n", jmeno, skutecny_utok_hrdiny, monstrum_hp > 0 ? monstrum_hp : 0);

        // Kontrola, zda monstrum přežilo tah hrdiny
        if (monstrum_hp <= 0) {
            break; 
        }

        // --- TAH MONSTRA ---
        // Náhodný útok monstra od 5 do monstrum_utok_max
        int skutecny_utok_monstra = 5 + (rand() % (monstrum_utok_max - 5 + 1));
        hrdina_hp -= skutecny_utok_monstra;
        
        printf("👹 Monstrum vrací úder za %d poškození. %s má %d HP.\n\n", skutecny_utok_monstra, jmeno, hrdina_hp > 0 ? hrdina_hp : 0);

        kolo++;
    }

    // 3. VYHODNOCENÍ VÝSLEDKU
    printf("=================================\n");
    if (hrdina_hp > 0) {
        printf("🎉 VÍTĚZSTVÍ! %s porazil Monstrum v %d. kole!\n", jmeno, kolo);
        printf("Zbylo ti %d HP.\n", hrdina_hp);
    } else {
        printf("💀 PORÁŽKA! Monstrum tě rozdupalo v %d. kole.\n", kolo);
    }
    printf("=================================\n");

    return 0;
}
