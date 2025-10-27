import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Torre } from '../../models';
import { CommonModule } from '@angular/common';
import { Location } from '@angular/common';

@Component({
  selector: 'app-detalhes-torre',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './detalhes-torre.html',
  styleUrls: ['./detalhes-torre.css']
})
export class DetalhesTorreComponent implements OnInit {
  torre: Torre | undefined;

  constructor(private route: ActivatedRoute, private router: Router, private location: Location) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.torre = {
        ...params,
        latitude: parseFloat(params['latitude']),
        longitude: parseFloat(params['longitude'])
      } as Torre;
    });
  }

  verNoMapa() {
    if (this.torre) {
      const url = `https://www.google.com/maps?q=${this.torre.latitude},${this.torre.longitude}`;
      window.open(url, '_blank');
    }
  }

  goBack(): void {
    this.location.back();
  }
}
