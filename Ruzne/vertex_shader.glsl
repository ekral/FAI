glsl#version 450

layout(location = 0) out vec2 outUV;

void main() {
    // Generování UV souřadnic přímo z indexu vrcholu (0, 1, 2)
    outUV = vec2((gl_VertexIndex << 1) & 2, gl_VertexIndex & 2);
    
    // Přepočet UV na NDC souřadnice (-1.0 až 1.0)
    gl_Position = vec4(outUV * 2.0 - 1.0, 0.0, 1.0);
    
    // Poznámka: Vulkan má Y osu dolů, takže možná budete muset Y převrátit 
    // v závislosti na tom, jak máte nastavený Viewport:
    // gl_Position.y = -gl_Position.y;
}