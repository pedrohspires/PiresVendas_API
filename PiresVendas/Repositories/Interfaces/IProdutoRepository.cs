using PiresVendas.DTOs;

namespace PiresVendas.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        public Task<int> CreateAsync(ProdutoDTO produtoDto);
        public Task<int> UpdateAsync(ProdutoDTO produtoDto, int produtoId);
        public Task<ProdutoDTO> GetByIdAsync(int produtoId);
        public Task<List<ProdutoDTO>> GetListAsync(PesquisaDto pesquisaDto);
        public Task<bool> DeleteAsync(int produtoId);
        public Task<List<SelectDto>> GetSelectAsync(PesquisaDto pesquisaDto);

    }
}
