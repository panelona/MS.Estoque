using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using MS.GestaoEstoque.Interface;

namespace MS.GestaoEstoque.RabbitMqClient
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _nomeDaFila;
        private readonly IConnection _connection;
        private IModel _channel;
        private IProcessaEvento _processaEvento;

        public RabbitMqSubscriber(IConfiguration configuration, IProcessaEvento processaEvento)
        {
            _configuration = configuration;
            _processaEvento = processaEvento;
            var HostName = _configuration["MS_RABBITMQ_HOST"];
            var Port = int.Parse(_configuration["MS_RABBITMQ_PORT"]);
            var VirtualHost = _configuration["MS_RABBITMQ_VHOST"];
            var UserName = _configuration["MS_RABBITMQ_USER"];
            var Password = _configuration["MS_RABBITMQ_PASSWORD"];


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
            _nomeDaFila = _channel.QueueDeclare(_configuration["MS_RABBITMQ_ESTOQUE_QUEUENAME"]).QueueName;
            _channel.QueueBind(queue: _nomeDaFila, exchange: _configuration["MS_RABBITMQ_EXCHANGE"], routingKey: _configuration["MS_RABBITMQ_ESTOQUE_ROUTINGKEY"]);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumidor = new EventingBasicConsumer(_channel);
            consumidor.Received += (ch, ea) =>
            {
                var conteudo = Encoding.UTF8.GetString(ea.Body.ToArray());

                _processaEvento.Processa(conteudo);
            };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumidor);

            return Task.CompletedTask;
        }
    }
}
