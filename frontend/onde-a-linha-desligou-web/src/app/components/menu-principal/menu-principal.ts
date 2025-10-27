import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-menu-principal',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './menu-principal.html',
  styleUrls: ['./menu-principal.css']
})
export class MenuPrincipalComponent {
  constructor(private router: Router) {}

  navigateTo(group: string) {
    if (group === 'sobre') {
      // Handle about navigation if necessary, for now just log
      console.log('Navigate to About page');
    } else {
      this.router.navigate(['/linhas', group]);
    }
  }
}
