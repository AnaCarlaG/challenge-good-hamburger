using Application.DTOs.Request;
using Application.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace good_hamburger.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;    
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var itens = await _pedidoService.ObterTodosComItensAsync();
            return Ok(itens);
        }

        [HttpGet]
        [Route("/obterTodosComDeletados")]
        public async Task<IActionResult> ObterTodosComDeletados()
        {
            var itens = await _pedidoService.ObterTodosComDeletedAsync();
            return Ok(itens);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(id);
            return Ok(pedido);
        }
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoRequest request)
        {
            var pedido = await _pedidoService.CriarAsync(request.Itens);
            return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id },pedido);
        }
        [HttpPut]
        [Route("{id:guid}/acrescentar")]
        public async Task<IActionResult> AcrescentarPedidoAsync(Guid id, [FromBody] AtualizarPedidoRequest request)
        {
            var pedido = await _pedidoService.AcrescentarPedidoAsync(id, request.Itens);
            return Ok(pedido);
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarAsync(Guid id, [FromBody] AtualizarPedidoRequest request)
        {
            var pedido = await _pedidoService.AtualizarAsync(id, request.Itens);
            return Ok(pedido);
        }
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> AtualizarStatus (Guid id, [FromBody] AtualizarStatusRequest request)
        {
            var pedido = await _pedidoService.AtualizarStatusAsync(id, request.Status);
            return Ok(pedido);
        }
        [HttpDelete("{id:guid}/{isDeleted:bool}")]
        public async Task<IActionResult> Deletar(Guid id, bool isDeleted) 
        {
            await _pedidoService.RemoverAsync(id,isDeleted);
            return NoContent();
        }
    }
}
