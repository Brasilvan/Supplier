
namespace Supplier.Business.Interfaces
{
    public interface IAutenticacaoService : IDisposable
    {
        Task<bool> Autenticar(string email, string senha);
        Task<bool> UsuarioExiste(string email);
        string GerarToken(int id, string email);
    }
}
