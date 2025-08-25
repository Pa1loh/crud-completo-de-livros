import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GeneroServico } from '../../servicos/genero.service';
import { Genero, CriarGeneroDto, AtualizarGeneroDto } from '../../modelos/genero.model';

@Component({
  selector: 'app-generos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './generos.component.html',
  styleUrl: './generos.component.scss'
})
export class GenerosComponent implements OnInit {
  generos: Genero[] = [];
  carregando = false;
  salvando = false;
  exibirModal = false;
  generoEdicao: Genero | null = null;

  formularioGenero: CriarGeneroDto = {
    nome: '',
  };

  constructor(private generoServico: GeneroServico) {}

  ngOnInit(): void {
    this.carregarGeneros();
  }

  abrirModalCriar(): void {
    this.generoEdicao = null;
    this.formularioGenero = {
      nome: '',
    };
    this.exibirModal = true;
  }

  editarGenero(genero: Genero): void {
    this.generoEdicao = genero;
    this.formularioGenero = {
      nome: genero.nome,
    };
    this.exibirModal = true;
  }

  async salvarGenero(): Promise<void> {
    this.salvando = true;
    
    try {
      if (this.generoEdicao) {
        const generoAtualizado: AtualizarGeneroDto = {
          id: this.generoEdicao.id,
          ...this.formularioGenero
        };
        await this.generoServico.atualizar(generoAtualizado).toPromise();
      } else {
        await this.generoServico.criar(this.formularioGenero).toPromise();
      }

      await this.carregarGeneros();
      this.fecharModal();
    } catch (erro) {
    } finally {
      this.salvando = false;
    }
  }

  async excluirGenero(id: number): Promise<void> {
    if (!confirm('Deseja realmente excluir este gênero?')) {
      return;
    }

    try {
      await this.generoServico.excluir(id).toPromise();
      await this.carregarGeneros();
    } catch (erro) {
    }
  }

  fecharModal(): void {
    this.exibirModal = false;
    this.generoEdicao = null;
  }

  private async carregarGeneros(): Promise<void> {
    this.carregando = true;
    
    try {
      this.generos = await this.generoServico.obterTodos().toPromise() || [];
    } catch (erro) {
    } finally {
      this.carregando = false;
    }
  }
}
