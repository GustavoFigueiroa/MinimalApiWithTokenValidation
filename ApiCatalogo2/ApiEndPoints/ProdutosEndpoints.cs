using ApiCatalogo2.Context;
using ApiCatalogo2.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo2.ApiEndPoints
{
    public static class ProdutosEndpoints
    {
        public static void MapProdutosEndpoints(this WebApplication app)
        {
            //adiciona um produto
            app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
            {
                db.Add(produto);
                await db.SaveChangesAsync();

                return Results.Created($"/produtos/{produto.ProdutoId}", produto);
            });

            //Mostra todos produtos
            app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync()).RequireAuthorization();

            //Mostra um produto por ID
            app.MapGet("/produtos{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Produtos.FindAsync(id) is Produto produto
                    ? Results.Ok(produto)
                    : Results.NotFound();
            });

            //Atualizar um produto
            app.MapPut("/produtos{id:int}", async (int id, Produto produto, AppDbContext db) =>
            {
                if (produto.ProdutoId != id)
                {
                    return Results.BadRequest();
                }

                var produtoDB = await db.Produtos.FindAsync(id);

                if (produtoDB is null) return Results.BadRequest();

                produtoDB.Nome = produto.Nome;
                produtoDB.Descricao = produto.Descricao;
                produtoDB.Imagem = produto.Imagem;
                produtoDB.CategoriaId = produto.CategoriaId;
                produtoDB.DataCompra = produto.DataCompra;
                produtoDB.Preco = produto.Preco;
                produtoDB.Estoque = produto.Estoque;

                await db.SaveChangesAsync();
                return Results.Ok(produtoDB);
            });

            //Deleta um produto
            app.MapDelete("/produtos{id:int}", async (int id, AppDbContext db) =>
            {

                var produto = await db.Produtos.FindAsync(id);
                if (produto is null)
                {
                    return Results.NotFound();
                }

                db.Produtos.Remove(produto);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
