using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Supplier.Business.Models.Validations;

namespace Supplier.Business.Services
{
    public class TransacaoService : BaseService, ITransacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(HttpClient httpClient,
                                ITransacaoRepository transacaoRepository,
                                INotificador notificador) : base(notificador)
        {
            _httpClient = httpClient;
            _transacaoRepository = transacaoRepository;
        }        

        public async Task<TransacaoCliente> Autorizar(TransacaoCliente transacaoCliente)
        {
            if (!ExecutaValidacao(new TransacaoValidation(), transacaoCliente)) return transacaoCliente;

            var transacao = new TransacaoCliente();

            var baseUrl = AppSettings("ClientesUrl");
            _httpClient.BaseAddress = new System.Uri(baseUrl);

            var ResponseClient = await _httpClient.GetAsync($"/api/clientes/obter-por-id/{transacaoCliente.IdCliente}");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var cliente = JsonSerializer.Deserialize<ClienteConsulta>(await ResponseClient.Content.ReadAsStringAsync(), options);

            if (cliente == null || cliente.Cpf == null)
            {
                Notificar("Cliente não cadastrado");
                return transacao;
            }

            if (transacaoCliente.Valor > cliente.ValorLimite)
            {
                Notificar("Saldo insuficiente");
                return transacao;
            }

            return await _transacaoRepository.Autorizar(transacaoCliente);
        }        

        private Guid GerarNumeroSimulacao()
        {
            return Guid.NewGuid();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
