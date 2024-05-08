using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkillCraft.EntityFrameworkCore.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CreateStorageSummaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorageSummaries",
                columns: table => new
                {
                    StorageSummaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActorId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Allocated = table.Column<long>(type: "bigint", nullable: false),
                    Used = table.Column<long>(type: "bigint", nullable: false),
                    Remaining = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageSummaries", x => x.StorageSummaryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageSummaries_ActorId",
                table: "StorageSummaries",
                column: "ActorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorageSummaries_Allocated",
                table: "StorageSummaries",
                column: "Allocated");

            migrationBuilder.CreateIndex(
                name: "IX_StorageSummaries_Remaining",
                table: "StorageSummaries",
                column: "Remaining");

            migrationBuilder.CreateIndex(
                name: "IX_StorageSummaries_Used",
                table: "StorageSummaries",
                column: "Used");

            migrationBuilder.CreateIndex(
                name: "IX_StorageSummaries_UserId",
                table: "StorageSummaries",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageSummaries");
        }
    }
}
