﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PriceAdvisor.Persistence;
using System;

namespace PriceAdvisor.Migrations
{
    [DbContext(typeof(PriceAdvisorDbContext))]
    [Migration("20180418120716_ProductEdited")]
    partial class ProductEdited
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PriceAdvisor.Core.Models.Administration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Scrapable");

                    b.HasKey("Id");

                    b.ToTable("Administrations");
                });

            modelBuilder.Entity("PriceAdvisor.Core.Models.Data", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Price");

                    b.HasKey("Id");

                    b.ToTable("Datas");
                });

            modelBuilder.Entity("PriceAdvisor.Core.Models.EShop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdministrationId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AdministrationId");

                    b.ToTable("Eshops");
                });

            modelBuilder.Entity("PriceAdvisor.Core.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EshopId");

                    b.Property<int>("ProductId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("EshopId");

                    b.HasIndex("ProductId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("PriceAdvisor.Core.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<bool>("Edited");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PriceAdvisor.Core.Models.EShop", b =>
                {
                    b.HasOne("PriceAdvisor.Core.Models.Administration", "Administration")
                        .WithMany("EShops")
                        .HasForeignKey("AdministrationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PriceAdvisor.Core.Models.Price", b =>
                {
                    b.HasOne("PriceAdvisor.Core.Models.EShop", "EShop")
                        .WithMany("Prices")
                        .HasForeignKey("EshopId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PriceAdvisor.Core.Models.Product", "Product")
                        .WithMany("Prices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
