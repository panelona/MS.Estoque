using MS.GestaoEstoque.Models;

namespace MS.GestaoEstoque.Interface
{
    public interface IRabbitMqClient
    {
        void EnviaParaMsPedidos(PedidoDtoResponse pedido);
        void EnviaParaMsEmails(PedidoDtoResponse pedido);

    }
}
