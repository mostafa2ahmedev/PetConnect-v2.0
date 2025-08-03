import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauth-component',
  imports: [],
  templateUrl: './unauth-component.html',
  styleUrl: './unauth-component.css',
})
export class UnauthComponent {
constructor(private router: Router) {}

  // Navigation function
  navigateToHome() {
    this.router.navigate(['/home']); // Programmatic navigation
  }
}
