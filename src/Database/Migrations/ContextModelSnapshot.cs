﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("Database.Map", b =>
                {
                    b.Property<long>("MapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Artist")
                        .HasColumnType("TEXT");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MapperId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OsuAprovedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OsuSubmitDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OsuUpdateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("TEXT");

                    b.HasKey("MapId");

                    b.HasIndex("MapperId");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("Database.Mapper", b =>
                {
                    b.Property<long>("MapperId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("MapperId");

                    b.ToTable("Mappers");
                });

            modelBuilder.Entity("Database.Nominator", b =>
                {
                    b.Property<long>("NominatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("NominatorId");

                    b.ToTable("Nominators");
                });

            modelBuilder.Entity("MapNominator", b =>
                {
                    b.Property<long>("MapsMapId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("NominatorsNominatorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MapsMapId", "NominatorsNominatorId");

                    b.HasIndex("NominatorsNominatorId");

                    b.ToTable("MapNominator");
                });

            modelBuilder.Entity("Database.Map", b =>
                {
                    b.HasOne("Database.Mapper", "Mapper")
                        .WithMany("Maps")
                        .HasForeignKey("MapperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mapper");
                });

            modelBuilder.Entity("MapNominator", b =>
                {
                    b.HasOne("Database.Map", null)
                        .WithMany()
                        .HasForeignKey("MapsMapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Nominator", null)
                        .WithMany()
                        .HasForeignKey("NominatorsNominatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Database.Mapper", b =>
                {
                    b.Navigation("Maps");
                });
#pragma warning restore 612, 618
        }
    }
}
