namespace MS.GestaoEstoque.Models
{
    public class PedidoDtoResponse
    {
        public int NumeroPedido { get; set; }
        public IEnumerable<string> Itens { get; set; }
        public string EmailCliente { get; set; }
        public string? Mensagem { get; set; }
    }
}
