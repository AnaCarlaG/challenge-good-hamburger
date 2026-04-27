using GoodHamburger.Web.Models.Enum;
using GoodHamburger.Web.Models.Reponse;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoodHamburger.Web.Services
{
    public class PedidoApiService
    {
        private readonly HttpClient _httpClient;

        public PedidoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<PedidoResponse>> ObterTodosAsync()
        {
            var response = await _httpClient.GetAsync("api/pedido");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PedidoResponse>>(content, _serializerOptions);
        }

        public async Task<PedidoResponse?> ObterPorIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/pedido/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PedidoResponse?>(content, _serializerOptions);
        }
        public async Task<PedidoResponse?> CriarAsync(IEnumerable<TipoItem> itens)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pedido", new { Itens = itens });
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PedidoResponse>(content,_serializerOptions);
        }
        public async Task<PedidoResponse?> AtualizarStatusAsync(Guid id, StatusPedido statusPedido)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/pedido/{id}/status", new { Status = statusPedido });
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            return JsonSerializer.Deserialize<PedidoResponse>(content,_serializerOptions);
        }
        public async Task RemoverAsync(Guid id) => await _httpClient.DeleteAsync($"api/pedido/{id}/false");
        public async Task<List<ItemCardapioResponse>> ObterCardapioAsync() 
        {
            var response = await _httpClient.GetAsync("api/cardapio");
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ItemCardapioResponse>>(content,_serializerOptions);
        }
    }
}
