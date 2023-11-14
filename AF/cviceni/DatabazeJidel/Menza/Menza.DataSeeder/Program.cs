using Menza.Data;
using Menza.Models;

using var context = new MenzaContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Jidla.AddRange(
    new Jidlo() { Id = 1, Nazev = "Pizza", Cena = 150.0},
    new Jidlo() { Id = 2, Nazev = "Kuřecí medailonky", Cena = 88.8},
    new Jidlo() { Id = 3, Nazev = "Karbanátek z lososa,", Cena = 116.0}
);
context.SaveChanges();