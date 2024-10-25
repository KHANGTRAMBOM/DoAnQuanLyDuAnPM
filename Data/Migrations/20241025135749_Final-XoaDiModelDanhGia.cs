using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalXoaDiModelDanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    IDNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiemDanhGia = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGia_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGia_Users_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_BookId",
                table: "DanhGia",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IDNguoiDung",
                table: "DanhGia",
                column: "IDNguoiDung");
        }
    }
}
