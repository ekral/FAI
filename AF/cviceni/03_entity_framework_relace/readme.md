# 03 Entity framework relace studentů

**autor: Erik Král ekral@utb.cz**

---

Vytvořte následující relace:

1) Vztah 1:N Student : Skupina
2) Vztah 1:1 Student : Karta (zůstatek na kartě)
3) Vztah N:M Student : Predmet

Vytvořte funkce pro 

- Seed databáze.
- VypisSkupinyEager - vypíše skupiny a studenty a zůstatek na kartě  ve skupině pomocí Eager Loading.
- VypisSkupinyExplicit - vypíše skupiny a studenty ve skupině pomocí Explicit Loading .
- VypisPredmetuStudentu - vypíše studenty a jejich předměty pomocí Eager Loading.

Vytvořte konzolovou aplikaci umožňující spustit vytvořené metody.

