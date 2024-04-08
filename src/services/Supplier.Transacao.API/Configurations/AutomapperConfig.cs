using AutoMapper;
using Supplier.Transacao.API.DTO;
using Supplier.Business.Models;

namespace Supplier.Transacao.API.Configurations
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<TransacaoCliente, TransacaoClienteDTO>().ReverseMap();
        }
    }
}
