using FluentValidation;

namespace Supplier.Business.Models.Validations
{
    public class TransacaoValidation : AbstractValidator<TransacaoCliente>
    {
        public TransacaoValidation()
        {
            RuleFor(t => t.Valor)
                .GreaterThan(0).WithMessage("O campo {PropertyName} tem que ser positivo (maior que zero).");
        }
    }
}
