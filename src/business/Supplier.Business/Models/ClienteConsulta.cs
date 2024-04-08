
namespace Supplier.Business.Models
{
    public class ClienteConsulta : Entity
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public decimal ValorLimite { get; set; }
    }
}
