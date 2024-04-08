using Supplier.Business.Interfaces;
using Supplier.Business.Models.Validations;
using Supplier.Business.Models;
using System.Security.Cryptography;
using System.Text;

namespace Supplier.Business.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                              INotificador notificador) : base(notificador)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task Adicionar(Usuario usuario)
        {
            if (!ExecutaValidacao(new UsuarioValidation(), usuario)) return;

            var emailJaCadastrado = _usuarioRepository.Buscar(u => u.Email == usuario.Email).Result.Any();

            if (emailJaCadastrado)
            {
                Notificar("Já existe um Usuário com o Email informado.");
                return;
            }

            var IdJaCadastrado = _usuarioRepository.ObterPorId(usuario.Id).Result;

            if (IdJaCadastrado != null)
            {
                Notificar("Já existe um Usuário com o ID informado.");
                return;
            }

            var criptoSenha = CriarHashSenha(usuario.Senha);

            usuario.PasswordHash = criptoSenha.PasswordHash;
            usuario.PasswordSalt = criptoSenha.PasswordSalt;

            await _usuarioRepository.Adicionar(usuario);
        }

        private Usuario CriarHashSenha(string senha)
        {
            using var hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            byte[] passwordSalt = hmac.Key;
            var user = new Usuario()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            return user;
        }

        public void Dispose()
        {
            _usuarioRepository?.Dispose();
        }
    }
}