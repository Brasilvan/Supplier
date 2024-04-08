using AutoMapper;
using Supplier.Clientes.API.DTO;
using Supplier.Business.Models;

namespace Supplier.Clientes.API.Configurations
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Cliente, ClienteConsultaDTO>().ReverseMap();
            CreateMap<Cliente, ClienteCadastroDTO>().ReverseMap();
            CreateMap<TransacaoCliente, TransacaoClienteDTO>().ReverseMap();
        }
    }
}
