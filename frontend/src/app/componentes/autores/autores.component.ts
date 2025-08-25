import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutorServico } from '../../servicos/autor.service';
import { Autor, CriarAutorDto, AtualizarAutorDto } from '../../modelos/autor.model';

@Component({
  selector: 'app-autores',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './autores.component.html',
  styleUrl: './autores.component.scss'
})
export class AutoresComponent implements OnInit {
  autores: Autor[] = [];
  carregando = false;
  salvando = false;
  exibirModal = false;
  autorEdicao: Autor | null = null;

  formularioAutor: CriarAutorDto = {
    nome: '',
  };

  constructor(private autorServico: AutorServico) {}

  ngOnInit(): void {
    this.carregarAutores();
  }

  abrirModalCriar(): void {
    this.autorEdicao = null;
    this.formularioAutor = {
      nome: '',
    };
    this.exibirModal = true;
  }

  editarAutor(autor: Autor): void {
    this.autorEdicao = autor;
    this.formularioAutor = {
      nome: autor.nome,
    };
    this.exibirModal = true;
  }

  async salvarAutor(): Promise<void> {
    this.salvando = true;
    
    try {
      if (this.autorEdicao) {
        const autorAtualizado: AtualizarAutorDto = {
          id: this.autorEdicao.id,
          ...this.formularioAutor
        };
        await this.autorServico.atualizar(autorAtualizado).toPromise();
      } else {
        await this.autorServico.criar(this.formularioAutor).toPromise();
      }

      await this.carregarAutores();
      this.fecharModal();
    } catch (erro) {
    } finally {
      this.salvando = false;
    }
  }

  async excluirAutor(id: number): Promise<void> {
    if (!confirm('Deseja realmente excluir este autor?')) {
      return;
    }

    try {
      await this.autorServico.excluir(id).toPromise();
      await this.carregarAutores();
    } catch (erro) {
    }
  }

  fecharModal(): void {
    this.exibirModal = false;
    this.autorEdicao = null;
  }

  private async carregarAutores(): Promise<void> {
    this.carregando = true;
    
    try {
      this.autores = await this.autorServico.obterTodos().toPromise() || [];
    } catch (erro) {
    } finally {
      this.carregando = false;
    }
  }
}
