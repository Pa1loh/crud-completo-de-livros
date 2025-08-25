import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LivroServico } from '../../servicos/livro.service';
import { AutorServico } from '../../servicos/autor.service';
import { GeneroServico } from '../../servicos/genero.service';
import { Livro, CriarLivroDto, AtualizarLivroDto } from '../../modelos/livro.model';
import { Autor } from '../../modelos/autor.model';
import { Genero } from '../../modelos/genero.model';

@Component({
  selector: 'app-livros',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.scss'
})
export class LivrosComponent implements OnInit {
  livros: Livro[] = [];
  autores: Autor[] = [];
  generos: Genero[] = [];
  carregando = false;
  erro: string | null = null;
  exibirModal = false;
  salvando = false;
  livroEdicao: Livro | null = null;

  formularioLivro: CriarLivroDto = {
    titulo: '',
    autorId: 0,
    generoId: 0
  };

  get anoAtual(): number {
    return new Date().getFullYear();
  }

  constructor(
    private livroServico: LivroServico,
    private autorServico: AutorServico,
    private generoServico: GeneroServico
  ) {}

  ngOnInit(): void {
    this.carregarDados();
  }

  rastrearLivro(indice: number, livro: Livro): number {
    return livro.id;
  }

  fecharModalSeClicouFora(evento: Event): void {
    if (evento.target === evento.currentTarget) {
      this.fecharModal();
    }
  }

  obterNomeAutor(autorId: number): string {
    const autor = this.autores.find(a => a.id === autorId);
    if (!autor) {
      return 'Autor não encontrado';
    }
    return autor.nome;
  }

  obterNomeGenero(generoId: number): string {
    const genero = this.generos.find(g => g.id === generoId);
    if (!genero) {
      return 'Gênero não encontrado';
    }
    return genero.nome;
  }

  abrirModalCriar(): void {
    this.livroEdicao = null;
    this.formularioLivro = {
      titulo: '',
      autorId: 0,
      generoId: 0
    };
    this.exibirModal = true;
  }

  editarLivro(livro: Livro): void {
    this.livroEdicao = livro;
    this.formularioLivro = {
      titulo: livro.titulo,
      autorId: livro.autorId,
      generoId: livro.generoId
    };
    this.exibirModal = true;
  }

  async salvarLivro(): Promise<void> {
    if (this.salvando) {
      return;
    }

    this.salvando = true;
    
    try {
      if (this.livroEdicao) {
        const livroAtualizado: AtualizarLivroDto = {
          id: this.livroEdicao.id,
          ...this.formularioLivro
        };
        await this.livroServico.atualizar(livroAtualizado).toPromise();
      } else {
        await this.livroServico.criar(this.formularioLivro).toPromise();
      }
      
      this.fecharModal();
      await this.carregarLivros();
    } catch (erro) {
      this.erro = 'Erro ao salvar livro. Tente novamente.';
    } finally {
      this.salvando = false;
    }
  }

  async excluirLivro(id: number): Promise<void> {
    if (!confirm('Deseja realmente excluir este livro?')) {
      return;
    }

    try {
      await this.livroServico.excluir(id).toPromise();
      await this.carregarLivros();
    } catch (erro) {
      this.erro = 'Erro ao excluir livro. Tente novamente.';
    }
  }

  fecharModal(): void {
    this.exibirModal = false;
    this.livroEdicao = null;
  }

  async carregarDados(): Promise<void> {
    this.carregando = true;
    this.erro = null;
    
    try {
      const [livros, autores, generos] = await Promise.all([
        this.livroServico.obterTodos().toPromise(),
        this.autorServico.obterTodos().toPromise(),
        this.generoServico.obterTodos().toPromise()
      ]);

      this.livros = livros || [];
      this.autores = autores || [];
      this.generos = generos || [];
    } catch (erro) {
      this.erro = 'Erro ao carregar dados. Tente novamente.';
    } finally {
      this.carregando = false;
    }
  }

  private async carregarLivros(): Promise<void> {
    try {
      this.livros = await this.livroServico.obterTodos().toPromise() || [];
    } catch (erro) {
      this.erro = 'Erro ao carregar livros. Tente novamente.';
    }
  }
}
