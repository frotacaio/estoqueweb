using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueWeb.Migrations
{
    public partial class Versao7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemPedido",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "INTEGER", nullable: false),
                    IdProduto = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorUnitario = table.Column<double>(type: "REAL", nullable: false),
                    PedidoIdPedido = table.Column<int>(type: "INTEGER", nullable: true),
                    ProdutoIdProduto = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPedido", x => new { x.IdPedido, x.IdProduto });
                    table.ForeignKey(
                        name: "FK_ItemPedido_Pedido_PedidoIdPedido",
                        column: x => x.PedidoIdPedido,
                        principalTable: "Pedido",
                        principalColumn: "IdPedido");
                    table.ForeignKey(
                        name: "FK_ItemPedido_Produto_ProdutoIdProduto",
                        column: x => x.ProdutoIdProduto,
                        principalTable: "Produto",
                        principalColumn: "IdProduto");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedido_PedidoIdPedido",
                table: "ItemPedido",
                column: "PedidoIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedido_ProdutoIdProduto",
                table: "ItemPedido",
                column: "ProdutoIdProduto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPedido");
        }
    }
}
