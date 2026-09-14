#include <etl/ext_vector.h>
#include <cstddef>
#include <memory>

// ... předpokládejme stejné proměnné jako výše ...

alignas(16) std::byte memoryPool[4096];
void* ptr = memoryPool;
size_t space = sizeof(memoryPool);
size_t sizeNeeded = (count + safetyBuffer) * sizeof(VkExtensionProperties);

if (std::align(alignof(VkExtensionProperties), sizeNeeded, ptr, space)) {
    VkExtensionProperties* typedPtr = reinterpret_cast<VkExtensionProperties*>(ptr);

    // ETL ext_vector obalí paměť. Konstruktor bere: (ukazatel, maximální_kapacita)
    // Na začátku je prázdný (velikost 0), ale ví, že maximum je (count + safetyBuffer)
    etl::ext_vector<VkExtensionProperties> safeEtlVector(typedPtr, count + safetyBuffer);

    // Protože Vulkan očekává, že paměť je připravená k zápisu, 
    // zvětšíme vektor na plnou kapacitu (alokuje prvky v rámci předané paměti)
    safeEtlVector.resize(count + safetyBuffer);

    // Volání Vulkanu
    uint32_t callCount = count;
    // vkEnumerateInstanceExtensionProperties(..., &callCount, safeEtlVector.data());

    // Upravíme velikost na to, co reálně driver zapsal
    safeEtlVector.resize(std::min(callCount, (uint32_t)safeEtlVector.capacity()));
    
    // Nyní safeEtlVector hlídá jakékoliv přetečení při práci ve vašem kódu.
}
