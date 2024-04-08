using Microsoft.EntityFrameworkCore;
using Supplier.Clientes.API.Configurations;
using Supplier.Core.Identidade;
using Supplier.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();

builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ResolveDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
