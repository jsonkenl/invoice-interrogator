using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace InvoiceInterrogator.Infrastructure.Migrations
{
    public partial class AddUseTaxReturnDateToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UseTaxReturnDate",
                table: "Invoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseTaxReturnDate",
                table: "Invoices");
        }
    }
}
