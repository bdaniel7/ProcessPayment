using Microsoft.EntityFrameworkCore.Migrations;

namespace ProcessPayment.Model.Migrations
{
    public partial class AddPaymentStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PaymentStates",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Pending" });

            migrationBuilder.InsertData(
                table: "PaymentStates",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Processed" });

            migrationBuilder.InsertData(
                table: "PaymentStates",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Failed" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StateId",
                table: "Payments",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentStates_StateId",
                table: "Payments",
                column: "StateId",
                principalTable: "PaymentStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentStates_StateId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentStates");

            migrationBuilder.DropIndex(
                name: "IX_Payments_StateId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Payments");
        }
    }
}
