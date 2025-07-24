import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../core/services/account-service';
import { AuthService } from '../../core/services/auth-service';

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  constructor(
    private accontService: AccountService,
    private router: Router,
    public authService: AuthService
  ) {}
  isAuthenticated(): boolean {
    return this.accontService.isAuthenticated();
  }
  logout(): void {
    this.accontService.logout();
    this.router.navigate(['/login']);
  }
}
