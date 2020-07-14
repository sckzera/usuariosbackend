using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<Guid>(nullable: false),
                    tipoUsuario = table.Column<int>(maxLength: 1, nullable: false),
                    nome = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: false),
                    senha = table.Column<string>(nullable: false),
                    ra = table.Column<string>(maxLength: 6, nullable: true),
                    telefone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
