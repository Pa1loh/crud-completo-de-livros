import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GeneroServico } from '../../servicos/genero.service';
import { Genero, CriarGeneroDto, AtualizarGeneroDto } from '../../modelos/genero.model';

@Component({
  selector: 'app-generos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Gerenciamento de Gêneros</h2>
        <button class="btn btn-primary" (click)="abrirModalCriar()">
          Novo Gênero
        </button>
      </div>

      <div class="card" *ngIf="carregando">
        <div class="loading"></div>
      </div>

      <div class="card" *ngIf="!carregando">
        <table class="table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let genero of generos">
              <td>{{ genero.nome }}</td>
              <td>
                <div class="d-flex gap-2">
                  <button class="btn btn-secondary" (click)="editarGenero(genero)">
                    Editar
                  </button>
                  <button class="btn btn-danger" (click)="excluirGenero(genero.id)">
                    Excluir
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="modal" *ngIf="exibirModal">
        <div class="modal-content">
          <div class="modal-header">
            <h3>{{ generoEdicao ? 'Editar Gênero' : 'Novo Gênero' }}</h3>
            <button class="btn-close" (click)="fecharModal()">&times;</button>
          </div>
          <form (ngSubmit)="salvarGenero()" #generoForm="ngForm">
            <div class="modal-body">
              <div class="form-group">
                <label for="nome">Nome*</label>
                <input
                  type="text"
                  id="nome"
                  [(ngModel)]="formularioGenero.nome"
                  name="nome"
                  required
                  #nome="ngModel"
                />
                <div *ngIf="nome.invalid && nome.touched" class="error">
                  Nome é obrigatório
                </div>
              </div>             
            </div>

            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" (click)="fecharModal()">
                Cancelar
              </button>
              <button 
                type="submit" 
                class="btn btn-primary"
                [disabled]="generoForm.invalid || salvando"
              >
                {{ salvando ? 'Salvando...' : 'Salvar' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `,
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
