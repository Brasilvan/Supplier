using Microsoft.AspNetCore.Http.HttpResults;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Business.Models.Validations;

namespace Supplier.Business.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository,
                              INotificador notificador) : base(notificador)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> Adicionar(Cliente cliente)
        {
            if (!ExecutaValidacao(new ClienteValidation(), cliente)) return cliente;
                                                
            var cpfJaCadastrado = _clienteRepository.Buscar(c => c.Cpf == cliente.Cpf).Result.Any();

            if (cpfJaCadastrado)
            {
                Notificar("Já existe um Cliente com o CPF informado.");
                return cliente;
            }

            return await _clienteRepository.Cadastrar(cliente);
        }

        public async Task AtualizarSaldo(TransacaoCliente transacaoCliente)
        {
            var cliente = _clienteRepository.ObterPorId(transacaoCliente.IdCliente).Result;

            if (cliente == null || cliente.Cpf == null)
            {
                Notificar("Cliente não cadastrado");
                return;
            }

            if (transacaoCliente.Valor > cliente.ValorLimite)
            {
                Notificar("Saldo insuficiente");
                return;
            }

            await _clienteRepository.AtualizarSaldo(transacaoCliente);
        }

        public void Dispose()
        {
            _clienteRepository?.Dispose();
        }
    }
}
