
namespace Supplier.Business.Models
{
    public class TransacaoCliente : Entity
    {
        public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public decimal Valor { get; set; }
    }
}
