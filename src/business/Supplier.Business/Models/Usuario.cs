
using System.ComponentModel.DataAnnotations.Schema;

namespace Supplier.Business.Models
{
    public class Usuario : Entity
    {
        public Guid Id { get; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        
        [NotMapped]
        public string Senha { get; set; }

        public Usuario()
        {
            Id = Guid.NewGuid();
        }
    }
}
