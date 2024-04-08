using Microsoft.AspNetCore.Mvc;
using Supplier.Business.Models;

namespace Supplier.Business.Interfaces
{
    public interface ITransacaoService : IDisposable
    {
        Task<TransacaoCliente> Autorizar(TransacaoCliente transacaoCliente);
    }
}
