using ApiLivros.ViewModels;
using Servico.Dtos;

namespace ApiLivros.Extensions;

public static class LivroDtoExtensions
{
    public static LivroViewModel ParaViewModel(this LivroDto dto)
    {
        return new LivroViewModel
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            AutorId = dto.AutorId,
            NomeAutor = dto.NomeAutor,
            GeneroId = dto.GeneroId,
            NomeGenero = dto.NomeGenero,
            DataCriacao = dto.DataCriacao,
            DataAtualizacao = dto.DataAtualizacao
        };
    }
}

public static class AutorDtoExtensions
{
    public static AutorViewModel ParaViewModel(this AutorDto dto)
    {
        return new AutorViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            DataCriacao = dto.DataCriacao,
            DataAtualizacao = null
        };
    }
}

public static class GeneroDtoExtensions
{
    public static GeneroViewModel ParaViewModel(this GeneroDto dto)
    {
        return new GeneroViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            DataCriacao = dto.DataCriacao,
            DataAtualizacao = null
        };
    }
}