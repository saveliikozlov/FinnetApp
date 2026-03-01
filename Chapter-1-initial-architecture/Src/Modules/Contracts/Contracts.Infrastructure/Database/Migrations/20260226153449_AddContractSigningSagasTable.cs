using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvolutionaryArchitecture.Contracts.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddContractSigningSagasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractSigningSagas",
                schema: "Contracts",
                columns: table => new
                {
                    SagaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSigningSagas", x => x.SagaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractSigningSagas_CorrelationId",
                schema: "Contracts",
                table: "ContractSigningSagas",
                column: "CorrelationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractSigningSagas",
                schema: "Contracts");
        }
    }
}
