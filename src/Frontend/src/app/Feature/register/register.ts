import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  selectedRole: string = '';

  constructor(private router: Router) {}

  navigateToForm(): void {
    if (!this.selectedRole) {
      alert('Please select a role first.');
      return;
    }

    if (this.selectedRole === 'doctor') {
      this.router.navigate(['/auth/face-compare']);
    } else if (this.selectedRole === 'customer') {
      this.router.navigate(['/register/customer']);
    } else if (this.selectedRole === 'seller') {
      this.router.navigate(['/register/seller']);
    }
  }
}
