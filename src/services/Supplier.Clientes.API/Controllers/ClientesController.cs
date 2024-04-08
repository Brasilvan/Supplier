using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Clientes.API.DTO;
using System.Net;

namespace Supplier.Clientes.API.Controllers
{
    [Authorize]
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private const string CLIENTES_CACHE_KEY = "clientesCache";

        public ClientesController(IClienteRepository clienteRepository,
                                  IClienteService clienteService,
                                  IMapper mapper,
                                  IMemoryCache memoryCache,
                                  INotificador notificador) : base(notificador)
        {
            _clienteRepository = clienteRepository;
            _clienteService = clienteService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
                
        [HttpGet("obter-todos")]
        public async Task<IEnumerable<ClienteConsultaDTO>> ObterTodos()
        {
            if (_memoryCache.TryGetValue(CLIENTES_CACHE_KEY, out IEnumerable<ClienteConsultaDTO> clientes))
            {
                return clientes;
            }

            clientes = _mapper.Map<IEnumerable<ClienteConsultaDTO>>(await _clienteRepository.ObterTodos());

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            _memoryCache.Set(CLIENTES_CACHE_KEY, clientes, memoryCacheEntryOptions);

            return clientes;
        }

        [HttpGet("obter-por-id/{id:guid}")]
        public async Task<ActionResult<ClienteConsultaDTO>> ObterPorId(Guid id)
        {
            var cliente = _mapper.Map<ClienteConsultaDTO>(await _clienteRepository.ObterPorId(id));

            if (cliente == null) return NotFound();

            return cliente;
        }

        [HttpPost("adicionar")]
        public async Task<ActionResult<ClienteCadastroDTO>> Adicionar(ClienteCadastroDTO clienteCadastroDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);            

            var cliente = await _clienteService.Adicionar(_mapper.Map<Cliente>(clienteCadastroDTO));            

            return CustomResponse(HttpStatusCode.Created, eApi.ClienteComando, clienteCadastroDTO, cliente.Id);
        }
        
    }
}
