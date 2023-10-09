﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Utb.PizzaKiosk.Models;

#nullable disable

namespace Utb.PizzaKiosk.Models.Migrations
{
    [DbContext(typeof(PizzaKioskContext))]
    partial class PizzaKioskContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("Utb.PizzaKiosk.Models.PizzaConfigurationOption", b =>
                {
                    b.Property<int>("PizzaConfigurationOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PizzaConfigurationOptionId");

                    b.ToTable("PizzaOptions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PizzaConfigurationOption");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Utb.PizzaKiosk.Models.BooleanOption", b =>
                {
                    b.HasBaseType("Utb.PizzaKiosk.Models.PizzaConfigurationOption");

                    b.Property<bool>("DefaultValue")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("BooleanOption");

                    b.HasData(
                        new
                        {
                            PizzaConfigurationOptionId = 2,
                            Description = "Garling",
                            DefaultValue = true
                        });
                });

            modelBuilder.Entity("Utb.PizzaKiosk.Models.QuantityOption", b =>
                {
                    b.HasBaseType("Utb.PizzaKiosk.Models.PizzaConfigurationOption");

                    b.Property<int>("DefaultValue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximalValue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinimalValue")
                        .HasColumnType("INTEGER");

                    b.ToTable("PizzaOptions", t =>
                        {
                            t.Property("DefaultValue")
                                .HasColumnName("QuantityOption_DefaultValue");
                        });

                    b.HasDiscriminator().HasValue("QuantityOption");

                    b.HasData(
                        new
                        {
                            PizzaConfigurationOptionId = 3,
                            Description = "Number of pfeferoni",
                            DefaultValue = 1,
                            MaximalValue = 10,
                            MinimalValue = 0
                        });
                });

            modelBuilder.Entity("Utb.PizzaKiosk.Models.StringOptions", b =>
                {
                    b.HasBaseType("Utb.PizzaKiosk.Models.PizzaConfigurationOption");

                    b.Property<int>("DefaultValueIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Options")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("StringOptions");

                    b.HasData(
                        new
                        {
                            PizzaConfigurationOptionId = 1,
                            Description = "Pizza size",
                            DefaultValueIndex = 1,
                            Options = "[\"Small\",\"Medium\",\"Large\"]"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
