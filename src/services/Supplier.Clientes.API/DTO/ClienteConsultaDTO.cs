using System.ComponentModel.DataAnnotations;

namespace Supplier.Clientes.API.DTO
{
    public class ClienteConsultaDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal ValorLimite { get; set; }
    }
}
