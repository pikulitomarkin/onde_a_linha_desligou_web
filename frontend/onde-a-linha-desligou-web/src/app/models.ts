export interface Linha {
  nome: string;
  chave: string;
  colunaA: string;
  colunaB: string;
  arquivoExcel: string;
  grupo: string;
}

export interface Torre {
  codigoOriginal: string;
  numeroParaExibicao: string;
  cidade: string;
  setor: string;
  latitude: number;
  longitude: number;
}
