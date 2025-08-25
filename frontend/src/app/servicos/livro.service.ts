import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Livro, CriarLivroDto, AtualizarLivroDto } from '../modelos/livro.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LivroServico {
  private readonly apiUrl = `${environment.apiUrl}/api/v1/livros`;

  constructor(private http: HttpClient) {}

  obterTodos(): Observable<Livro[]> {
    return this.http.get<Livro[]>(this.apiUrl);
  }

  obterPorId(id: number): Observable<Livro> {
    return this.http.get<Livro>(`${this.apiUrl}/${id}`);
  }

  criar(livro: CriarLivroDto): Observable<Livro> {
    return this.http.post<Livro>(this.apiUrl, livro);
  }

  atualizar(livro: AtualizarLivroDto): Observable<Livro> {
    return this.http.put<Livro>(`${this.apiUrl}/${livro.id}`, livro);
  }

  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
