using FluentValidation;
using Supplier.Business.Models.Validations.Documentos;

namespace Supplier.Business.Models.Validations
{
    public class ClienteValidation : AbstractValidator<Cliente>
    {
        public ClienteValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.Cpf.Length).Equal(CpfValidacao.TamanhoCpf)
                .WithMessage("O campo CPF precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

            RuleFor(c => CpfValidacao.Validar(c.Cpf)).Equal(true)
                .WithMessage("O CPF fornecido é inválido.");

            RuleFor(c => c.ValorLimite)
                .GreaterThan(-1).WithMessage("O campo {PropertyName} não pode ser negativo.");
        }
    }
}
