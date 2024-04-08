using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Supplier.Business.Interfaces;
using Supplier.Business.Notifications;
using System.Net;

namespace Supplier.Clientes.API.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, Enum? TipoOperacao = null, object? result = null, Guid? id = null)
        {
            if (TipoOperacao != null)
            {
                if (OperacaoValida())
                {
                    switch (TipoOperacao)
                    {
                        case eApi.ClienteConsulta:
                            return new ObjectResult(result)
                            {
                                StatusCode = Convert.ToInt32(statusCode)
                            };
                        case eApi.ClienteComando:
                            return Ok(new
                            {
                                idCliente = id,
                                status = $"{eStatus.OK.ToString()} ({Convert.ToInt32(statusCode)})"
                            });
                        case eApi.Transacao:
                            return Ok(new
                            {
                                status = $"{eStatus.APROVADO.ToString()} ({Convert.ToInt32(statusCode)})",
                                idTransacao = id
                            });
                        default:
                            return new ObjectResult(result)
                            {
                                StatusCode = Convert.ToInt32(statusCode)
                            };
                    }
                }
                else
                {
                    switch (TipoOperacao)
                    {
                        case eApi.Transacao:
                            return BadRequest(new
                            {
                                status = eStatus.NEGADO.ToString()
                            }); ;
                        default:
                            return BadRequest(new
                            {
                                status = eStatus.ERRO.ToString(),
                                detalheErro = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
                            });
                    }
                }

            }
            else
            {
                if (OperacaoValida())
                {
                    return new ObjectResult(result)
                    {
                        StatusCode = Convert.ToInt32(statusCode)
                    };
                }

                return BadRequest(new
                {
                    status = eStatus.ERRO.ToString(),
                    detalheErro = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
                });
            }
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!ModelState.IsValid)
                NotificarErroModelInvalida(modelState);

            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var ErrorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;

                NotificarErro(ErrorMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected enum eApi
        {
            ClienteConsulta,
            ClienteComando,
            Transacao
        }

        protected enum eStatus
        {
            OK,
            ERRO,
            APROVADO,
            NEGADO
        }
    }
}
