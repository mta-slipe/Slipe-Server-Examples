﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SlipeTeamDeathmatch.Persistence;

#nullable disable

namespace SlipeTeamDeathmatch.Migrations
{
    [DbContext(typeof(TdmContext))]
    [Migration("20220414163044_Accounts")]
    partial class Accounts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("SlipeTeamDeathmatch.Models.Account", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeathCount")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsGuest")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KillCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MatchCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
