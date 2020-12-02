﻿// <auto-generated />
using System;
using DataBaseApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataBaseApi.Migrations
{
    [DbContext(typeof(BooksContext))]
    [Migration("20201202175900_Migr")]
    partial class Migr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DataBaseApi.Models.Book", b =>
                {
                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("BlockCreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("BlockPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CoverCreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CoverPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Barcode");

                    b.ToTable("Books");
                });
#pragma warning restore 612, 618
        }
    }
}