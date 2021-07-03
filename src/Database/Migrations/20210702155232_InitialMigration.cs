﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mappers",
                columns: table => new
                {
                    MapperId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mappers", x => x.MapperId);
                });

            migrationBuilder.CreateTable(
                name: "Nominators",
                columns: table => new
                {
                    NominatorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominators", x => x.NominatorId);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    MapId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BeatmapsetId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OsuModificationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MapperId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_Maps_Mappers_MapperId",
                        column: x => x.MapperId,
                        principalTable: "Mappers",
                        principalColumn: "MapperId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapNominator",
                columns: table => new
                {
                    MapsMapId = table.Column<int>(type: "INTEGER", nullable: false),
                    NominatorsNominatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapNominator", x => new { x.MapsMapId, x.NominatorsNominatorId });
                    table.ForeignKey(
                        name: "FK_MapNominator_Maps_MapsMapId",
                        column: x => x.MapsMapId,
                        principalTable: "Maps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapNominator_Nominators_NominatorsNominatorId",
                        column: x => x.NominatorsNominatorId,
                        principalTable: "Nominators",
                        principalColumn: "NominatorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapNominator_NominatorsNominatorId",
                table: "MapNominator",
                column: "NominatorsNominatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_MapperId",
                table: "Maps",
                column: "MapperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapNominator");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Nominators");

            migrationBuilder.DropTable(
                name: "Mappers");
        }
    }
}