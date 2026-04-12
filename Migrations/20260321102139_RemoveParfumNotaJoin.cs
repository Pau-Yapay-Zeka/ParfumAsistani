using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParfumAsistani.Migrations
{
    /// <inheritdoc />
    public partial class RemoveParfumNotaJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParfumNota");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParfumNota",
                columns: table => new
                {
                    NotalarId = table.Column<int>(type: "int", nullable: false),
                    ParfumlerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParfumNota", x => new { x.NotalarId, x.ParfumlerId });
                    table.ForeignKey(
                        name: "FK_ParfumNota_Notalar_NotalarId",
                        column: x => x.NotalarId,
                        principalTable: "Notalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParfumNota_Parfumler_ParfumlerId",
                        column: x => x.ParfumlerId,
                        principalTable: "Parfumler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParfumNota_ParfumlerId",
                table: "ParfumNota",
                column: "ParfumlerId");
        }
    }
}
