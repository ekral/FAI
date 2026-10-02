// Příklad v GLSL shaderu
struct Obdelnik {
    vec2 pozice;
    vec2 velikost;
    vec4 barva;
};

layout(std430, binding = 0) buffer MojeUIModuly {
    Obdelnik prvky[]; // Dynamické pole - délku určuje C++ kód při alokaci bufferu
};

/*
3. Umí číst i zapisovat (Read/Write)Zatímco do běžných vertex bufferů nebo textur shader během vykreslování typicky jen nahlíží, do SSBO může Compute shader nebo Fragment shader přímo zapisovat data za běhu GPU. Pro běžné UI sice stačí z SSBO jen číst, ale tato vlastnost z něj dělá velmi mocný nástroj.4. Layout std430U SSBO se používá paměťový standard std430. Ten zajišťuje, že data jsou v paměti naskládána velmi úsporně a efektivně, což usnadňuje mapování struktur mezi C++ a shaderem (nemusíte řešit složité zarovnávání dat na 16 bajtů, jako tomu je u std140 u Uniform Bufferů).Jak to zapadá do vašeho UI?Místo toho, abyste pro Vulkan pracně vytvářeli klasický VkBuffer s příznakem VK_BUFFER_USAGE_VERTEX_BUFFER_BIT (Vertex Buffer), vytvoříte buffer s příznakem VK_BUFFER_USAGE_STORAGE_BUFFER_BIT.V C++ naplníte pole struktur (např. pozice tlačítek, barvy, poloměry zaoblení).Data jedním příkazem zkopírujete do tohoto SSBO.V Shaderu k nim přistoupíte přes index instance (gl_InstanceIndex) a okamžitě víte, jaké vlastnosti daný obdélník má.Máte už ve svém Vulkan kódu implementovaný systém pro správu paměti a vytváření Descriptor Setů, do kterých by se tento SSBO buffer nabindoval?
*/