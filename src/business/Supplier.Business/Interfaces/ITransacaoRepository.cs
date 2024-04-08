
using Supplier.Business.Models;

namespace Supplier.Business.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<TransacaoCliente> Autorizar(TransacaoCliente transacaoCliente);        
    }
}
