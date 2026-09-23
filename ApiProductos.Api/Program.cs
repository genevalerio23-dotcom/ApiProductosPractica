using ApiProductos.Datos.Conexion;
using ApiProductos.Datos.Repositorios;
using ApiProductos.Negocio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string cadenaConexion =
    builder.Configuration.GetConnectionString("TiendaDB")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'TiendaDB'.");

builder.Services.AddScoped<IConexion>(
    _ => new ConexionSql(cadenaConexion));

builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MapeoProfile>();
});

var app = builder.Build();

app.UseMiddleware<
    ApiProductos.Api.Middlewares.ManejadorErroresMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();