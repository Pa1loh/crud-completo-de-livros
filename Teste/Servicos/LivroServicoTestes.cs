using Dominio.Entidades;
using Dominio.Excecoes;
using Infra.Contexto;
using Servico.Dtos;
using Servico.Servicos;
using Teste.Base;

namespace Teste.Servicos;

[TestFixture]
public class LivroServicoTestes : TesteComBancoMemoria
{
    private LivrosContexto _contexto;
    private LivroServico _livroServico;
    private Guid _autorId;
    private Guid _generoId;

    [SetUp]
    public async Task ConfigurarTeste()
    {
        _contexto = CriarContextoEmMemoria();
        _livroServico = new LivroServico(_contexto);
        
        _autorId = Guid.NewGuid();
        _generoId = Guid.NewGuid();
        
        var autor = new Autor(_autorId, "Teste Autor");
        var genero = new Genero(_generoId, "Teste Gênero");
        
        _contexto.Autores.Add(autor);
        _contexto.Generos.Add(genero);
        await _contexto.SaveChangesAsync();
    }

    [TearDown]
    public void LimparTeste()
    {
        _contexto.Dispose();
    }

    [Test]
    public async Task DeveCriarLivroComDadosValidos()
    {
        var dto = new CriarLivroDto 
        { 
            Titulo = "Dom Casmurro",
            AutorId = _autorId,
            GeneroId = _generoId
        };

        var resultado = await _livroServico.CriarAsync(dto);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Titulo, Is.EqualTo("Dom Casmurro"));
        Assert.That(resultado.AutorId, Is.EqualTo(_autorId));
        Assert.That(resultado.GeneroId, Is.EqualTo(_generoId));
        Assert.That(resultado.Id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task DeveRejeitarLivroComTituloExistente()
    {
        var livro = new Livro(Guid.NewGuid(), "1984", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        var dto = new CriarLivroDto 
        { 
            Titulo = "1984",
            AutorId = _autorId,
            GeneroId = _generoId
        };

        var excecao = Assert.ThrowsAsync<RecursoDuplicadoException>(
            async () => await _livroServico.CriarAsync(dto));

        Assert.That(excecao.Message, Is.EqualTo("Já existe livro com título '1984'"));
    }

    [Test]
    public async Task DeveRejeitarLivroComAutorInexistente()
    {
        var autorInexistente = Guid.NewGuid();
        var dto = new CriarLivroDto 
        { 
            Titulo = "Livro Teste",
            AutorId = autorInexistente,
            GeneroId = _generoId
        };

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _livroServico.CriarAsync(dto));

        Assert.That(excecao.Message, Is.EqualTo("Autor não encontrado"));
    }

    [Test]
    public async Task DeveRejeitarLivroComGeneroInexistente()
    {
        var generoInexistente = Guid.NewGuid();
        var dto = new CriarLivroDto 
        { 
            Titulo = "Livro Teste",
            AutorId = _autorId,
            GeneroId = generoInexistente
        };

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _livroServico.CriarAsync(dto));

        Assert.That(excecao.Message, Is.EqualTo("Gênero não encontrado"));
    }

    [Test]
    public async Task DeveObterLivroPorIdExistente()
    {
        var id = Guid.NewGuid();
        var livro = new Livro(id, "O Cortiço", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterPorIdAsync(id);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Id, Is.EqualTo(id));
        Assert.That(resultado.Titulo, Is.EqualTo("O Cortiço"));
        Assert.That(resultado.NomeAutor, Is.EqualTo("Teste Autor"));
        Assert.That(resultado.NomeGenero, Is.EqualTo("Teste Gênero"));
    }

    [Test]
    public async Task DeveRetornarNuloParaLivroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var resultado = await _livroServico.ObterPorIdAsync(idInexistente);

        Assert.That(resultado, Is.Null);
    }

    [Test]
    public async Task DeveAtualizarLivroComDadosValidos()
    {
        var id = Guid.NewGuid();
        var livro = new Livro(id, "Título Original", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        var dto = new AtualizarLivroDto 
        { 
            Titulo = "Título Atualizado",
            AutorId = _autorId,
            GeneroId = _generoId
        };

        var resultado = await _livroServico.AtualizarAsync(id, dto);

        Assert.That(resultado.Titulo, Is.EqualTo("Título Atualizado"));
    }

    [Test]
    public async Task DeveRejeitarAtualizacaoDeLivroInexistente()
    {
        var idInexistente = Guid.NewGuid();
        var dto = new AtualizarLivroDto 
        { 
            Titulo = "Qualquer Título",
            AutorId = _autorId,
            GeneroId = _generoId
        };

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _livroServico.AtualizarAsync(idInexistente, dto));

        Assert.That(excecao.Message, Is.EqualTo("Livro não encontrado"));
    }

    [Test]
    public async Task DeveRemoverLivroExistente()
    {
        var id = Guid.NewGuid();
        var livro = new Livro(id, "Livro Para Remover", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () => await _livroServico.RemoverAsync(id));

        var livroRemovido = await _contexto.Livros.FindAsync(id);
        Assert.That(livroRemovido, Is.Null);
    }

    [Test]
    public async Task DeveRejeitarRemocaoDeLivroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var excecao = Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            async () => await _livroServico.RemoverAsync(idInexistente));

        Assert.That(excecao.Message, Is.EqualTo("Livro não encontrado"));
    }

    [Test]
    public async Task DeveObterLivrosPorAutor()
    {
        var livro1 = new Livro(Guid.NewGuid(), "Livro 1", _autorId, _generoId);
        var livro2 = new Livro(Guid.NewGuid(), "Livro 2", _autorId, _generoId);
        _contexto.Livros.AddRange(livro1, livro2);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterPorAutorAsync(_autorId);

        var livros = resultado.ToArray();
        Assert.That(livros.Length, Is.EqualTo(2));
        Assert.That(livros.All(l => l.AutorId == _autorId), Is.True);
    }

    [Test]
    public async Task DeveObterLivrosPorGenero()
    {
        var livro1 = new Livro(Guid.NewGuid(), "Livro A", _autorId, _generoId);
        var livro2 = new Livro(Guid.NewGuid(), "Livro B", _autorId, _generoId);
        _contexto.Livros.AddRange(livro1, livro2);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterPorGeneroAsync(_generoId);

        var livros = resultado.ToArray();
        Assert.That(livros.Length, Is.EqualTo(2));
        Assert.That(livros.All(l => l.GeneroId == _generoId), Is.True);
    }

    [Test]
    public async Task DeveVerificarSeLivroExiste()
    {
        var id = Guid.NewGuid();
        var livro = new Livro(id, "Livro Existente", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        var existe = await _livroServico.ExisteAsync(id);
        var naoExiste = await _livroServico.ExisteAsync(Guid.NewGuid());

        Assert.That(existe, Is.True);
        Assert.That(naoExiste, Is.False);
    }
}