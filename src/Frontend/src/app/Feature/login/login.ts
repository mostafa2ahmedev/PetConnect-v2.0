import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class Login {
  Email: string = '';
  Password: string = '';
  remembered: boolean = false;
  errorMessage: string | null = null;

  constructor(
    private accountService: AccountService,
    private router: Router,
    public authService: AuthService
  ) {}

  onLogin() {
    this.errorMessage = null;

    const loginData = {
      email: this.Email,
      password: this.Password,
      rememberMe: this.remembered,
    };

    this.accountService.PostLogin(loginData).subscribe({
      next: (res) => {
        // Store token (use sessionStorage if rememberMe is false)
        const storage = this.remembered ? localStorage : sessionStorage;
        storage.setItem('token', res.token);
        storage.setItem('userId', res.id);
        storage.setItem('userRoles', JSON.stringify(res.roles));

        // Optional: Store user details (if returned)
        if (res.user) {
          storage.setItem('user', JSON.stringify(res.user));
        }

        // Redirect to dashboard or another secure route
        if (this.authService.isAdmin()) {
          this.router.navigate(['/admin']);
        } else if (this.authService.isCustomer()) {
          this.router.navigate([`/profile`]);
        } else {
          this.router.navigate(['/doctors']);
        }
      },
      error: (err) => {
        console.error('Login failed', err);
        this.errorMessage =
          err.error?.message || 'Login failed. Please try again.';
      },
    });
  }
}
