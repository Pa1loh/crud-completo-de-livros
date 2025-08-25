import { Autor } from './autor.model';
import { Genero } from './genero.model';

export interface Livro {
  id: number;
  titulo: string;
  autorId: number;
  generoId: number;
  autor?: Autor;
  genero?: Genero;
}

export interface CriarLivroDto {
  titulo: string;
  autorId: number;
  generoId: number;
}

export interface AtualizarLivroDto {
  id: number;
  titulo: string;
  autorId: number;
  generoId: number;
}
