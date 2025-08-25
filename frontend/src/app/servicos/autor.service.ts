import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Autor, CriarAutorDto, AtualizarAutorDto } from '../modelos/autor.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutorServico {
  private readonly apiUrl = `${environment.apiUrl}/api/v1/autores`;

  constructor(private http: HttpClient) {}

  obterTodos(): Observable<Autor[]> {
    return this.http.get<Autor[]>(this.apiUrl);
  }

  obterPorId(id: number): Observable<Autor> {
    return this.http.get<Autor>(`${this.apiUrl}/${id}`);
  }

  criar(autor: CriarAutorDto): Observable<Autor> {
    return this.http.post<Autor>(this.apiUrl, autor);
  }

  atualizar(autor: AtualizarAutorDto): Observable<Autor> {
    return this.http.put<Autor>(`${this.apiUrl}/${autor.id}`, autor);
  }

  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
