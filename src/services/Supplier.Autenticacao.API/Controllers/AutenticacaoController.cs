using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Supplier.Autenticacao.API.DTO;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Supplier.Core.Identidade;
using System.Security.Cryptography;

namespace Supplier.Autenticacao.API.Controllers
{
    [Route("api/autenticacao")]
    [ApiController]
    public class AutenticacaoController : MainController
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AutenticacaoController(IUsuarioRepository usuarioRepository,
                              IUsuarioService usuarioService,
                              IMapper mapper,
                              IOptions<AppSettings> appSettings,
                              INotificador notificador) : base(notificador)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<UsuarioDTO>> Cadastrar(UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _usuarioService.Adicionar(_mapper.Map<Usuario>(usuarioDTO));

            return CustomResponse(HttpStatusCode.Created, null, usuarioDTO, null);
        }

        [HttpPost("autenticar")]
        public ActionResult<string> Autenticar(UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var usuarioCadastrado = _usuarioRepository.Buscar(u => u.Email.ToLower() == usuarioDTO.Email.ToLower()).Result.FirstOrDefault();
            
            if (usuarioCadastrado == null)
            {
                NotificarErro("Usuário ou Senha inválidos.");
                return CustomResponse();
            }

            var loginValido = ValidarSenha(usuarioCadastrado, usuarioDTO.Senha);

            if (!loginValido)
            {
                NotificarErro("Usuário ou Senha inválidos.");
                return CustomResponse();
            }

            return GerarJwt(usuarioDTO.Email.ToLower());
        }

        private bool ValidarSenha(Usuario usuario, string senha)
        {
            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            for (int x = 0; x < computedHash.Length; x++)
            {
                if (computedHash[x] != usuario.PasswordHash[x]) return false;
            }

            return true;
        }        

        private string GerarJwt(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                   new Claim(ClaimTypes.Email, email),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            }); ;

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
