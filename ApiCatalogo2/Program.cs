using ApiCatalogo2.ApiEndPoints;
using ApiCatalogo2.AppServicesExtensions;
using ApiCatalogo2.Context;
using ApiCatalogo2.Models;
using ApiCatalogo2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Incluindo servi�os
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

//Definindo Endpoints
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

//Definir os endpoints(ExcludeFromDescription para ignorar no swagger)
//app.MapGet("/", () => "Cat�logo de Produtos - 2022").ExcludeFromDescription();

var enviroment = app.Environment;
app.UseExceptionHandling(enviroment)
    .UseSwaggerMiddleware()
    .UseAppCors();

//Ativar os servi�os de autentica��o e autoriza��o 
app.UseAuthentication();
app.UseAuthorization();

app.Run();
