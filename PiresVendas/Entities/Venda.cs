using System.ComponentModel.DataAnnotations.Schema;

namespace PiresVendas.Entities
{
    public class Venda : BaseEntity
    {
        [Column("id_produto")]
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        [Column("quantidade")]
        public decimal Quantidade { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("desconto")]
        public decimal Desconto { get; set; }
    }
}
