using connect_dentes_API;
using Microsoft.EntityFrameworkCore;
using PiresVendas.DTOs;
using PiresVendas.Entities;
using PiresVendas.Repositories.Interfaces;
using PiresVendas.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PiresVendas.Repositories.Implementations
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly MyDbContext _dbContext;

        public MovimentacaoRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static MovimentacaoDTO ConvertToDTO(Movimentacao? movimentacao)
        {
            var dto = new MovimentacaoDTO();

            dto.Id = movimentacao.Id;
            dto.ProdutoId = movimentacao.ProdutoId;
            dto.Produto = movimentacao.Produto;
            dto.Quantidade = movimentacao.Quantidade;
            dto.Valor = movimentacao.Valor;
            dto.Desconto = movimentacao.Desconto;
            dto.Tipo = movimentacao.Tipo;

            dto.DataCadastro = movimentacao.DataCadastro;
            dto.DataEdicao = movimentacao.DataEdicao;

            return dto;
        }

        public async void VerificaDTO(MovimentacaoDTO dto)
        {
            Produto? movimentacao = null;
            if (dto.ProdutoId == null)
                throw new Exception("Selecione um 'Produto'!");
            else
            {
                movimentacao = await _dbContext.Produtos.FindAsync(dto.ProdutoId);
                if (movimentacao == null)
                    throw new Exception("Produto informado não encontrado!");
            }

            if (dto.Quantidade == null || dto.Quantidade < 1)
                throw new Exception("Informe uma quantidade válida (maior que zero)!");

            if (dto.Valor == null || dto.Valor < 1)
                throw new Exception("Informe um valor válida (maior que zero)!");

            if (dto.Tipo == null)
                throw new Exception("Selecione um 'Tipo' para a movimentação!");
            else
            {
                if (dto.Tipo == Tipos.Venda)
                {
                    var totalCompras = await _dbContext.Movimentacoes.Where(x => x.Tipo == Tipos.Compra).Select(x => x.Quantidade).SumAsync();
                    var totalVendas = await _dbContext.Movimentacoes.Where(x => x.Tipo == Tipos.Venda).Select(x => x.Quantidade).SumAsync();
                    var totalProdutos = totalCompras - totalVendas;

                    if (dto.Quantidade > totalProdutos)
                        throw new Exception("Estoque indisponível para venda!");
                }
            }
        }

        public async Task<int> CreateAsync(MovimentacaoDTO dto)
        {
            VerificaDTO(dto);
            var movimentacao = new Movimentacao();

            movimentacao.ProdutoId = dto.ProdutoId.Value;
            movimentacao.Quantidade = dto.Quantidade.Value;
            movimentacao.Valor = dto.Valor.Value;
            movimentacao.Desconto = dto.Desconto ?? 0;
            movimentacao.Tipo = dto.Tipo.Value;

            await _dbContext.AddAsync(movimentacao);
            await _dbContext.SaveChangesAsync();

            return movimentacao.Id;
        }

        public async Task<int> UpdateAsync(MovimentacaoDTO dto, int id)
        {
            VerificaDTO(dto);
            var item = await _dbContext.Movimentacoes.FindAsync(id);
            if (item == null)
                throw new Exception("Movimentação não encontrada");

            item.ProdutoId = dto.ProdutoId.Value;
            item.Quantidade = dto.Quantidade.Value;
            item.Valor = dto.Valor.Value;
            item.Desconto = dto.Desconto ?? 0;
            item.Tipo = dto.Tipo.Value;

            await _dbContext.SaveChangesAsync();

            return item.Id;
        }

        public async Task<MovimentacaoDTO> GetByIdAsync(int id)
        {
            var movimentacao = await _dbContext.Movimentacoes.Include(x => x.Produto).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (movimentacao == null)
                throw new Exception("Movimentacao não encontrada");

            return ConvertToDTO(movimentacao);
        }

        public async Task<List<MovimentacaoDTO>> GetListAsync(PesquisaDto pesquisaDto)
        {
            var dbQuery = _dbContext.Movimentacoes.Include(x => x.Produto).AsQueryable();

            if (pesquisaDto.Pesquisa != null)
            {
                var search = $"%{pesquisaDto.Pesquisa.Replace(" ", "%")}%";
                dbQuery = dbQuery.Where(x => x.Id.ToString().Equals(search) ||
                                             x.Produto.Descricao.Equals(search));
            }

            var movimentacaosLista = await dbQuery.ToListAsync();
            return movimentacaosLista.Select(x => ConvertToDTO(x)).ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movimentacao = await _dbContext.Movimentacoes.FindAsync(id);
            if (movimentacao == null)
                throw new Exception("Movimentação não encontrado");

            try
            {
                _dbContext.Remove(movimentacao);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar movimentação");
            }

            return true;
        }

        public async Task<List<SelectDto>> GetSelectAsync(PesquisaDto pesquisaDto)
        {
            var dbQuery = _dbContext.Movimentacoes.Include(x => x.Produto).AsQueryable();

            if (pesquisaDto.Pesquisa != null)
            {
                var search = $"%{pesquisaDto.Pesquisa.Replace(" ", "%")}%";
                dbQuery = dbQuery.Where(x => x.Id.ToString().Equals(search) ||
                                             x.Produto.Descricao.Equals(search));
            }

            var movimentacaosLista = await dbQuery.ToListAsync();
            return movimentacaosLista.Select(x => new SelectDto
            {
                Id = x.Id,
                Descricao = $"{x.Id} - {x.Produto.Descricao} ({(x.Tipo == Tipos.Venda ? "Venda" : "Compra")})",
            })
            .ToList();
        }
    }
}
