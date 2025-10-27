using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PublishedYear = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            // Добавляем начальные данные авторов
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name", "DateOfBirth" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Лев Толстой", new DateOnly(1828, 9, 9) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Фёдор Достоевский", new DateOnly(1821, 11, 11) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Антон Чехов", new DateOnly(1860, 1, 29) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Александр Пушкин", new DateOnly(1799, 6, 6) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Николай Гоголь", new DateOnly(1809, 4, 1) }
                });

            // Добавляем начальные данные книг
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Title", "PublishedYear", "AuthorId" },
                values: new object[,]
                {
                    // Книги Толстого
                    { new Guid("66666666-6666-6666-6666-666666666661"), "Война и мир", 1869, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-6666-6666-6666-666666666662"), "Анна Каренина", 1877, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-6666-6666-6666-666666666663"), "Воскресение", 1899, new Guid("11111111-1111-1111-1111-111111111111") },
                    
                    // Книги Достоевского
                    { new Guid("77777777-7777-7777-7777-777777777771"), "Преступление и наказание", 1866, new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("77777777-7777-7777-7777-777777777772"), "Идиот", 1869, new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("77777777-7777-7777-7777-777777777773"), "Братья Карамазовы", 1880, new Guid("22222222-2222-2222-2222-222222222222") },
                    
                    // Книги Чехова
                    { new Guid("88888888-8888-8888-8888-888888888881"), "Вишнёвый сад", 1904, new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("88888888-8888-8888-8888-888888888882"), "Три сестры", 1901, new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("88888888-8888-8888-8888-888888888883"), "Чайка", 1896, new Guid("33333333-3333-3333-3333-333333333333") },
                    
                    // Книги Пушкина
                    { new Guid("99999999-9999-9999-9999-999999999991"), "Евгений Онегин", 1833, new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("99999999-9999-9999-9999-999999999992"), "Капитанская дочка", 1836, new Guid("44444444-4444-4444-4444-444444444444") },
                    
                    // Книги Гоголя
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), "Мёртвые души", 1842, new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), "Ревизор", 1836, new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), "Вечера на хуторе близ Диканьки", 1831, new Guid("55555555-5555-5555-5555-555555555555") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // При откате удаляем сначала книги, потом авторов
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}