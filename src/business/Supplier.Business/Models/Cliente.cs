
namespace Supplier.Business.Models
{
    public class Cliente : Entity
    {
        public Guid Id { get; private set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public decimal ValorLimite { get; set; }

        public Cliente()
        {
            Id = Guid.NewGuid();
        }
    }
}
