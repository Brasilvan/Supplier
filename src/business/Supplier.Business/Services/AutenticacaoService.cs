using Supplier.Business.Interfaces;

namespace Supplier.Business.Services
{
    public class AutenticacaoService : BaseService, IAutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoService(IUsuarioRepository usuarioRepository,
                                   INotificador notificador) : base(notificador)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Task<bool> Autenticar(string email, string senha)
        {
            throw new NotImplementedException();
        }

        public string GerarToken(int id, string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UsuarioExiste(string email)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
