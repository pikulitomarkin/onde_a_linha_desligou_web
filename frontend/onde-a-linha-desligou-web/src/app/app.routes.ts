import { Routes } from '@angular/router';
import { MenuPrincipalComponent } from './components/menu-principal/menu-principal';
import { MenuLinhasComponent } from './components/menu-linhas/menu-linhas';
import { BuscaKmComponent } from './components/busca-km/busca-km';
import { DetalhesTorreComponent } from './components/detalhes-torre/detalhes-torre';

export const routes: Routes = [
  { path: '', component: MenuPrincipalComponent },
  { path: 'linhas/:grupo', component: MenuLinhasComponent },
  { path: 'buscar', component: BuscaKmComponent },
  { path: 'detalhes', component: DetalhesTorreComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' } // Wildcard route for a 404-like redirect
];
