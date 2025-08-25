using Dominio.Entidades;
using Dominio.Excecoes;
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

        var excecao = Assert.ThrowsAsync<RecursoDuplicadoException>(
            async () => await _generoServico.CriarAsync(dto));

        Assert.That(excecao.Message, Is.EqualTo("Já existe gênero com nome 'Romance'"));
    }

    [Test]
    public async Task DeveObterGeneroPorIdExistente()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Terror");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        var resultado = await _generoServico.ObterPorIdAsync(id);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Id, Is.EqualTo(id));
        Assert.That(resultado.Nome, Is.EqualTo("Terror"));
    }

    [Test]
    public async Task DeveRetornarNuloParaGeneroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var resultado = await _generoServico.ObterPorIdAsync(idInexistente);

        Assert.That(resultado, Is.Null);
    }

    [Test]
    public async Task DeveAtualizarGeneroComDadosValidos()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Nome Original");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        var dto = new AtualizarGeneroDto { Nome = "Nome Atualizado" };

        var resultado = await _generoServico.AtualizarAsync(id, dto);

        Assert.That(resultado.Nome, Is.EqualTo("Nome Atualizado"));
    }

    [Test]
    public async Task DeveRejeitarAtualizacaoComNomeExistente()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var genero1 = new Genero(id1, "Primeiro Gênero");
        var genero2 = new Genero(id2, "Segundo Gênero");
        _contexto.Generos.AddRange(genero1, genero2);
        await _contexto.SaveChangesAsync();

        var dto = new AtualizarGeneroDto { Nome = "Primeiro Gênero" };

        var excecao = Assert.ThrowsAsync<RecursoDuplicadoException>(
            async () => await _generoServico.AtualizarAsync(id2, dto));

        Assert.That(excecao.Message, Is.EqualTo("Já existe gênero com nome 'Primeiro Gênero'"));
    }

    [Test]
    public async Task DeveRejeitarAtualizacaoDeGeneroInexistente()
    {
        var idInexistente = Guid.NewGuid();
        var dto = new AtualizarGeneroDto { Nome = "Nome Qualquer" };

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _generoServico.AtualizarAsync(idInexistente, dto));

        Assert.That(excecao.Message, Is.EqualTo("Gênero não encontrado"));
    }

    [Test]
    public async Task DeveRemoverGeneroSemLivrosAssociados()
    {
        var id = Guid.NewGuid();
        var genero = new Genero(id, "Gênero Para Remover");
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () => await _generoServico.RemoverAsync(id));

        var generoRemovido = await _contexto.Generos.FindAsync(id);
        Assert.That(generoRemovido, Is.Null);
    }

    [Test]
    public async Task DeveRejeitarRemocaoDeGeneroComLivrosAssociados()
    {
        var autorId = Guid.NewGuid();
        var generoId = Guid.NewGuid();
        var autor = new Autor(autorId, "Autor Teste");
        var genero = new Genero(generoId, "Gênero Com Livros");
        var livro = new Livro(Guid.NewGuid(), "Livro Teste", autorId, generoId);
        
        _contexto.Autores.Add(autor);
        _contexto.Generos.Add(genero);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        var excecao = Assert.ThrowsAsync<RegraDeNegocioException>(
            async () => await _generoServico.RemoverAsync(generoId));

        Assert.That(excecao.Message, Is.EqualTo("Não é possível remover um gênero que possui livros associados"));
    }

    [Test]
    public async Task DeveRejeitarRemocaoDeGeneroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _generoServico.RemoverAsync(idInexistente));

        Assert.That(excecao.Message, Is.EqualTo("Gênero não encontrado"));
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