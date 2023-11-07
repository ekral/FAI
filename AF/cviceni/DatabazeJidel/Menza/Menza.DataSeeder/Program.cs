using Menza.Data;
using Menza.Models;

using var context = new MenzaContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Jidla.AddRange(
    new Jidlo() { Id = 1, Nazev = "Pizza", Cena = 150},
    new Jidlo() { Id = 2, Nazev = "Pizza2", Cena = 200},
    new Jidlo() { Id = 3, Nazev = "Pizza3", Cena = 250}
);
context.SaveChanges();