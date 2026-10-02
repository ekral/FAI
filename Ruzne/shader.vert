#version 450

layout(location = 0) out vec2 outUV;

void main() {
    // Vygeneruje UV souřadnice (0.0 až 2.0) na základě indexu vrcholu (0, 1, 2)
    outUV = vec2((gl_VertexIndex << 1) & 2, gl_VertexIndex & 2);
    
    // Převede UV na NDC souřadnice (-1.0 až 1.0). Vulkan má Y dolů.
    gl_Position = vec4(outUV * 2.0 - 1.0, 0.0, 1.0);
}
