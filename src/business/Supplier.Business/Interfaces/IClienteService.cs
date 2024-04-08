using Supplier.Business.Models;

namespace Supplier.Business.Interfaces
{
    public interface IClienteService : IDisposable
    {
        Task<Cliente> Adicionar(Cliente cliente);
        Task AtualizarSaldo(TransacaoCliente transacaoCliente);
    }
}
