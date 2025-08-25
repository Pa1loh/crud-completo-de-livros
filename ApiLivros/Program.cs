using ApiLivros.Configuracao;
using ApiLivros.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AdicionarDependencias(builder.Configuration);

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

builder.Services.AddCors(opcoes =>
{
    opcoes.AddPolicy("PermitirTodos", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<TratamentoGlobalErros>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opcoes =>
    {
        opcoes.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Livros v1.0");
        opcoes.RoutePrefix = string.Empty;
        opcoes.DocumentTitle = "API de Livros - Documentação";
        opcoes.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseCors("PermitirTodos");
app.UseAuthorization();
app.MapControllers();

app.Run();