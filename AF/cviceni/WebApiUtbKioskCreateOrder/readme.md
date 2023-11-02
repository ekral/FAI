# Úkol na cvičení: Vytvoření objednávky (metoda CreateOrder)

Aktualizujte si repozitář (proveďte pull nebo si naklonujte repozitář znovu).

[https://github.com/ekral/FAI.git](https://github.com/ekral/FAI.git)

V solution [Utb.PizzaKiosk.sln](https://github.com/ekral/FAI/blob/master/AF/src/Utb.PizzaKiosk) a projektu [Utb.PizzaKiosk.WebApi](https://github.com/ekral/FAI/blob/master/AF/src/Utb.PizzaKiosk/Utb.PizzaKosk.WebApi):

- Vyzkoušejte soubor [app.http](https://github.com/ekral/FAI/blob/master/AF/src/Utb.PizzaKiosk/Utb.PizzaKosk.WebApi/app.http) (Jen nové Visual Studio). Pokud nepoužíváte Visual Studio, tak můžete pro dotazy použít například aplikaci [Postman](https://www.postman.com).
- Spusťte jako první konzolovou aplikaci **DataSeeder**, která vytvoří databází (a předtím odstraní případně jejích předchozí verzi) a naplní databázi výchozími daty.
- Doprogramujte metodu [CreateOrder](https://github.com/ekral/FAI/blob/master/AF/src/Utb.PizzaKiosk/Utb.PizzaKosk.WebApi/Program.cs#L75).

Metoda CreateOrder používá DTO (Data Transfer Objects) pro přednání informací o nové objednávce. V těle metody zatím vkládá jen testovací data. Doplňte vytvoření objednávky dle parametů funkce. Všimněte si, že pro zadání OrderedPizza a OrderedIngredient se v metodě používá navigační property místo cizího klíče. Díky tomu Entity Framework doplní automaticky hodnoty cizích klíčů aniž bychom je předem znali.
