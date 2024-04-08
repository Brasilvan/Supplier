using Supplier.Business.Interfaces;
using Supplier.Business.Notifications;
using Supplier.Business.Services;
using Supplier.Business.Services.Handles;
using Supplier.Data.Context;
using Supplier.Data.Repository;

namespace Supplier.Transacao.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            // Data
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();

            // Business
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddHttpClient<ITransacaoService, TransacaoService>()
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services.AddScoped<INotificador, Notificador>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
