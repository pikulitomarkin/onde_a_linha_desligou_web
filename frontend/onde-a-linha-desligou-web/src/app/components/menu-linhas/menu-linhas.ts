import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LinhaService } from '../../linha.service';
import { Linha } from '../../models';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-menu-linhas',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './menu-linhas.html',
  styleUrls: ['./menu-linhas.css']
})
export class MenuLinhasComponent implements OnInit {
  linhas$: Observable<Linha[]> | undefined;
  grupo: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private linhaService: LinhaService
  ) {}

  ngOnInit() {
    this.grupo = this.route.snapshot.paramMap.get('grupo');
    if (this.grupo) {
      this.linhas$ = this.linhaService.getLinhas(this.grupo);
    }
  }

  navigateToBusca(linha: Linha) {
    this.router.navigate(['/buscar', { ...linha }]);
  }
}
