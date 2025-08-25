export interface Genero {
  id: number;
  nome: string;
  dataCriacao: Date;
}

export interface CriarGeneroDto {
  nome: string;
}

export interface AtualizarGeneroDto {
  id: number;
  nome: string;
}
