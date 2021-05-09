using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Add_DonationRecord_And_ReceptionRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Units = table.Column<int>(type: "int", nullable: false),
                    DonatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRecords_BloodDonors_DonatedById",
                        column: x => x.DonatedById,
                        principalTable: "BloodDonors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceptionRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Units = table.Column<int>(type: "int", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceptionRecords_BloodRecipients_ReceivedById",
                        column: x => x.ReceivedById,
                        principalTable: "BloodRecipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonationRecords_DonatedById",
                table: "DonationRecords",
                column: "DonatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionRecords_ReceivedById",
                table: "ReceptionRecords",
                column: "ReceivedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationRecords");

            migrationBuilder.DropTable(
                name: "ReceptionRecords");
        }
    }
}
