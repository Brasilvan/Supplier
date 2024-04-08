using FluentValidation;
using FluentValidation.Results;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Business.Notifications;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace Supplier.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected string AppSettings(string value)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true);

            IConfiguration config = builder.Build();            

            return config.GetValue<string>(value);
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var item in validationResult.Errors)
            {
                Notificar(item.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecutaValidacao<TV, TE>(TV validacao, TE entidade)
            where TV : AbstractValidator<TE>
            where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }               
    }
}
