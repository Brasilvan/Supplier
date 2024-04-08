using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supplier.Autenticacao.API.DTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
