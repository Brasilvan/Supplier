using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Data.Context;
using System.Text;
using System.Text.Json;

namespace Supplier.Data.Repository
{
    public class TransacaoRepository : Repository<TransacaoCliente>, ITransacaoRepository
    {
        public TransacaoRepository(MeuDbContext context) : base(context)
        {
        }

        [HttpPost]
        public async Task<TransacaoCliente> Autorizar(TransacaoCliente transacaoCliente)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "transacaoCliente",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string transacaoSerializada = JsonSerializer.Serialize(transacaoCliente);
                var transacaoBytes = Encoding.UTF8.GetBytes(transacaoSerializada);

                channel.BasicPublish(exchange: "",
                                     routingKey: "transacaoCliente",
                                     basicProperties: null,
                                     body: transacaoBytes);

                transacaoCliente.Id = Guid.NewGuid();

                return transacaoCliente;
            }
        }        
    }
}
