﻿// <auto-generated />
using System;
using DocumentStoreManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentStoreManagement.Infrastructure.Migrations.PostgresApplication
{
    [DbContext(typeof(PostgresApplicationContext))]
    partial class PostgresApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Document", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ReleaseQuantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Documents");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.OrderDetail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("DocumentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("UnitPrice")
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

                    b.ToTable("Books");
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

                    b.ToTable("Magazines");
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Newspaper", b =>
                {
                    b.HasBaseType("DocumentStoreManagement.Core.Models.Document");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.ToTable("Newspapers");
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

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Book", b =>
                {
                    b.HasOne("DocumentStoreManagement.Core.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("DocumentStoreManagement.Core.Models.Book", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Magazine", b =>
                {
                    b.HasOne("DocumentStoreManagement.Core.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("DocumentStoreManagement.Core.Models.Magazine", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Newspaper", b =>
                {
                    b.HasOne("DocumentStoreManagement.Core.Models.Document", null)
                        .WithOne()
                        .HasForeignKey("DocumentStoreManagement.Core.Models.Newspaper", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DocumentStoreManagement.Core.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
