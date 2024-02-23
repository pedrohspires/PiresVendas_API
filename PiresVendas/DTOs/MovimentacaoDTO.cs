using PiresVendas.Entities;
using PiresVendas.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace PiresVendas.DTOs
{
    public class MovimentacaoDTO : BaseDTO
    {
        public int? ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public decimal? Quantidade { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Desconto { get; set; }

        public Tipos? Tipo { get; set; }
    }
}
