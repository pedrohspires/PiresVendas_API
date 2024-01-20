using System.ComponentModel.DataAnnotations.Schema;

namespace PiresVendas.DTOs
{
    public class ProdutoDTO
    {
        public int? Id { get; set; }
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; }
        public decimal? QuantidadeMinima { get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataEdicao { get; set; }
    }
}
