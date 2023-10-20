﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Utb.Prednaska;

#nullable disable

namespace Utb.Prednaska.Migrations
{
    [DbContext(typeof(PizzaContext))]
    partial class PizzaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("Utb.Prednaska.Incredience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Incredience");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Cibule"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Hranolky"
                        });
                });

            modelBuilder.Entity("Utb.Prednaska.Pizza", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Cena")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PizzaStyleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PizzaStyleId");

                    b.ToTable("Pizzas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Cena = 100.0,
                            Name = "Margherita",
                            PizzaStyleId = 1
                        },
                        new
                        {
                            Id = 2,
                            Cena = 130.0,
                            Name = "Salami",
                            PizzaStyleId = 1
                        },
                        new
                        {
                            Id = 3,
                            Cena = 135.0,
                            Name = "Funghi",
                            PizzaStyleId = 2
                        });
                });

            modelBuilder.Entity("Utb.Prednaska.PizzaIncredience", b =>
                {
                    b.Property<int>("IncredienceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PizzaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IncredienceId", "PizzaId");

                    b.HasIndex("PizzaId");

                    b.ToTable("PizzaIncredience");

                    b.HasData(
                        new
                        {
                            IncredienceId = 1,
                            PizzaId = 1
                        },
                        new
                        {
                            IncredienceId = 2,
                            PizzaId = 1
                        });
                });

            modelBuilder.Entity("Utb.Prednaska.PizzaStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PizzaStyle");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Italsky styl"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Americky styl"
                        });
                });

            modelBuilder.Entity("Utb.Prednaska.Pizza", b =>
                {
                    b.HasOne("Utb.Prednaska.PizzaStyle", "PizzaStyle")
                        .WithMany("Pizzas")
                        .HasForeignKey("PizzaStyleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PizzaStyle");
                });

            modelBuilder.Entity("Utb.Prednaska.PizzaIncredience", b =>
                {
                    b.HasOne("Utb.Prednaska.Incredience", null)
                        .WithMany()
                        .HasForeignKey("IncredienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Utb.Prednaska.Pizza", null)
                        .WithMany()
                        .HasForeignKey("PizzaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Utb.Prednaska.PizzaStyle", b =>
                {
                    b.Navigation("Pizzas");
                });
#pragma warning restore 612, 618
        }
    }
}