using PujcovnaAutomobilu.Models;

PujcovnaAutomobiluContext context = new();

context.Database.EnsureCreated();
context.Database.EnsureCreated();

context.Automobils.AddRange(
    new Automobil() { Id = 1, Model = "Škoda 105"},
    new Automobil() { Id = 2, Model = "Citroen Berlingo"},
    new Automobil() { Id = 3, Model = "Škoda Octavia"}
    );

context.SaveChanges();