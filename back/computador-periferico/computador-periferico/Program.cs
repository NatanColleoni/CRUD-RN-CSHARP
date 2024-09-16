using computador_periferico.Data;
using computador_periferico.Data.Entidades;
using computador_periferico.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ComputadorDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/computadores", async (ComputadorDbContext context) =>
{
    return await context.Computadores.Include(x => x.Perifericos).ToListAsync();
}).WithName("Consultar Computadores");

app.MapGet("/computadores/{id}", async (int id, ComputadorDbContext context) =>
{
    var computador = await context.Computadores.Where(x => x.Id == id).Include(x => x.Perifericos).FirstOrDefaultAsync();

    if (computador is null) return Results.NoContent();

    return Results.Ok(computador);
}).WithName("Consultar Computador");

app.MapPost("/computadores", async (CriarComputadorDto dto, ComputadorDbContext context) =>
{
    if(dto.Perifericos.Count <= 0)
    {
        return Results.BadRequest(new
        {
            Msg = "O computador precisa ter periféricos."
        });
    }

    var novoComputador = new Computador
    {
        Nome = dto.Nome,
        Cor = dto.Cor,
        DataFabricacao = dto.DataFabricacao,
    };

    foreach(var periferico in dto.Perifericos)
    {
        novoComputador.AddPeriferico(new Periferico
        {
            Nome = periferico.Nome
        });
    }

    await context.Computadores.AddAsync(novoComputador);
    await context.SaveChangesAsync();
    return Results.Ok(novoComputador);
}).WithName("Criar Computador");

app.MapPut("/computadores/{id}", async (int id, EditarComputadorDto dto,  ComputadorDbContext context) =>
{
    if (dto.Perifericos.Count <= 0)
    {
        return Results.BadRequest(new
        {
            Msg = "O computador precisa ter periféricos."
        });
    }

    var computador = await context.Computadores.Where(x => x.Id == id).Include(x => x.Perifericos).FirstOrDefaultAsync();
    
    if (computador is null) return Results.BadRequest(new
    {
        Msg = "Computador não encontrado."
    });

    computador.Perifericos = [];

    foreach (var periferico in dto.Perifericos)
    {
        computador.AddPeriferico(new Periferico
        {
            Nome = periferico.Nome
        });   
    }

    context.Computadores.Update(computador);
    await context.SaveChangesAsync();


    return Results.Ok(computador);
}).WithName("Editar Computador");

app.MapDelete("/computadores/{id}", async (int id, ComputadorDbContext context) =>
{
    var computador = await context.Computadores.Where(x => x.Id == id).FirstOrDefaultAsync();

    if (computador is null) return Results.BadRequest(new
    {
        Msg = "Computador não encontrado."
    });

    context.Computadores.Remove(computador);
    await context.SaveChangesAsync();


    return Results.Ok();
}).WithName("Deletar Computador");

app.Run();
