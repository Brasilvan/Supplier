using FluentValidation;
using Supplier.Business.Models.Validations.Documentos;

namespace Supplier.Business.Models.Validations
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(u => EmailValidacao.Validar(u.Email)).Equal(true)
                .WithMessage("O Email fornecido é inválido.");

            //RuleFor(u => u.Senha)
            //    .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
            //    .Length(8, 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");
        }
    }
}
