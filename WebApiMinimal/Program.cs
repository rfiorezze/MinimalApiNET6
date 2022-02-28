using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiMinimal.Contexto;
using WebApiMinimal.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(
    "Data Source=mssql-server,1433;Initial Catalog=BD_MINIMAL_API;User Id=sa;Password=numsey#2022"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiProdutos v1", Description = "Teste com Minimal APIs", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiProdutos v1");
    c.RoutePrefix = string.Empty;
});

app.MapPost("AdicionaProduto", async (Produto produto, Contexto contexto) =>
{
    contexto.Produto.Add(produto);
    await contexto.SaveChangesAsync();
});

app.MapPost("ExcluirProduto/{id}", async (int id, Contexto contexto) =>
{
    var produtoExcluir = await contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);
    if (produtoExcluir != null)
    {
        contexto.Produto.Remove(produtoExcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapPost("ListarProdutos", async (Contexto contexto) =>
{
    return await contexto.Produto.ToListAsync();
});

app.MapPost("ObterProduto", async (int id, Contexto contexto) =>
{
    await contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);
});

app.Run();
