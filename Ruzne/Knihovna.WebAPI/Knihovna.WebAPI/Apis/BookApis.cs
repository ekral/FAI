﻿using Knihovna.WebAPI.Data;
using Knihovna.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Knihovna.WebAPI.Apis
{
    public static class BookApis
    {
        public static IEndpointRouteBuilder MapBookApis(this IEndpointRouteBuilder app)
        {
            var bookItems = app.MapGroup("books");

            bookItems.MapGet("/", GetAllBooks).WithName("GetAllBooks");
            bookItems.MapGet("/{id:int}", GetBook);
            bookItems.MapPost("/", CreateBook);
            bookItems.MapPut("/{id:int}", UpdateBook);
            bookItems.MapDelete("/{id:int}", DeleteBook);

            return app;

        }

        public static async Task<Created> Seed(KnihovnaContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Knihy.AddRangeAsync(
                new Kniha() { KnihaId = 1, Nazev = "Svejk" },
                new Kniha() { KnihaId = 2, Nazev = "Temno" },
                new Kniha() { KnihaId = 3, Nazev = "Pan prstenu" }
                );

            await context.Ctenari.AddRangeAsync(
                new Ctenar() { CtenarId = 1, Jmeno = "Petr" },
                new Ctenar() { CtenarId = 2, Jmeno = "Erik" },
                new Ctenar() { CtenarId = 3, Jmeno = "Alena" }
                );

            await context.Vypujcky.AddRangeAsync(
                new Vypujcka() { VypujckaId = 1, KnihaId = 1, CtenarId = 1, DatumVypujcky = DateOnly.FromDateTime(DateTime.Now) }
                );

            await context.SaveChangesAsync();

            return TypedResults.Created();
        }

        public static async Task<Ok<Kniha[]>> GetAllBooks(KnihovnaContext context)
        {
            return TypedResults.Ok(await context.Knihy.ToArrayAsync());
        }

        public static async Task<Results<Ok<Kniha>, NotFound>> GetBook(int id, KnihovnaContext context)
        {
            if (await context.Knihy.FindAsync(id) is Kniha kniha)
            {
                return TypedResults.Ok(kniha);
            }
            else
            {
                return TypedResults.NotFound();
            }
        }

        public static async Task<Created<Kniha>> CreateBook(Kniha kniha, KnihovnaContext context)
        {
            await context.AddAsync(kniha);

            await context.SaveChangesAsync();

            return TypedResults.Created($"/books/{kniha.KnihaId}", kniha);
        }

        public static async Task<Results<NotFound, NoContent>> UpdateBook(int id, Kniha input, KnihovnaContext context)
        {
            if (await context.Knihy.FindAsync(id) is Kniha kniha)
            {
                kniha.Nazev = input.Nazev;

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        }

        public static async Task<Results<NotFound, NoContent>> DeleteBook(int id, KnihovnaContext context)
        {
            if (await context.Knihy.FindAsync(id) is Kniha kniha)
            {
                context.Remove(kniha);

                await context.SaveChangesAsync();

                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        }
    }
}
