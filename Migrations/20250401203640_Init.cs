using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    ParentCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    StoreUrl = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Labels_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    LabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Values_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductStores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductUrl = table.Column<string>(type: "text", nullable: true),
                    LatestPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStores_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValueId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductValues_Values_ValueId",
                        column: x => x.ValueId,
                        principalTable: "Values",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductStoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_ProductStores_ProductStoreId",
                        column: x => x.ProductStoreId,
                        principalTable: "ProductStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImageUrl", "Name", "ParentCategoryId", "Slug" },
                values: new object[,]
                {
                    { new Guid("12c593c9-2fc2-4ef6-b537-9ed6a95f2e96"), null, "Voće", null, "voce" },
                    { new Guid("17e63574-d2b8-4a74-b94d-3210fc0b4186"), null, "Meso", null, "meso" },
                    { new Guid("91ac1be2-b97c-47ed-902d-712a96d8b0f0"), null, "Pića", null, "pica" },
                    { new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), null, "Mliječni proizvodi i jaja", null, "mlijecni-proizvodi-i-jaja" },
                    { new Guid("f91a4d2c-dbe7-42ad-a3cd-7d2a0f557ec6"), null, "Povrće", null, "povrce" },
                    { new Guid("0d4667bc-9675-4e55-8d3c-6f4763f053b2"), null, "Sladoledi", new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), "sladoledi" },
                    { new Guid("1ac3b0d3-d60e-4979-8886-94b43f194e28"), null, "Plodovi mora", new Guid("17e63574-d2b8-4a74-b94d-3210fc0b4186"), "plodovi-mora" },
                    { new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), null, "Mlijeko", new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), "mlijeko" },
                    { new Guid("41da84c7-0196-4bde-8e0c-bd20688e6e63"), null, "Čaj", new Guid("91ac1be2-b97c-47ed-902d-712a96d8b0f0"), "caj" },
                    { new Guid("5a16ab3e-05ae-4679-9d96-77f0db2c47a3"), null, "Perad", new Guid("17e63574-d2b8-4a74-b94d-3210fc0b4186"), "perad" },
                    { new Guid("5de62c74-fb47-468a-b56d-5d9d38208039"), null, "Kava", new Guid("91ac1be2-b97c-47ed-902d-712a96d8b0f0"), "kava" },
                    { new Guid("742ff5e3-92e6-4a89-8d77-d49a4e92065c"), null, "Govedina", new Guid("17e63574-d2b8-4a74-b94d-3210fc0b4186"), "govedina" },
                    { new Guid("7b3a5662-d9f6-48ad-b6ea-6fce8c92715a"), null, "Jaja", new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), "jaja" },
                    { new Guid("7f4d2586-e464-4b18-9e4f-d4b38c553295"), null, "Gazirana pića", new Guid("91ac1be2-b97c-47ed-902d-712a96d8b0f0"), "gazirana-pica" },
                    { new Guid("a3f95625-4ec1-4222-8cdc-b491c4356f9c"), null, "Alkoholna pića", new Guid("91ac1be2-b97c-47ed-902d-712a96d8b0f0"), "alkoholna-pica" },
                    { new Guid("aa4c9e5f-3e8b-4d99-a04e-dadfe2a5e10e"), null, "Jogurti", new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), "jogurti" },
                    { new Guid("aad5b759-df12-4937-a1f5-91c30a0f4e90"), null, "Sirevi", new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), "sirevi" },
                    { new Guid("b625a4f2-533b-4a33-822b-488457325b5d"), null, "Maslac", new Guid("f5a5d762-7a8f-4a8e-8f30-d9f4db59b7f0"), "maslac" },
                    { new Guid("e9e34ab3-1bdb-4005-9508-d084071f5850"), null, "Svinjsko meso", new Guid("17e63574-d2b8-4a74-b94d-3210fc0b4186"), "svinjsko-meso" }
                });

            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "CategoryId", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78b"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "Trajnost mlijeka", "trajnost-mlijeka" },
                    { new Guid("43b33b76-69cc-4424-8608-eeb20f931476"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "Mliječna masnoća", "mlijecna-masnoca" },
                    { new Guid("bf131788-50bf-413a-94c2-92c8b1de1acf"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "Proizvođač", "proizvoac" },
                    { new Guid("cd54d494-4e7f-4a36-ac2b-0c5b47f1c96b"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "Vrsta mlijeka", "vrsta-mlijeka" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "ImageUrl", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("04133ce5-7349-443d-afb6-83b6a4dc99ed"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Dukat Svježe mlijeko 3,2% m.m. 1 l", "dukat-svjeze-mlijeko-3-2-m-m-1-l" },
                    { new Guid("0f2b8014-346d-4585-a68d-a2683e9f7ace"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Z bregov Trajno mlijeko 0,9% m.m. 1 l", "z-bregov-trajno-mlijeko-0-9-m-m-1-l" },
                    { new Guid("46e4e5b5-1ac4-4c56-a816-424757676beb"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", " Z bregov Svježe mlijeko 3,2% m.m. 1 l", "z-bregov-svjeze-mlijeko-3-2-m-m-1-l" },
                    { new Guid("4cc38c19-b4da-4708-a582-dc1852dacb30"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Belje Kravica Kraljica Trajno mlijeko 2,8% m.m. 1 l", "belje-kravica-kraljica-trajno-mlijeko-2-8-m-m-1-l" },
                    { new Guid("559388ed-55f8-4a0d-bd49-76615bda30ab"), new Guid("aa4c9e5f-3e8b-4d99-a04e-dadfe2a5e10e"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Dukatos Grčki tip jogurta 150 g", "dukatos-grcki-tip-jogurta-150-g" },
                    { new Guid("695f806c-048e-4858-a49a-bfd8c34932af"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Z bregov Svježe mlijeko 3,2% m.m. 1,75 l", "z-bregov-svjeze-mlijeko-3-2-m-m-1-75-l" },
                    { new Guid("7bc71537-30ec-4122-b44b-73d1cf9da71c"), new Guid("aa4c9e5f-3e8b-4d99-a04e-dadfe2a5e10e"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Dukat Tekući jogurt 2,8% m.m. 1 kg", "dukat-tekuci-jogurt-2-8-m-m-1-kg" },
                    { new Guid("7f8aadfd-1c53-4f68-b6f6-216091111261"), new Guid("aa4c9e5f-3e8b-4d99-a04e-dadfe2a5e10e"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "b Aktiv LGG jogurt 2,4% m.m. 1 kg", "b-aktiv-lgg-jogurt-2-4-m-m-1-kg" },
                    { new Guid("839cf81b-29d6-4397-8244-aaed755428ca"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Dukat Lagano jutro Trajno mlijeko bez laktoze 1,5% m.m 1 l", "dukat-lagano-jutro-trajno-mlijeko-bez-laktoze-1-5-m-m-1-l" },
                    { new Guid("90be72d4-0f10-41e7-9e95-944fbcebc898"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "K Plus Trajno mlijeko 2,8% m.m. 1 l", "k-plus-trajno-mlijeko-2-8-m-m-1-l" },
                    { new Guid("a3e14b35-f6d5-4f6c-b0f4-18fa798a0e9d"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Alpsko Trajno mlijeko 3,5% m.m. 1 l", "alpsko-trajno-mlijeko-3-5-m-m-1-l" },
                    { new Guid("a6d6f654-2551-41ea-af85-f9b976852d20"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Z bregov Mlijeko bez laktoze 2,8% m.m. 1 l", "z-bregov-mlijeko-bez-laktoze-2-8-m-m-1-l" },
                    { new Guid("f5efe512-7e4b-4fe6-9a7c-4001b70b945e"), new Guid("aa4c9e5f-3e8b-4d99-a04e-dadfe2a5e10e"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "b Aktiv LGG Jogurt 2,4% m.m. natur 330 g", "b-aktiv-lgg-jogurt-2-4-m-m-natur-330-g" },
                    { new Guid("faf662d0-7006-41a7-9000-96895f8912a4"), new Guid("2d15ec67-b47b-4756-bc7b-e7a4e4f1232f"), "https://dukatshop.hr/965-thickbox_default/dukat-svjeze-mlijeko-32-mm-1-l.jpg", "Dukat Trajno mlijeko 2,8% m.m. 1 l", "dukat-trajno-mlijeko-2-8-m-m-1-l" }
                });

            migrationBuilder.InsertData(
                table: "Values",
                columns: new[] { "Id", "LabelId", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("2a99e355-fcf1-46c8-8b5b-019bdbfc99de"), new Guid("bf131788-50bf-413a-94c2-92c8b1de1acf"), "Moja kravica", "moja-kravica" },
                    { new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78c"), new Guid("cd54d494-4e7f-4a36-ac2b-0c5b47f1c96b"), "Kravlje mlijeko", "kravlje-mlijeko" },
                    { new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78d"), new Guid("cd54d494-4e7f-4a36-ac2b-0c5b47f1c96b"), "Kozje mlijeko", "kozje-mlijeko" },
                    { new Guid("43b33b76-69cc-4424-8608-eeb20f931477"), new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78b"), "Svježe mlijeko", "svjeze-mlijeko" },
                    { new Guid("48923d87-1b7e-4121-9f87-023e71712203"), new Guid("43b33b76-69cc-4424-8608-eeb20f931476"), "iznad 3%", "iznad-3" },
                    { new Guid("95e493da-1772-4be5-8038-250a5453c2bb"), new Guid("2ce02301-83c2-4fb7-8397-185a4ea2e78b"), "Trajno mlijeko", "trajno-mlijeko" },
                    { new Guid("b93dc567-d6d3-4dae-b3bd-9a4b990a5f6b"), new Guid("43b33b76-69cc-4424-8608-eeb20f931476"), "1% do 2%", "1-do-2" },
                    { new Guid("bb601c25-43f5-403b-8ec1-92c470ec35e5"), new Guid("43b33b76-69cc-4424-8608-eeb20f931476"), "2% do 3%", "2-do-3" },
                    { new Guid("bf131788-50bf-413a-94c2-92c8b1de1adf"), new Guid("43b33b76-69cc-4424-8608-eeb20f931476"), "do 1%", "do-1" },
                    { new Guid("d813d858-809f-49e2-8f87-2b69b37b194b"), new Guid("bf131788-50bf-413a-94c2-92c8b1de1acf"), "Dukat", "dukat" },
                    { new Guid("e4006d51-2e28-46be-8747-159956b6fdbb"), new Guid("bf131788-50bf-413a-94c2-92c8b1de1acf"), "z bregov", "z-bregov" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Labels_CategoryId",
                table: "Labels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_Slug",
                table: "Labels",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_Amount",
                table: "Prices",
                column: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductStoreId",
                table: "Prices",
                column: "ProductStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStores_ProductId",
                table: "ProductStores",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStores_StoreId",
                table: "ProductStores",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductValues_ProductId",
                table: "ProductValues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductValues_ValueId",
                table: "ProductValues",
                column: "ValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Slug",
                table: "Stores",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Values_LabelId",
                table: "Values",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Values_Slug",
                table: "Values",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "ProductValues");

            migrationBuilder.DropTable(
                name: "ProductStores");

            migrationBuilder.DropTable(
                name: "Values");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
