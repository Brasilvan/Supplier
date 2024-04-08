using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using System.Text;
using System.Text.Json;

namespace Supplier.Clientes.API.RabbitConsumer
{
    public class RabbitTransacaoCliente : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitTransacaoCliente(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "transacaoCliente",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var transacao = JsonSerializer.Deserialize<TransacaoCliente>(contentString, options);

                NotificarCliente(transacao);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: "transacaoCliente",
                                  autoAck: false,
                                  consumer: consumer);

            return Task.CompletedTask;
        }

        public void NotificarCliente(TransacaoCliente transacao)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetRequiredService<IClienteService>();
                notificationService.AtualizarSaldo(transacao);
            }
        }
    }
}
