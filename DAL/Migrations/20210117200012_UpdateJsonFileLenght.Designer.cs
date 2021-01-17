﻿// <auto-generated />
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210117200012_UpdateJsonFileLenght")]
    partial class UpdateJsonFileLenght
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("GameEntities.SavedGameData", b =>
                {
                    b.Property<int>("SavedGameDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("JsonData")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SavedGameDataId");

                    b.ToTable("SavedGameDataEntries");
                });
#pragma warning restore 612, 618
        }
    }
}