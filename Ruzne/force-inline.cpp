#include <concepts>

// Makro pro vynucený inlining napříč kompilátory
#if defined(_MSC_VER)
    #define FORCE_INLINE __forceinline
#else
    #define FORCE_INLINE __attribute__((always_inline)) inline
#endif

template <typename F>
requires std::invocable<F, int>
struct MojeTrida {
    const F akce;

    constexpr MojeTrida(F zadanaAkce) : akce(zadanaAkce) {}

    // FORCE_INLINE garantuje, že tělo funkce se vloží přímo do procesorového pipeline
    FORCE_INLINE constexpr bool Spust(int x) const {
        return akce(x);
    }
};
