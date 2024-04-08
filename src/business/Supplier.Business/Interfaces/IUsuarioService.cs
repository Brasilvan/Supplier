using Supplier.Business.Models;

namespace Supplier.Business.Interfaces
{
    public interface IUsuarioService : IDisposable
    {
        Task Adicionar(Usuario usuario);
    }
}
