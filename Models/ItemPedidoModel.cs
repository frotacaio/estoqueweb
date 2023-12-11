using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueWeb.Models
{
    [Table("ItemPedido")]
    public class ItemPedidoModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPedido { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public double ValorUnitario { get; set; }
        public PedidoModel? Pedido { get; set; }
        public ProdutoModel? Produto { get; set; }
        [NotMapped] // Especifica que o campo não irá para o campo, será usando em tempo de execução
        public double ValorItem {get => this.Quantidade * this.ValorUnitario;}  //função lambda

    }
}