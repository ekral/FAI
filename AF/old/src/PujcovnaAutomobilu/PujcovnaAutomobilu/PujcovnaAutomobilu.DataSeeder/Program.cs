using PujcovnaAutomobilu.Models;

PujcovnaAutomobiluContext context = new();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Automobils.AddRange(
    new Automobil() { Id = 1, Model = "Škoda 105", Pujceno = false},
    new Automobil() { Id = 2, Model = "Citroen Berlingo", Pujceno = false },
    new Automobil() { Id = 3, Model = "Škoda Octavia", Pujceno = false }
    );

context.SaveChanges();