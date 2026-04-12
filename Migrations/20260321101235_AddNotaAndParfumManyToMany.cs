using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParfumAsistani.Migrations
{
    /// <inheritdoc />
    public partial class AddNotaAndParfumManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GorselUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notalar", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParfumNota");

            migrationBuilder.DropTable(
                name: "Notalar");
        }
    }
}
