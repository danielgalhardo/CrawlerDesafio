﻿// <auto-generated />
using CrawlerAlura.src.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CrawlerAlura.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240903200715_AddWorkloadToAluraCourse")]
    partial class AddWorkloadToAluraCourse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.20");

            modelBuilder.Entity("CrawlerAlura.src.Domain.Entities.AluraCourse", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Instructor")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Link")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Workload")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AluraCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
