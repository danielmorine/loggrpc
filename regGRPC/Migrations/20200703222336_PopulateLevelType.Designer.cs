﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using regGRPC;

namespace regGRPC.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200703222336_PopulateLevelType")]
    partial class PopulateLevelType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Scaffolds.EnvironmentType", b =>
                {
                    b.Property<byte>("ID")
                        .HasColumnName("EnvironmentTypeID")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(20)")
                        .HasMaxLength(20);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("VARCHAR(20)")
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.ToTable("EnvironmentType");
                });

            modelBuilder.Entity("Scaffolds.LevelType", b =>
                {
                    b.Property<byte>("ID")
                        .HasColumnName("LevelTypeID")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(50)")
                        .HasMaxLength(50);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("VARCHAR(50)")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("LevelType");
                });

            modelBuilder.Entity("Scaffolds.RegistrationProcess", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("RegistrationProcessID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte>("EnvironmentTypeID")
                        .HasColumnType("tinyint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReportID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("EnvironmentTypeID");

                    b.HasIndex("ReportID")
                        .IsUnique();

                    b.ToTable("RegistrationProcess");
                });

            modelBuilder.Entity("Scaffolds.Report", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ReportID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Events")
                        .HasColumnType("int");

                    b.Property<byte>("LevelTypeID")
                        .HasColumnType("tinyint");

                    b.Property<string>("ReportDescription")
                        .IsRequired()
                        .HasColumnType("VARCHAR(3000)")
                        .HasMaxLength(3000);

                    b.Property<string>("ReportSource")
                        .IsRequired()
                        .HasColumnType("VARCHAR(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasMaxLength(500);

                    b.HasKey("ID");

                    b.HasIndex("LevelTypeID");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("Scaffolds.RegistrationProcess", b =>
                {
                    b.HasOne("Scaffolds.EnvironmentType", "EnvironmentType")
                        .WithMany("RegistrationProcess")
                        .HasForeignKey("EnvironmentTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Scaffolds.Report", "Report")
                        .WithOne("RegistrationProcess")
                        .HasForeignKey("Scaffolds.RegistrationProcess", "ReportID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Scaffolds.Report", b =>
                {
                    b.HasOne("Scaffolds.LevelType", "LevelType")
                        .WithMany("Reports")
                        .HasForeignKey("LevelTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
