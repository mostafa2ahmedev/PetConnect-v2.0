import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule,RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  selectedRole: string = '';


  constructor(private router: Router) {}

  navigateToForm() {
    if (this.selectedRole === 'doctor' || this.selectedRole === 'customer'|| this.selectedRole === 'seller') {
      this.router.navigate(['/register', this.selectedRole]);
    }
  }
}
