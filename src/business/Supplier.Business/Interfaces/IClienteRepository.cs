using Supplier.Business.Models;

namespace Supplier.Business.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> Cadastrar(Cliente cliente);
        Task AtualizarSaldo(TransacaoCliente transacaoCliente);
    }
}
