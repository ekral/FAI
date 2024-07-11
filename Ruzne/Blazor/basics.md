# Render modes

Blender má několik renderovacích módů. Nejjednoduší je **Static Server Rendering (SSR)**, který renderuje celý dokument na straně serveru. Díky Enhance Navigation, která je jako výchozí stav zapnutá, tak nedochází k obnovení celého dokumentu, ale jen jeho části pomocí fetch. K tomuto se využívá javasriptový skript na klientovi. Další vylepšení je 

- Render modes, v SSR jen formulare, lepsi zadavat az pri pouziti misto webassembly muzu dat server
- Kdy se provadi refresh UI
- implicitni a explicitni syntaxe, u if a foreach musi byt slozene zavorky

---
1. [ASP.NET Core Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-8.0)
2. [Enhanced navigation and form handling](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-8.0#enhanced-navigation-and-form-handling)