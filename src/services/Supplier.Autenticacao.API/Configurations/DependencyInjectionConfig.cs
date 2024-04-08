using Supplier.Business.Interfaces;
using Supplier.Business.Notifications;
using Supplier.Business.Services;
using Supplier.Data.Context;
using Supplier.Data.Repository;

namespace Supplier.Autenticacao.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            // Data
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Business
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<INotificador, Notificador>();

            return services;
        }
    }
}
