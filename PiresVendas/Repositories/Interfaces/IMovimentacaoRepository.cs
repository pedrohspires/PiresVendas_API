using PiresVendas.DTOs;

namespace PiresVendas.Repositories.Interfaces
{
    public interface IMovimentacaoRepository
    {
        public Task<int> CreateAsync(MovimentacaoDTO dto);
        public Task<int> UpdateAsync(MovimentacaoDTO dto, int id);
        public Task<MovimentacaoDTO> GetByIdAsync(int id);
        public Task<List<MovimentacaoDTO>> GetListAsync(PesquisaDto dto);
        public Task<bool> DeleteAsync(int id);
        public Task<List<SelectDto>> GetSelectAsync(PesquisaDto dto);
    }
}
