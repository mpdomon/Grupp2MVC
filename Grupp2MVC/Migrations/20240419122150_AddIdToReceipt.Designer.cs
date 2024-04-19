﻿// <auto-generated />
using System;
using Grupp2MVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Grupp2MVC.Migrations
{
    [DbContext(typeof(Grupp2MVCContext))]
    [Migration("20240419122150_AddIdToReceipt")]
    partial class AddIdToReceipt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Grupp2MVC.Models.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeOfArrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeOfDeparture")
                        .HasColumnType("datetime2");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("Grupp2MVC.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AmountOfWheels")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeOfArrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TimeOfDeparture")
                        .HasColumnType("datetime2");

                    b.Property<int>("VehicleType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("Grupp2MVC.Models.Receipt", b =>
                {
                    b.HasOne("Grupp2MVC.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });
#pragma warning restore 612, 618
        }
    }
}
