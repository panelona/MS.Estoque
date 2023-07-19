using System.Text.Json.Serialization;

namespace MS.GestaoEstoque.Models
{
    public class PedidoDtoRequest
    {
        [JsonPropertyName("numeroPedido")]
        public int NumeroPedido { get; set; }
        [JsonPropertyName("itens")]
        public IEnumerable<string> Itens { get; set; }
        [JsonPropertyName("emailCliente")]
        public string EmailCliente { get; set; }
    }
}
