export interface Autor {
  id: number;
  nome: string;
  dataCriacao: Date;
}

export interface CriarAutorDto {
  nome: string;
}

export interface AtualizarAutorDto {
  id: number;
  nome: string;
}
