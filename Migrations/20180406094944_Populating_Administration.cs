using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PriceAdvisor.Migrations
{
    public partial class Populating_Administration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Administrations (Scrapable) VALUES ('0')");
            migrationBuilder.Sql("INSERT INTO Administrations (Scrapable) VALUES ('1')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE * FROM Administrations");
        }
    }
}
