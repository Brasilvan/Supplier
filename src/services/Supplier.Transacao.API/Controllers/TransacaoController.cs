using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Business.Services;
using Supplier.Transacao.API.DTO;
using System.Net;

namespace Supplier.Transacao.API.Controllers
{
    [Route("api/transacao")]
    [ApiController]
    public class TransacaoController : MainController
    {
        private readonly ITransacaoService _transacaoService;
        private readonly IMapper _mapper;

        public TransacaoController(ITransacaoService transacaoService,
                                   IMapper mapper,
                                   INotificador notificador) : base(notificador)
        {
            _transacaoService = transacaoService;
            _mapper = mapper;
        }

        [HttpPost("autorizar")]
        public async Task<ActionResult<TransacaoCliente>> Autorizar(TransacaoClienteDTO transacaoClienteDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var transacao = await _transacaoService.Autorizar(_mapper.Map<TransacaoCliente>(transacaoClienteDTO));

            return CustomResponse(HttpStatusCode.Created, eApi.Transacao, transacaoClienteDTO, transacao.Id);
        }
    }
}
