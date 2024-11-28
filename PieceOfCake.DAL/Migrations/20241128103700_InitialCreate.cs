#pragma warning disable IDE0005 // Using directive is unnecessary.
using System;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable IDE0161 // Convert to file-scoped namespace
namespace PieceOfCake.DAL.Migrations
#pragma warning restore IDE0161 // Convert to file-scoped namespace
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    ServingSize = table.Column<byte>(type: "tinyint", maxLength: 255, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealOfTheDayTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOfTheDayTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasureUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Calendar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DishMealOfTheDayType",
                columns: table => new
                {
                    DishesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealOfTheDayTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishMealOfTheDayType", x => new { x.DishesId, x.MealOfTheDayTypesId });
                    table.ForeignKey(
                        name: "FK_DishMealOfTheDayType_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishMealOfTheDayType_MealOfTheDayTypes_MealOfTheDayTypesId",
                        column: x => x.MealOfTheDayTypesId,
                        principalTable: "MealOfTheDayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishMenu",
                columns: table => new
                {
                    DishesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishMenu", x => new { x.DishesId, x.MenusId });
                    table.ForeignKey(
                        name: "FK_DishMenu_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishMenu_Menus_MenusId",
                        column: x => x.MenusId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealOfTheDayTypeMenu",
                columns: table => new
                {
                    MealOfTheDayTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOfTheDayTypeMenu", x => new { x.MealOfTheDayTypesId, x.MenusId });
                    table.ForeignKey(
                        name: "FK_MealOfTheDayTypeMenu_MealOfTheDayTypes_MealOfTheDayTypesId",
                        column: x => x.MealOfTheDayTypesId,
                        principalTable: "MealOfTheDayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOfTheDayTypeMenu_Menus_MenusId",
                        column: x => x.MenusId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    MeasureUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingredients_MeasureUnits_MeasureUnitId",
                        column: x => x.MeasureUnitId,
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingredients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishMealOfTheDayType_MealOfTheDayTypesId",
                table: "DishMealOfTheDayType",
                column: "MealOfTheDayTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_DishMenu_MenusId",
                table: "DishMenu",
                column: "MenusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_DishId",
                table: "Ingredients",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_MeasureUnitId",
                table: "Ingredients",
                column: "MeasureUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ProductId",
                table: "Ingredients",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOfTheDayTypeMenu_MenusId",
                table: "MealOfTheDayTypeMenu",
                column: "MenusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishMealOfTheDayType");

            migrationBuilder.DropTable(
                name: "DishMenu");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "MealOfTheDayTypeMenu");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "MeasureUnits");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "MealOfTheDayTypes");

            migrationBuilder.DropTable(
                name: "Menus");
        }
    }
}
