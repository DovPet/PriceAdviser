using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PriceAdvisor.Migrations
{
    public partial class Populating_EShops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("INSERT INTO Eshops (AdministrationId,Name) VALUES ('1','ATEA')");
            migrationBuilder.Sql("INSERT INTO Eshops (AdministrationId,Name) VALUES ('2','Skytech')");
            migrationBuilder.Sql("INSERT INTO Eshops (AdministrationId,Name) VALUES ('2','Kilobaitas')");
            migrationBuilder.Sql("INSERT INTO Eshops (AdministrationId,Name) VALUES ('2','Fortakas')");
            migrationBuilder.Sql("INSERT INTO Eshops (AdministrationId,Name) VALUES ('2','TopoCentras')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.Sql("DELETE * FROM Eshops");
        }
    }
}
