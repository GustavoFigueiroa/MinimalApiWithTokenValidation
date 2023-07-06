using ApiCatalogo2.Context;
using ApiCatalogo2.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.//ConfigureServices
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefautConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();
//Definir os endpoints(ExcludeFromDescription para ignorar no swagger)
app.MapGet("/", () => "Cat�logo de Produtos - 2022").ExcludeFromDescription();

//Adiciona uma categoria
app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

//Mostra todas categorias
app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync());

//Mostra uma categorias por ID
app.MapGet("/categorias{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id) is Categoria categoria
        ? Results.Ok(categoria)
        : Results.NotFound();
});

//Atualizar uma categoria
app.MapPut("/categorias{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
{
    if (categoria.CategoriaId != id)
    {
        return Results.BadRequest();
    }

    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.BadRequest();

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

//Deletar uma categoria
app.MapDelete("/categorias{id:int}", async (int id, AppDbContext db) =>
{

    var categoria = await db.Categorias.FindAsync(id);
    if (categoria is null)
    {
        return Results.NotFound();
    }

    db.Categorias.Remove(categoria);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Configure the HTTP request pipeline.//Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
