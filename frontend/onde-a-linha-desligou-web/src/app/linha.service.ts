import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Linha, Torre } from './models';

@Injectable({
  providedIn: 'root'
})
export class LinhaService {
  private apiUrl = '/api/Linhas'; // Assuming the backend is served from the same origin

  constructor(private http: HttpClient) { }

  getLinhas(grupo: string): Observable<Linha[]> {
    return this.http.get<Linha[]>(`${this.apiUrl}/${grupo}`);
  }

  buscarTorre(chave: string, valorA?: number, valorB?: number): Observable<Torre> {
    let params = new HttpParams().set('chave', chave);
    if (valorA !== undefined) {
      params = params.set('valorA', valorA.toString());
    }
    if (valorB !== undefined) {
      params = params.set('valorB', valorB.toString());
    }

    return this.http.get<Torre>(`${this.apiUrl}/buscar`, { params });
  }
}
