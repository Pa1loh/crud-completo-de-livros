using ApiLivros.Configuracao;
using ApiLivros.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Livros", Version = "v1" });
});

builder.Services.AdicionarDependencias(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://frontend:4200",
                "http://livros-frontend:4200"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TratamentoGlobalErros>();
app.UseCors("PermitirFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();