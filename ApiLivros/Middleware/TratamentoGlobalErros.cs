using System.Net;
using System.Text.Json;
using Dominio.Excecoes;

namespace ApiLivros.Middleware;

public class TratamentoGlobalErros
{
    private readonly RequestDelegate _proximo;
    private readonly ILogger<TratamentoGlobalErros> _logger;

    public TratamentoGlobalErros(RequestDelegate proximo, ILogger<TratamentoGlobalErros> logger)
    {
        _proximo = proximo;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _proximo(contexto);
        }
        catch (Exception excecao)
        {
            _logger.LogError(excecao, "Erro não tratado: {Mensagem}", excecao.Message);
            await TratarExcecaoAsync(contexto, excecao);
        }
    }

    private static async Task TratarExcecaoAsync(HttpContext contexto, Exception excecao)
    {
        contexto.Response.ContentType = "application/json";
        
        var resposta = new { mensagem = excecao.Message };
        contexto.Response.StatusCode = ObterCodigoStatus(excecao);
        
        var opcoes = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(resposta, opcoes);
        await contexto.Response.WriteAsync(json);
    }

    private static int ObterCodigoStatus(Exception excecao)
    {
        return excecao switch
        {
            RecursoNaoEncontradoException => (int)HttpStatusCode.NotFound,
            RecursoDuplicadoException => (int)HttpStatusCode.Conflict,
            RegraDeNegocioException => (int)HttpStatusCode.BadRequest,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}