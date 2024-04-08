using AutoMapper;
using Supplier.Autenticacao.API.DTO;
using Supplier.Business.Models;

namespace Supplier.Autenticacao.API.Configurations
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
