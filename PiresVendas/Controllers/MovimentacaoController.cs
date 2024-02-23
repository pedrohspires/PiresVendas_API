using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PiresVendas.DTOs;
using PiresVendas.Repositories.Interfaces;

namespace PiresVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public MovimentacaoController(IMovimentacaoRepository movimentacaoRepository)
        {
            _movimentacaoRepository = movimentacaoRepository;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] MovimentacaoDTO movimentacao)
        {
            try
            {
                var movimentacaoId = await _movimentacaoRepository.CreateAsync(movimentacao);
                return Ok(movimentacaoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update([FromBody] MovimentacaoDTO movimentacao, int id)
        {
            try
            {
                var movimentacaoId = await _movimentacaoRepository.UpdateAsync(movimentacao, id);
                return Ok(movimentacaoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoDTO>> GetById(int id)
        {
            try
            {
                var movimentacao = await _movimentacaoRepository.GetByIdAsync(id);
                return Ok(movimentacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Listagem")]
        public async Task<ActionResult<MovimentacaoDTO>> GetList([FromBody] PesquisaDto pesquisa)
        {
            try
            {
                var movimentacao = await _movimentacaoRepository.GetListAsync(pesquisa);
                return Ok(movimentacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MovimentacaoDTO>> Delete(int id)
        {
            try
            {
                var resultado = await _movimentacaoRepository.DeleteAsync(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("select")]
        public async Task<ActionResult<MovimentacaoDTO>> GetSelect([FromBody] PesquisaDto pesquisa)
        {
            try
            {
                var movimentacaos = await _movimentacaoRepository.GetSelectAsync(pesquisa);
                return Ok(movimentacaos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
