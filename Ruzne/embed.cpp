#include <stdint.h>
#include <stddef.h>

// Důležité: Zarovnání na 4 bajty pro správné čtení Vulkanem
alignas(uint32_t) const uint8_t vertex_shader_spirv[] = {
    #embed "shaders/triangle.vert.spv"
};

// Velikost v bajtech, kterou předáte do VkShaderModuleCreateInfo
const size_t shader_size = sizeof(vertex_shader_spirv);
