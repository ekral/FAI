#version 450

layout(location = 0) in vec2 outUV;
layout(location = 0) out vec4 outColor;

// Struktura Push Constants (Musí se přesně shodovat s C++ strukturou!)
layout(push_constant) uniform LineData {
    vec2 pA;       // Počáteční bod úsečky
    vec2 pB;       // Koncový bod úsečky
    vec4 color;    // Barva úsečky
    float thickness; // Tloušťka úsečky (např. 0.005)
} line;

// Pomocná funkce pro výpočet vzdálenosti bodu od úsečky
float distanceToLine(vec2 p, vec2 a, vec2 b) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
    return length(pa - ba * h);
}

void main() {
    // Převedeme UV souřadnice (0 až 1) zpět na prostor obrazovky (-1 až 1),
    // abychom mohli porovnávat pozice s push constants.
    vec2 currentPos = outUV * 2.0 - 1.0;
    
    float d = distanceToLine(currentPos, line.pA, line.pB);
    
    // Pokud je pixel blízko úsečky, vykreslíme ji, jinak průhledno/pozadí
    if (d < line.thickness) {
        outColor = line.color;
    } else {
        outColor = vec4(0.1, 0.1, 0.1, 1.0); // Tmavé pozadí
    }
}
