using System.ComponentModel.DataAnnotations.Schema;

namespace PiresVendas.Entities
{
    [Table("produto")]
    public class Produto : BaseEntity
    {
        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("preco")]
        public decimal Preco { get; set; }

        [Column("quantidade_minima")]
        public decimal QuantidadeMinima { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }
    }
}
