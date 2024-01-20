using connect_dentes_API;
using Microsoft.EntityFrameworkCore;
using PiresVendas.DTOs;
using PiresVendas.Entities;
using PiresVendas.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PiresVendas.Repositories.Implementations
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly MyDbContext _dbContext;

        public ProdutoRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static ProdutoDTO ConvertToDTO(Produto? produto)
        {
            var produtoDto = new ProdutoDTO();

            produtoDto.Id = produto.Id;
            produtoDto.Descricao = produto.Descricao;
            produtoDto.Preco = produto.Preco;
            produtoDto.QuantidadeMinima = produto.QuantidadeMinima;
            produtoDto.Ativo = produto.Ativo;
            produtoDto.DataCadastro = produto.DataCadastro;
            produtoDto.DataEdicao = produto.DataEdicao;

            return produtoDto;
        }

        public void VerificaDTO(ProdutoDTO produtoDto)
        {
            if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
                throw new Exception("Informe a 'Descrição'!");

            if (produtoDto.Preco == null)
                throw new Exception("Informe o 'Valor'");

            if (produtoDto.QuantidadeMinima == null)
                throw new Exception("Informe a 'Quantidade Mínima'");
        }

        public async Task<int> CreateAsync(ProdutoDTO produtoDto)
        {
            VerificaDTO(produtoDto);
            var novoProduto = new Produto();

            novoProduto.Descricao = produtoDto.Descricao;
            novoProduto.Preco = produtoDto.Preco.Value;
            novoProduto.QuantidadeMinima = produtoDto.QuantidadeMinima.Value;
            novoProduto.Ativo = produtoDto.Ativo ?? true;

            await _dbContext.AddAsync(novoProduto);
            await _dbContext.SaveChangesAsync();

            return novoProduto.Id;
        }

        public async Task<int> UpdateAsync(ProdutoDTO produtoDto, int produtoId)
        {
            VerificaDTO(produtoDto);
            var produtoAlterar = await _dbContext.Produtos.FindAsync(produtoId);
            if (produtoAlterar == null)
                throw new Exception("Produto não encontrado");

            produtoAlterar.Descricao = produtoDto.Descricao;
            produtoAlterar.Preco = produtoDto.Preco.Value;
            produtoAlterar.QuantidadeMinima = produtoDto.QuantidadeMinima.Value;
            produtoAlterar.Ativo = produtoDto.Ativo ?? true;

            await _dbContext.SaveChangesAsync();

            return produtoAlterar.Id;
        }

        public async Task<ProdutoDTO> GetByIdAsync(int produtoId)
        {
            var produto = await _dbContext.Produtos.FindAsync(produtoId);
            if (produto == null)
                throw new Exception("Produto não encontrado");

            return ConvertToDTO(produto);
        }

        public async Task<List<ProdutoDTO>> GetListAsync(PesquisaDto pesquisaDto)
        {
            var dbQuery = _dbContext.Produtos.AsQueryable();

            if (pesquisaDto.Pesquisa != null)
            {
                var search = $"%{pesquisaDto.Pesquisa.Replace(" ", "%")}%";
                dbQuery = dbQuery.Where(x => x.Id.ToString().Equals(search) ||
                                             x.Descricao.Equals(search));
            }

            var produtosLista = await dbQuery.ToListAsync();
            return produtosLista.Select(x => ConvertToDTO(x)).ToList();
        }

        public async Task<bool> DeleteAsync(int produtoId)
        {
            var produto = await _dbContext.Produtos.FindAsync(produtoId);
            if (produto == null)
                throw new Exception("Produto não encontrado");

            try
            {
                _dbContext.Remove(produto);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar produto");
            }

            return true;
        }

        public async Task<List<SelectDto>> GetSelectAsync(PesquisaDto pesquisaDto)
        {
            var dbQuery = _dbContext.Produtos.AsQueryable();

            if (pesquisaDto.Pesquisa != null)
            {
                var search = $"%{pesquisaDto.Pesquisa.Replace(" ", "%")}%";
                dbQuery = dbQuery.Where(x => x.Id.ToString().Equals(search) ||
                                             x.Descricao.Equals(search));
            }

            var produtosLista = await dbQuery.ToListAsync();
            return produtosLista.Select(x => new SelectDto
            {
                Id = x.Id,
                Descricao = $"{x.Id} - {x.Descricao}",
            })
            .ToList();
        }
    }
}
