import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Genero, CriarGeneroDto, AtualizarGeneroDto } from '../modelos/genero.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeneroServico {
  private readonly apiUrl = `${environment.apiUrl}/api/v1/generos`;

  constructor(private http: HttpClient) {}

  obterTodos(): Observable<Genero[]> {
    return this.http.get<Genero[]>(this.apiUrl);
  }

  obterPorId(id: number): Observable<Genero> {
    return this.http.get<Genero>(`${this.apiUrl}/${id}`);
  }

  criar(genero: CriarGeneroDto): Observable<Genero> {
    return this.http.post<Genero>(this.apiUrl, genero);
  }

  atualizar(genero: AtualizarGeneroDto): Observable<Genero> {
    return this.http.put<Genero>(`${this.apiUrl}/${genero.id}`, genero);
  }

  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
