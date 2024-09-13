using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quaze.Migrations
{
    /// <inheritdoc />
    public partial class QuizImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageGuid",
                table: "Quizes",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageGuid",
                table: "Quizes");
        }
    }
}
