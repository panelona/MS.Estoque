using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using MS.GestaoEstoque.Interface;
using MS.GestaoEstoque.Models;

namespace MS.GestaoEstoque.RabbitMqClient
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public RabbitMqClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["MS_RABBITMQ_HOST"],
                Port = int.Parse(_configuration["MS_RABBITMQ_PORT"]),
                VirtualHost = _configuration["MS_RABBITMQ_VHOST"],
                UserName = _configuration["MS_RABBITMQ_USER"],
                Password = _configuration["MS_RABBITMQ_PASSWORD"]
            }.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _configuration["MS_RABBITMQ_EXCHANGE"], type: _configuration["MS_RABBITMQ_EXCHANGETYPE"]);
        }

        public void EnviaParaMsPedidos(PedidoDtoResponse pedido)
        {
            string msg = JsonSerializer.Serialize(pedido);
            var body = Encoding.UTF8.GetBytes(msg);

            _channel.BasicPublish(exchange: _configuration["MS_RABBITMQ_EXCHANGE"],
                routingKey: "pedidoDebug",
                basicProperties: null,
                body: body
                );
        }

        public void EnviaParaMsEmails(PedidoDtoResponse pedido)
        {
            string msg = JsonSerializer.Serialize(pedido);
            var body = Encoding.UTF8.GetBytes(msg);

            _channel.BasicPublish(exchange: _configuration["MS_RABBITMQ_EXCHANGE"],
                routingKey: "email",
                basicProperties: null,
                body: body
                );
        }
    }
}
