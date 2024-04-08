
namespace Supplier.Business.Models
{
    public class TransacaoClienteDTO : Entity
    {
        public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public decimal Valor { get; set; }
    }
}
