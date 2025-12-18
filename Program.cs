using ApiEmpresas.Data;
using Microsoft.EntityFrameworkCore;
using ApiEmpresas.Repositories.Interfaces;
using ApiEmpresas.Repositories.Implementations;
using ApiEmpresas.Services.Interfaces;
using ApiEmpresas.Services.Implementations;
using FluentValidation;
using FluentValidation.AspNetCore;
using ApiEmpresas.Errors;




var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// REGISTRAR O DbContext USANDO MySQL
// ------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// ------------------------------------------------------
// REPOSITORIES
// ------------------------------------------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // genérico

builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<ISetorRepository, SetorRepository>();
builder.Services.AddScoped<IProfissaoRepository, ProfissaoRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IHabilidadeRepository, HabilidadeRepository>();

// ------------------------------------------------------
// SERVICES
// ------------------------------------------------------
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<ISetorService, SetorService>();
builder.Services.AddScoped<IProfissaoService, ProfissaoService>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IHabilidadeService, HabilidadeService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MVC Controllers
builder.Services.AddControllers();

// registra a validação automática (server-side) e adapters client-side
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// registra todos os validators do assembly atual (ou troque o tipo para a classe de um validator específico)
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
