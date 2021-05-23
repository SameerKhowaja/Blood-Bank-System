using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationRecords_BloodDonors_DonatedById",
                table: "DonationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceptionRecords_BloodRecipients_ReceivedById",
                table: "ReceptionRecords");

            migrationBuilder.AlterColumn<int>(
                name: "ReceivedById",
                table: "ReceptionRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "ReceptionRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DonatedById",
                table: "DonationRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DonorId",
                table: "DonationRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationRecords_BloodDonors_DonatedById",
                table: "DonationRecords",
                column: "DonatedById",
                principalTable: "BloodDonors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceptionRecords_BloodRecipients_ReceivedById",
                table: "ReceptionRecords",
                column: "ReceivedById",
                principalTable: "BloodRecipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationRecords_BloodDonors_DonatedById",
                table: "DonationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceptionRecords_BloodRecipients_ReceivedById",
                table: "ReceptionRecords");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "ReceptionRecords");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "DonationRecords");

            migrationBuilder.AlterColumn<int>(
                name: "ReceivedById",
                table: "ReceptionRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DonatedById",
                table: "DonationRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationRecords_BloodDonors_DonatedById",
                table: "DonationRecords",
                column: "DonatedById",
                principalTable: "BloodDonors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceptionRecords_BloodRecipients_ReceivedById",
                table: "ReceptionRecords",
                column: "ReceivedById",
                principalTable: "BloodRecipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
