using Infra.Configuracao;
using Servico.Configuracao;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AdicionarInfraestrutura(builder.Configuration);
builder.Services.AdicionarServicos();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opcoes =>
{
    opcoes.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Livros",
        Version = "v1",
        Description = "Exemplo de crud completo com o contexto de livros",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Paulo",
            Email = "pcarmofaria@hotmail.com"
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opcoes =>
    {
        opcoes.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Livros v1");
        opcoes.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();