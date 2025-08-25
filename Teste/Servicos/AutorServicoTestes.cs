using Dominio.Entidades;
using Dominio.Excecoes;
using Infra.Contexto;
using Servico.Dtos;
using Servico.Servicos;
using Teste.Base;

namespace Teste.Servicos;

[TestFixture]
public class AutorServicoTestes : TesteComBancoMemoria
{
    private LivrosContexto _contexto;
    private AutorServico _autorServico;

    [SetUp]
    public void ConfigurarTeste()
    {
        _contexto = CriarContextoEmMemoria();
        _autorServico = new AutorServico(_contexto);
    }

    [TearDown]
    public void LimparTeste()
    {
        _contexto.Dispose();
    }

    [Test]
    public async Task DeveCriarAutorComDadosValidos()
    {
        var dto = new CriarAutorDto { Nome = "Machado de Assis" };

        var resultado = await _autorServico.CriarAsync(dto);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Nome, Is.EqualTo("Machado de Assis"));
        Assert.That(resultado.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(resultado.DataCriacao, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public async Task DeveRejeitarAutorComNomeExistente()
    {
        var autor = new Autor(Guid.NewGuid(), "Clarice Lispector");
        _contexto.Autores.Add(autor);
        await _contexto.SaveChangesAsync();

        var dto = new CriarAutorDto { Nome = "Clarice Lispector" };

        var excecao = Assert.ThrowsAsync<RecursoDuplicadoException>(
            async () => await _autorServico.CriarAsync(dto));

        Assert.That(excecao.Message, Is.EqualTo("Já existe autor com nome 'Clarice Lispector'"));
    }

    [Test]
    public async Task DeveObterAutorPorIdExistente()
    {
        var id = Guid.NewGuid();
        var autor = new Autor(id, "José Saramago");
        _contexto.Autores.Add(autor);
        await _contexto.SaveChangesAsync();

        var resultado = await _autorServico.ObterPorIdAsync(id);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Id, Is.EqualTo(id));
        Assert.That(resultado.Nome, Is.EqualTo("José Saramago"));
    }

    [Test]
    public async Task DeveRetornarNuloParaAutorInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var resultado = await _autorServico.ObterPorIdAsync(idInexistente);

        Assert.That(resultado, Is.Null);
    }

    [Test]
    public async Task DeveAtualizarAutorComDadosValidos()
    {
        var id = Guid.NewGuid();
        var autor = new Autor(id, "Nome Original");
        _contexto.Autores.Add(autor);
        await _contexto.SaveChangesAsync();

        var dto = new AtualizarAutorDto { Nome = "Nome Atualizado" };

        var resultado = await _autorServico.AtualizarAsync(id, dto);

        Assert.That(resultado.Nome, Is.EqualTo("Nome Atualizado"));
    }

    [Test]
    public async Task DeveRejeitarAtualizacaoComNomeExistente()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var autor1 = new Autor(id1, "Primeiro Autor");
        var autor2 = new Autor(id2, "Segundo Autor");
        _contexto.Autores.AddRange(autor1, autor2);
        await _contexto.SaveChangesAsync();

        var dto = new AtualizarAutorDto { Nome = "Primeiro Autor" };

        var excecao = Assert.ThrowsAsync<RecursoDuplicadoException>(
            async () => await _autorServico.AtualizarAsync(id2, dto));

        Assert.That(excecao.Message, Is.EqualTo("Já existe autor com nome 'Primeiro Autor'"));
    }

    [Test]
    public async Task DeveRejeitarAtualizacaoDeAutorInexistente()
    {
        var idInexistente = Guid.NewGuid();
        var dto = new AtualizarAutorDto { Nome = "Nome Qualquer" };

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _autorServico.AtualizarAsync(idInexistente, dto));

        Assert.That(excecao.Message, Is.EqualTo("Autor não encontrado"));
    }

    [Test]
    public async Task DeveRemoverAutorExistente()
    {
        var id = Guid.NewGuid();
        var autor = new Autor(id, "Autor Para Remover");
        _contexto.Autores.Add(autor);
        await _contexto.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () => await _autorServico.RemoverAsync(id));

        var autorRemovido = await _contexto.Autores.FindAsync(id);
        Assert.That(autorRemovido, Is.Null);
    }

    [Test]
    public async Task DeveRejeitarRemocaoDeAutorInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _autorServico.RemoverAsync(idInexistente));

        Assert.That(excecao.Message, Is.EqualTo("Autor não encontrado"));
    }

    [Test]
    public async Task DeveVerificarSeAutorExiste()
    {
        var id = Guid.NewGuid();
        var autor = new Autor(id, "Autor Existente");
        _contexto.Autores.Add(autor);
        await _contexto.SaveChangesAsync();

        var existe = await _autorServico.ExisteAsync(id);
        var naoExiste = await _autorServico.ExisteAsync(Guid.NewGuid());

        Assert.That(existe, Is.True);
        Assert.That(naoExiste, Is.False);
    }
}