#include <glm/glm.hpp> // Skvělá knihovna pro matematiku kompatibilní s GLSL


// Krok A: Definice struktury na CPU

struct PushConstants {
    glm::vec2 pA;
    glm::vec2 pB;
    glm::vec4 color;
    float thickness;
};

// Krok B: Vytvoření Pipeline Layout s Push Constants

VkPushConstantRange pushConstantRange{};
pushConstantRange.stageFlags = VK_SHADER_STAGE_FRAGMENT_BIT; // Budou dostupné ve Fragment Shaderu
pushConstantRange.offset = 0;
pushConstantRange.size = sizeof(PushConstants); // Velikost struktury v bajtech

VkPipelineLayoutCreateInfo pipelineLayoutInfo{};
pipelineLayoutInfo.sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO;
pipelineLayoutInfo.pushConstantRangeCount = 1;
pipelineLayoutInfo.pPushConstantRanges = &pushConstantRange; // Předáme rozsah
pipelineLayoutInfo.setLayoutCount = 0; // Descriptor sets nepoužíváme

// 3. Změna souřadnic za běhu (Vykreslovací smyčka)


// V každém snímku (Render Loop):
void drawFrame(VkCommandBuffer commandBuffer, VkPipeline graphicsPipeline, VkPipelineLayout pipelineLayout, float time) {
    
    // 1. Aktualizace dat na CPU (např. rotující úsečka pomocí sin/cos)
    PushConstants constants;
    constants.pA = glm::vec2(0.0f, 0.0f); // Střed obrazovky
    constants.pB = glm::vec2(cos(time) * 0.5f, sin(time) * 0.5f); // Konec rotuje dokola
    constants.color = glm::vec4(0.0f, 1.0f, 0.0f, 1.0f); // Zelená úsečka
    constants.thickness = 0.01f;

    // 2. Začátek render passu (vkCmdBeginRenderPass...)
    // ...

    // 3. Nabindování naší pipeline
    vkCmdBindPipeline(commandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, graphicsPipeline);

    // 4. POSLÁNÍ PUSH CONSTANTS DO KARTY
    vkCmdPushConstants(
        commandBuffer,
        pipelineLayout,
        VK_SHADER_STAGE_FRAGMENT_BIT, // Musí odpovídat nastavení při vytváření layoutu
        0,                            // Offset
        sizeof(PushConstants),        // Velikost posílaných dat
        &constants                    // Ukazatel na data na CPU
    );

    // 5. Vykreslení jednoho trojúhelníku (3 vrcholy), který vygeneruje fullscreen
    vkCmdDraw(commandBuffer, 3, 1, 0, 0);

    // 6. Konec render passu (vkCmdEndRenderPass...)
}

VkPipelineLayout pipelineLayout;
vkCreatePipelineLayout(device, &pipelineLayoutInfo, nullptr, &pipelineLayout);


// Krok C: Ignorování Vertex Bufferů v Graphics Pipeline


VkPipelineVertexInputStateCreateInfo vertexInputInfo{};
vertexInputInfo.sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;
vertexInputInfo.vertexBindingDescriptionCount = 0;   // Žádné vertex buffery
vertexInputInfo.vertexAttributeDescriptionCount = 0; // Žádné atributy

