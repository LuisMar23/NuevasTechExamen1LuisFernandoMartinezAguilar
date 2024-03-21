using Examen1LuisMartinez.Contratos.Repositorios;
using Examen1LuisMartinez.Implementacion;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IProducto, ProductoRepositorio>();
        services.AddScoped<IProveedor, ProveedorRepositorio>();
    })
    .Build();

host.Run();
