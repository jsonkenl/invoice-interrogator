﻿// <auto-generated />
using InvoiceInterrogator.Core;
using InvoiceInterrogator.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace InvoiceInterrogator.Infrastructure.Migrations
{
    [DbContext(typeof(InvoiceInterrogatorDbContext))]
    [Migration("20180718213207_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InvoiceInterrogator.Core.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountCode");

                    b.Property<DateTime>("AccountTypeChangeDate");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("InvoiceInterrogator.Core.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DocVueId");

                    b.Property<string>("FileName");

                    b.Property<decimal>("InvoiceAmount");

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<string>("InvoiceNumber");

                    b.Property<bool>("Sampled");

                    b.Property<bool>("TaxIncluded");

                    b.Property<int?>("VendorId");

                    b.Property<string>("VoucherNumber");

                    b.HasKey("InvoiceId");

                    b.HasIndex("VendorId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("InvoiceInterrogator.Core.InvoiceAccount", b =>
                {
                    b.Property<int>("InvoiceId");

                    b.Property<int>("AccountId");

                    b.HasKey("InvoiceId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("InvoiceAccounts");
                });

            modelBuilder.Entity("InvoiceInterrogator.Core.Vendor", b =>
                {
                    b.Property<int>("VendorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Status");

                    b.Property<string>("VendorName");

                    b.Property<int>("VendorNumber");

                    b.Property<DateTime>("VendorStatusChangeDate");

                    b.HasKey("VendorId");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("InvoiceInterrogator.Core.Invoice", b =>
                {
                    b.HasOne("InvoiceInterrogator.Core.Vendor", "Vendor")
                        .WithMany("Invoices")
                        .HasForeignKey("VendorId");
                });

            modelBuilder.Entity("InvoiceInterrogator.Core.InvoiceAccount", b =>
                {
                    b.HasOne("InvoiceInterrogator.Core.Account", "Account")
                        .WithMany("InvoiceAccounts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InvoiceInterrogator.Core.Invoice", "Invoice")
                        .WithMany("InvoiceAccounts")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}