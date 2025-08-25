import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutorServico } from '../../servicos/autor.service';
import { Autor, CriarAutorDto, AtualizarAutorDto } from '../../modelos/autor.model';

@Component({
  selector: 'app-autores',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Gerenciamento de Autores</h2>
        <button class="btn btn-primary" (click)="abrirModalCriar()">
          Novo Autor
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
            <tr *ngFor="let autor of autores">
              <td>{{ autor.nome }}</td>
              <td>
                <div class="d-flex gap-2">
                  <button class="btn btn-secondary" (click)="editarAutor(autor)">
                    Editar
                  </button>
                  <button class="btn btn-danger" (click)="excluirAutor(autor.id)">
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
            <h3>{{ autorEdicao ? 'Editar Autor' : 'Novo Autor' }}</h3>
            <button class="btn-close" (click)="fecharModal()">&times;</button>
          </div>
          <form (ngSubmit)="salvarAutor()" #autorForm="ngForm">
            <div class="modal-body">
              <div class="form-group">
                <label for="nome">Nome*</label>
                <input
                  type="text"
                  id="nome"
                  [(ngModel)]="formularioAutor.nome"
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
                [disabled]="autorForm.invalid || salvando"
              >
                {{ salvando ? 'Salvando...' : 'Salvar' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `,
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
