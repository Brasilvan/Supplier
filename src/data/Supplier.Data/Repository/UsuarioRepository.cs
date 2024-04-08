using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Data.Context;

namespace Supplier.Data.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(MeuDbContext context) : base(context)
        {
        }
    }
}
