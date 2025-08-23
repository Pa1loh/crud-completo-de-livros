using System.Net;
using System.Text.Json;

namespace ApiLivros.Middleware;

public class TratamentoGlobalErrosMiddleware
{
    private readonly RequestDelegate _proximo;
    private readonly ILogger<TratamentoGlobalErrosMiddleware> _logger;

    public TratamentoGlobalErrosMiddleware(RequestDelegate proximo, ILogger<TratamentoGlobalErrosMiddleware> logger)
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
        
        var resposta = new
        {
            mensagem = ObterMensagemErro(excecao),
            detalhes = ObterDetalhesErro(excecao)
        };

        contexto.Response.StatusCode = ObterCodigoStatus(excecao);
        
        var opcoes = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(resposta, opcoes);
        await contexto.Response.WriteAsync(json);
    }

    private static string ObterMensagemErro(Exception excecao)
    {
        return excecao switch
        {
            ArgumentException => excecao.Message,
            InvalidOperationException => excecao.Message,
            KeyNotFoundException => "Recurso não encontrado",
            _ => "Erro interno do servidor"
        };
    }

    private static string? ObterDetalhesErro(Exception excecao)
    {
        return excecao switch
        {
            ArgumentException => null,
            InvalidOperationException => null,
            KeyNotFoundException => null,
            _ => "Ocorreu um erro inesperado. Tente novamente mais tarde."
        };
    }

    private static int ObterCodigoStatus(Exception excecao)
    {
        return excecao switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}