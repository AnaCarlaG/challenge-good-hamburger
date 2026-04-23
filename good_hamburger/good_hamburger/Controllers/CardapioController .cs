using Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace good_hamburger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardapioController :ControllerBase
    {
        private readonly IItemCardapioService _itemCardapioService;
        public CardapioController(IItemCardapioService itemCardapioService)
        {
            _itemCardapioService = itemCardapioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos() 
        {
            var itens = await _itemCardapioService.ObterTodosAsync();
            return Ok(itens);
        }
    }
}
