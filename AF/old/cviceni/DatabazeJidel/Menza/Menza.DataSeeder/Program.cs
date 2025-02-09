using Menza.Data;
using Menza.Models;

using var context = new MenzaContext();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Jidla.AddRange(
    new Jidlo() { Id = 1, Nazev = "Pizza", Cena = 90.0},
    new Jidlo() { Id = 2, Nazev = "Kureci platek", Cena = 88.9},
    new Jidlo() { Id = 3, Nazev = "Losos", Cena = 117.0}
);
context.SaveChanges();