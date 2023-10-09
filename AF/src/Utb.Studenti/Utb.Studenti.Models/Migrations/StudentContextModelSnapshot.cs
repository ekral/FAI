﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Utb.Studenti.Models;

#nullable disable

namespace Utb.Studenti.Models.Migrations
{
    [DbContext(typeof(StudentContext))]
    partial class StudentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Utb.Studenti.Models.Skupina", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nazev")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Skupiny");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nazev = "swi1"
                        });
                });

            modelBuilder.Entity("Utb.Studenti.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Jmeno")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SkupinaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SkupinaId");

                    b.ToTable("Studenti");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Jmeno = "Bohumil",
                            SkupinaId = 1
                        });
                });

            modelBuilder.Entity("Utb.Studenti.Models.Student", b =>
                {
                    b.HasOne("Utb.Studenti.Models.Skupina", "Skupina")
                        .WithMany("Studenti")
                        .HasForeignKey("SkupinaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skupina");
                });

            modelBuilder.Entity("Utb.Studenti.Models.Skupina", b =>
                {
                    b.Navigation("Studenti");
                });
#pragma warning restore 612, 618
        }
    }
}