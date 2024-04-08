using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Data.Context;

namespace Supplier.Data.Repository
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository 
    {
        public ClienteRepository(MeuDbContext context) : base(context)
        {
        }        

        public async Task AtualizarSaldo(TransacaoCliente transacaoCliente)
        {
            var cliente = ObterPorId(transacaoCliente.IdCliente).Result;

            if (cliente != null && cliente.Cpf != null)
            {
                cliente.ValorLimite -= transacaoCliente.Valor;

                await Atualizar(cliente);
            }
        }

        public async Task<Cliente> Cadastrar(Cliente cliente)
        {
            await Adicionar(cliente);

            return cliente;
        }

    }
}
