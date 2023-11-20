﻿// <auto-generated />
using System;
using DocumentStoreManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentStoreManagement.Infrastructure.Migrations.PostgresApplication
{
    [DbContext(typeof(PostgresApplicationContext))]
    [Migration("20231025022908_UpdateFieldsLength")]
    partial class UpdateFieldsLength
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Document", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ReleaseQuantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("Documents");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Document");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.OrderDetail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("DocumentId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("character varying(128)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Book", b =>
                {
                    b.HasBaseType("DocumentStoreManagement.Core.Models.Document");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("PageNumber")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("Book");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Magazine", b =>
                {
                    b.HasBaseType("DocumentStoreManagement.Core.Models.Document");

                    b.Property<string>("ReleaseMonth")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("ReleaseNumber")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("Magazine");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Newspaper", b =>
                {
                    b.HasBaseType("DocumentStoreManagement.Core.Models.Document");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasDiscriminator().HasValue("Newspaper");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.OrderDetail", b =>
                {
                    b.HasOne("DocumentStoreManagement.Core.Models.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DocumentStoreManagement.Core.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
