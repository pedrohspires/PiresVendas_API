using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PiresVendas.DTOs;
using PiresVendas.Repositories.Interfaces;

namespace PiresVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] ProdutoDTO produto)
        {
            try
            {
                var produtoId = await _produtoRepository.CreateAsync(produto);
                return Ok(produtoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update([FromBody] ProdutoDTO produto, int id)
        {
            try
            {
                var produtoId = await _produtoRepository.UpdateAsync(produto, id);
                return Ok(produtoId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTO>> GetById(int id)
        {
            try
            {
                var produto = await _produtoRepository.GetByIdAsync(id);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ProdutoDTO>> GetList([FromBody] PesquisaDto pesquisa)
        {
            try
            {
                var produto = await _produtoRepository.GetListAsync(pesquisa);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            try
            {
                var resultado = await _produtoRepository.DeleteAsync(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("select")]
        public async Task<ActionResult<ProdutoDTO>> GetSelect([FromBody] PesquisaDto pesquisa)
        {
            try
            {
                var produtos = await _produtoRepository.GetSelectAsync(pesquisa);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
