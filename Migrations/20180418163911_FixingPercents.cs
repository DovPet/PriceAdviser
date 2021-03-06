﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PriceAdvisor.Migrations
{
    public partial class FixingPercents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percents",
                table: "Administrations");

            migrationBuilder.AddColumn<int>(
                name: "Percents",
                table: "Eshops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percents",
                table: "Eshops");

            migrationBuilder.AddColumn<int>(
                name: "Percents",
                table: "Administrations",
                nullable: false,
                defaultValue: 0);
        }
    }
}
