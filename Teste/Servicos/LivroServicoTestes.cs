using Dominio.Entidades;
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
        Assert.That(resultado.DataCriacao, Is.LessThanOrEqualTo(DateTime.UtcNow));
    }

    [Test]
    public void DeveRejeitarLivroComTituloExistente()
    {
        var livro = new Livro(Guid.NewGuid(), "O Cortiço", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        _contexto.SaveChanges();

        var dto = new CriarLivroDto 
        { 
            Titulo = "O Cortiço",
            AutorId = _autorId,
            GeneroId = _generoId
        };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _livroServico.CriarAsync(dto));

        Assert.That(excecao!.Message, Is.EqualTo("Já existe um livro com este título"));
    }

    [Test]
    public void DeveRejeitarLivroComAutorInexistente()
    {
        var autorInexistente = Guid.NewGuid();
        var dto = new CriarLivroDto 
        { 
            Titulo = "Livro Teste",
            AutorId = autorInexistente,
            GeneroId = _generoId
        };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _livroServico.CriarAsync(dto));

        Assert.That(excecao!.Message, Is.EqualTo("Autor não encontrado"));
    }

    [Test]
    public void DeveRejeitarLivroComGeneroInexistente()
    {
        var generoInexistente = Guid.NewGuid();
        var dto = new CriarLivroDto 
        { 
            Titulo = "Livro Teste",
            AutorId = _autorId,
            GeneroId = generoInexistente
        };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _livroServico.CriarAsync(dto));

        Assert.That(excecao!.Message, Is.EqualTo("Gênero não encontrado"));
    }

    [Test]
    public async Task DeveObterLivroPorIdExistente()
    {
        var id = Guid.NewGuid();
        var livro = new Livro(id, "A Moreninha", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterPorIdAsync(id);

        Assert.That(resultado, Is.Not.Null);
        Assert.That(resultado.Id, Is.EqualTo(id));
        Assert.That(resultado.Titulo, Is.EqualTo("A Moreninha"));
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
    public async Task DeveObterTodosOsLivros()
    {
        var livro1 = new Livro(Guid.NewGuid(), "Livro Um", _autorId, _generoId);
        var livro2 = new Livro(Guid.NewGuid(), "Livro Dois", _autorId, _generoId);
        _contexto.Livros.AddRange(livro1, livro2);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterTodosAsync();

        Assert.That(resultado.Count(), Is.EqualTo(2));
        Assert.That(resultado.Any(l => l.Titulo == "Livro Um"), Is.True);
        Assert.That(resultado.Any(l => l.Titulo == "Livro Dois"), Is.True);
    }

    [Test]
    public async Task DeveObterLivrosPorAutor()
    {
        var outroAutorId = Guid.NewGuid();
        var outroAutor = new Autor(outroAutorId, "Outro Autor");
        _contexto.Autores.Add(outroAutor);

        var livro1 = new Livro(Guid.NewGuid(), "Livro Autor 1", _autorId, _generoId);
        var livro2 = new Livro(Guid.NewGuid(), "Livro Autor 2", _autorId, _generoId);
        var livro3 = new Livro(Guid.NewGuid(), "Livro Outro Autor", outroAutorId, _generoId);
        
        _contexto.Livros.AddRange(livro1, livro2, livro3);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterPorAutorAsync(_autorId);

        Assert.That(resultado.Count(), Is.EqualTo(2));
        Assert.That(resultado.All(l => l.AutorId == _autorId), Is.True);
    }

    [Test]
    public async Task DeveObterLivrosPorGenero()
    {
        var outroGeneroId = Guid.NewGuid();
        var outroGenero = new Genero(outroGeneroId, "Outro Gênero");
        _contexto.Generos.Add(outroGenero);

        var livro1 = new Livro(Guid.NewGuid(), "Livro Gênero 1", _autorId, _generoId);
        var livro2 = new Livro(Guid.NewGuid(), "Livro Gênero 2", _autorId, _generoId);
        var livro3 = new Livro(Guid.NewGuid(), "Livro Outro Gênero", _autorId, outroGeneroId);
        
        _contexto.Livros.AddRange(livro1, livro2, livro3);
        await _contexto.SaveChangesAsync();

        var resultado = await _livroServico.ObterPorGeneroAsync(_generoId);

        Assert.That(resultado.Count(), Is.EqualTo(2));
        Assert.That(resultado.All(l => l.GeneroId == _generoId), Is.True);
    }

    [Test]
    public async Task DeveAtualizarLivroExistente()
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
        Assert.That(resultado.Id, Is.EqualTo(id));

        var livroNoBanco = await _contexto.Livros.FindAsync(id);
        Assert.That(livroNoBanco, Is.Not.Null);
        Assert.That(livroNoBanco!.Titulo, Is.EqualTo("Título Atualizado"));
    }

    [Test]
    public void DeveRejeitarAtualizacaoParaLivroInexistente()
    {
        var idInexistente = Guid.NewGuid();
        var dto = new AtualizarLivroDto 
        { 
            Titulo = "Título Qualquer",
            AutorId = _autorId,
            GeneroId = _generoId
        };

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _livroServico.AtualizarAsync(idInexistente, dto));

        Assert.That(excecao!.Message, Is.EqualTo("Livro não encontrado"));
    }

    [Test]
    public async Task DeveRemoverLivroExistente()
    {
        var id = Guid.NewGuid();
        var livro = new Livro(id, "Livro Para Remover", _autorId, _generoId);
        _contexto.Livros.Add(livro);
        await _contexto.SaveChangesAsync();

        await _livroServico.RemoverAsync(id);

        var livroNoBanco = await _contexto.Livros.FindAsync(id);
        Assert.That(livroNoBanco, Is.Null);
    }

    [Test]
    public void DeveRejeitarRemocaoDeLivroInexistente()
    {
        var idInexistente = Guid.NewGuid();

        var excecao = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _livroServico.RemoverAsync(idInexistente));

        Assert.That(excecao!.Message, Is.EqualTo("Livro não encontrado"));
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