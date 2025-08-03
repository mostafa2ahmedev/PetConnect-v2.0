import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class Login implements OnInit {
  Email: string = '';
  Password: string = '';
  remembered: boolean = false;
  errorMessage: string | null = null;

  constructor(public accountService: AccountService, private router: Router) {}
  ngOnInit(): void {
    this.accountService.logout();
  }

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

        // Optional: Store user details
        if (res.user) {
          storage.setItem('user', JSON.stringify(res.user));
        }

        // Navigate first, then reload so the new page sees the updated state
        let targetRoute = '/doctors';
        if (this.accountService.isAdmin()) {
          targetRoute = '/admin';
        } else if (this.accountService.isCustomer()) {
          targetRoute = '/profile';
        } else if (this.accountService.isDoctor()) {
          targetRoute = '/doc-profile';
        }

        this.router.navigate([targetRoute]).then(() => {
          window.location.reload();
        });
      },
      error: (err) => {
        console.error('Login failed', err);
        this.errorMessage =
          err.error?.message || 'Login failed. Please try again.';
      },
    });
  }
}
