﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210117172507_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Domain.GameState", b =>
                {
                    b.Property<int>("GameStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CurrentMoveByPlayerOne")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlayerOnePlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlayerTwoPlayerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameStateId");

                    b.HasIndex("PlayerOnePlayerId");

                    b.HasIndex("PlayerTwoPlayerId");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("GameEntities.Cell", b =>
                {
                    b.Property<int>("CellId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlayerId1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("XPosition")
                        .HasColumnType("INTEGER");

                    b.Property<int>("YPosition")
                        .HasColumnType("INTEGER");

                    b.Property<int>("_state")
                        .HasColumnType("INTEGER");

                    b.HasKey("CellId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerId1");

                    b.ToTable("Cell");
                });

            modelBuilder.Entity("GameEntities.GameBoard", b =>
                {
                    b.Property<int>("GameBoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameBoardId");

                    b.ToTable("GameBoard");
                });

            modelBuilder.Entity("GameEntities.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AttackBoardGameBoardId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OwnBoardGameBoardId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerId");

                    b.HasIndex("AttackBoardGameBoardId");

                    b.HasIndex("OwnBoardGameBoardId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Domain.GameState", b =>
                {
                    b.HasOne("GameEntities.Player", "PlayerOne")
                        .WithMany()
                        .HasForeignKey("PlayerOnePlayerId");

                    b.HasOne("GameEntities.Player", "PlayerTwo")
                        .WithMany()
                        .HasForeignKey("PlayerTwoPlayerId");

                    b.Navigation("PlayerOne");

                    b.Navigation("PlayerTwo");
                });

            modelBuilder.Entity("GameEntities.Cell", b =>
                {
                    b.HasOne("GameEntities.Player", null)
                        .WithMany("AttackSavedBoard")
                        .HasForeignKey("PlayerId");

                    b.HasOne("GameEntities.Player", null)
                        .WithMany("OwnSavedBoard")
                        .HasForeignKey("PlayerId1");
                });

            modelBuilder.Entity("GameEntities.Player", b =>
                {
                    b.HasOne("GameEntities.GameBoard", "AttackBoard")
                        .WithMany()
                        .HasForeignKey("AttackBoardGameBoardId");

                    b.HasOne("GameEntities.GameBoard", "OwnBoard")
                        .WithMany()
                        .HasForeignKey("OwnBoardGameBoardId");

                    b.Navigation("AttackBoard");

                    b.Navigation("OwnBoard");
                });

            modelBuilder.Entity("GameEntities.Player", b =>
                {
                    b.Navigation("AttackSavedBoard");

                    b.Navigation("OwnSavedBoard");
                });
#pragma warning restore 612, 618
        }
    }
}
