using computador_periferico.Data;
using computador_periferico.Data.Entidades;
using computador_periferico.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ComputadorDbContext>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

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

app.MapPost("/computadores", async ([FromBody] CriarComputadorDto dto, ComputadorDbContext context) =>
{
    if(dto.perifericos.Count <= 0)
    {
        return Results.BadRequest(new
        {
            Msg = "O computador precisa ter periféricos."
        });
    }

    var novoComputador = new Computador
    {
        Nome = dto.nome,
        Cor = dto.cor,
        DataFabricacao = dto.dataFabricacao,
    };

    foreach(var periferico in dto.perifericos)
    {
        novoComputador.AddPeriferico(new Periferico
        {
            Nome = periferico.nome
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
            Nome = periferico.nome
        });   
    }

    context.Computadores.Update(computador);
    await context.SaveChangesAsync();


    return Results.Ok(computador);
}).WithName("Editar Computador");

app.MapDelete("/computadores/{id}", async ([FromRoute] int id, ComputadorDbContext context) =>
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
