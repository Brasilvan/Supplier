using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Supplier.Business.Interfaces;
using Supplier.Business.Notifications;
using Supplier.Business.Services;
using Supplier.Business.Services.Handles;
using Supplier.Clientes.API.RabbitConsumer;
using Supplier.Data.Context;
using Supplier.Data.Repository;

namespace Supplier.Clientes.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            // Data
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // Business
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<INotificador, Notificador>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();                        
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddHostedService<RabbitTransacaoCliente>();

            return services;
        }
    }
}
