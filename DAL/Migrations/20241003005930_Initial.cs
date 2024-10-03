using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Author = table.Column<string>(maxLength: 100, nullable: false),
                    ISBN = table.Column<string>(maxLength: 30, nullable: false),
                    PublicationYear = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("1f4494ff-8bb8-4967-9350-c9c4a8d7d697"), "Some books which are very scary!", "Horror" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("dbd5a981-6d85-41ba-9139-1c7d1feaf27a"), "Yuk!", "Romance" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "ISBN", "PublicationYear", "Quantity", "Title" },
                values: new object[] { new Guid("cc49ae9a-c1a2-4226-a5a4-5627646afaf2"), "Old vampire", new Guid("1f4494ff-8bb8-4967-9350-c9c4a8d7d697"), "978-617-7171-80-4", 2004, 3, "Scary story" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "ISBN", "PublicationYear", "Quantity", "Title" },
                values: new object[] { new Guid("6f1ad537-062f-44bf-aec9-57d78b2053a4"), "Witch next door", new Guid("1f4494ff-8bb8-4967-9350-c9c4a8d7d697"), "978-617-3121-80-4", 2010, 0, "Not scary story" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            string procedureFilePath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    AppDomain.CurrentDomain.RelativeSearchPath,
                    "Scripts",
                    "GetBooksProcedure.sql");

            migrationBuilder.Sql(File.ReadAllText(procedureFilePath));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBooks;");
        }
    }
}
