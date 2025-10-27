import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { LinhaService } from '../../linha.service';
import { Linha } from '../../models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-busca-km',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './busca-km.html',
  styleUrls: ['./busca-km.css']
})
export class BuscaKmComponent implements OnInit {
  linha: Linha | undefined;
  form: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private linhaService: LinhaService
  ) {
    this.form = this.fb.group({
      valorA: [''],
      valorB: ['']
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.linha = params as Linha;
    });
  }

  buscar() {
    if (this.linha) {
      const { valorA, valorB } = this.form.value;
      const numValorA = valorA ? parseFloat(valorA.replace(',', '.')) : undefined;
      const numValorB = valorB ? parseFloat(valorB.replace(',', '.')) : undefined;
      this.linhaService.buscarTorre(this.linha.chave, numValorA, numValorB)
        .subscribe(torre => {
          this.router.navigate(['/detalhes', { ...torre }]);
        });
    }
  }
}
