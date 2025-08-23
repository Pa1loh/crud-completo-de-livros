using Dominio.Entidades;
using Infra.Contexto;
using Servico.Dtos;
using Servico.Servicos;
using Teste.Base;

namespace Teste.Servicos;

[TestFixture]
public class GeneroServicoTestes : TesteComBancoMemoria
{
    private LivrosContexto _contexto;
    private GeneroServico _generoServico;

    [SetUp]
    public void ConfigurarTeste()
    {
        _contexto = CriarContextoEmMemoria();
        _generoServico = new GeneroServico(_contexto);
    }

    [TearDown]
    public void LimparTeste()
    {
        _contexto.Dispose();
    }

    [Test]
    public async Task DeveCriarGeneroComDadosValidos()
    {
        var dto = new CriarGeneroDto { Nome = "Ficção Científica" };

        var resultado = await _generoServico.CriarAsync(dto);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Nome, Is.EqualTo("Ficção Científica"));
        Assert.That(resultado.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(resultado.DataCriacao, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public async Task DeveRejeitarGeneroComNomeExistente()
    {
        var genero = new Genero(Guid.NewGuid(), "Romance");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        var dto = new CriarGeneroDto { Nome = "Romance" };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _generoServico.CriarAsync(dto));

        Assert.That(excecao.Message, Is.EqualTo("Já existe um gênero com este nome"));
    }

    [Test]
    public async Task DeveObterGeneroPorIdExistente()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Suspense");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        var resultado = await _generoServico.ObterPorIdAsync(id);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Id, Is.EqualTo(id));
        Assert.That(resultado.Nome, Is.EqualTo("Suspense"));
    }

    [Test]
    public async Task DeveRetornarNuloParaGeneroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var resultado = await _generoServico.ObterPorIdAsync(idInexistente);

        Assert.That(resultado, Is.Null);
    }

    [Test]
    public async Task DeveObterTodosOsGeneros()
    {
        var genero1 = new Genero(Guid.NewGuid(), "Terror");
        var genero2 = new Genero(Guid.NewGuid(), "Drama");
        _contexto.Generos.AddRange(genero1, genero2);
        await _contexto.SaveChangesAsync();

        var resultado = await _generoServico.ObterTodosAsync();

        Assert.That(resultado.Count(), Is.EqualTo(2));
        Assert.That(resultado.Any(g => g.Nome == "Terror"), Is.True);
        Assert.That(resultado.Any(g => g.Nome == "Drama"), Is.True);
    }

    [Test]
    public async Task DeveRetornarListaVaziaQuandoNaoHouverGeneros()
    {
        var resultado = await _generoServico.ObterTodosAsync();

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task DeveAtualizarGeneroExistente()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Nome Original");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        var dto = new AtualizarGeneroDto { Nome = "Nome Atualizado" };

        var resultado = await _generoServico.AtualizarAsync(id, dto);

        Assert.That(resultado.Nome, Is.EqualTo("Nome Atualizado"));
        Assert.That(resultado.Id, Is.EqualTo(id));

        var generoNoBanco = await _contexto.Generos.FindAsync(id);
        Assert.That(generoNoBanco, Is.Not.Null);
        Assert.That(generoNoBanco!.Nome, Is.EqualTo("Nome Atualizado"));
    }

    [Test]
    public void DeveRejeitarAtualizacaoParaGeneroInexistente()
    {
        var idInexistente = Guid.NewGuid();
        var dto = new AtualizarGeneroDto { Nome = "Nome Qualquer" };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _generoServico.AtualizarAsync(idInexistente, dto));

        Assert.That(excecao!.Message, Is.EqualTo("Gênero não encontrado"));
    }

    [Test]
    public void DeveRejeitarAtualizacaoComNomeJaExistente()
    {
        var genero1 = new Genero(Guid.NewGuid(), "Aventura");
        var genero2 = new Genero(Guid.NewGuid(), "Comédia");
        _contexto.Generos.AddRange(genero1, genero2);
        _contexto.SaveChanges();

        var dto = new AtualizarGeneroDto { Nome = "Aventura" };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _generoServico.AtualizarAsync(genero2.Id, dto));

        Assert.That(excecao!.Message, Is.EqualTo("Já existe um gênero com este nome"));
    }

    [Test]
    public async Task DeveRemoverGeneroExistente()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Gênero Para Remover");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        await _generoServico.RemoverAsync(id);

        var generoNoBanco = await _contexto.Generos.FindAsync(id);
        Assert.That(generoNoBanco, Is.Null);
    }

    [Test]
    public void DeveRejeitarRemocaoDeGeneroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _generoServico.RemoverAsync(idInexistente));

        Assert.That(excecao!.Message, Is.EqualTo("Gênero não encontrado"));
    }

    [Test]
    public async Task DeveVerificarSeGeneroExiste()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Gênero Existente");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        var existe = await _generoServico.ExisteAsync(id);
        var naoExiste = await _generoServico.ExisteAsync(Guid.NewGuid());

        Assert.That(existe, Is.True);
        Assert.That(naoExiste, Is.False);
    }
}