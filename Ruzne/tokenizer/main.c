#include <stdio.h>
#include <string.h>

// 1. Inicializace implementace (pouze v JEDNOM .c souboru)
#define STB_C_LEXER_IMPLEMENTATION
#include "stb_c_lexer.h"

int main() {
    // Vstupní kód, který budeme analyzovat
    const char* source_code = "int x = 42; float y = 3.14f; int z;";

    // 2. Nastavení lexeru a bufferů
    stb_lexer lex;
    // Potřebujeme alokovat paměť pro texty tokenů (např. názvy proměnných)
    char string_store[1024];

    // Inicializace lexeru (poslední parametry jsou pro custom definice, zde netřeba)
    stb_c_lexer_init(&lex, source_code, source_code + strlen(source_code), string_store, 1024);

    int found_int_x = 0;
    int found_float_y = 0;

    // Pomocná proměnná pro uložení předchozího typu tokenu
    enum stb_c_lexer_token_type prev_type = CLEX_eof;
    char prev_string[256] = "";

    // 3. Smyčka pro procházení tokenů
    while (stb_c_lexer_get_token(&lex)) {

        // Kontrola: Pokud byl minulý token identifikátor s textem "int"
        // a aktuální token je identifikátor s textem "x"
        if (prev_type == CLEX_id && strcmp(prev_string, "int") == 0) {
            if (lex.token == CLEX_id && strcmp(lex.string, "x") == 0) {
                found_int_x = 1;
            }
        }

        // Kontrola: Pokud byl minulý token identifikátor "float"
        // a aktuální token je identifikátor "y"
        if (prev_type == CLEX_id && strcmp(prev_string, "float") == 0) {
            if (lex.token == CLEX_id && strcmp(lex.string, "y") == 0) {
                found_float_y = 1;
            }
        }

        // Uložení aktuálního tokenu jako "předchozího" pro další krok
        prev_type = lex.token;
        if (lex.token == CLEX_id) {
            strncpy(prev_string, lex.string, sizeof(prev_string) - 1);
        }
        else {
            prev_string[0] = '\0'; // Pokud to není ID, text nás pro typy nezajímá
        }
    }

    // 4. Výpis výsledků
    printf("Nalezeno 'int x': %s\n", found_int_x ? "ANO" : "NE");
    printf("Nalezeno 'float y': %s\n", found_float_y ? "ANO" : "NE");

    return 0;
}
