import { Injectable } from '@angular/core';
import { 
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from '@angular/router';
import { AccountService } from '../services/account-service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private router: Router
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.accountService.isAuthenticated() && this.accountService.isAdmin()) {
      return true;
    }

    // If not authenticated, redirect to login
    if (!this.accountService.isAuthenticated()) {
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: state.url }
      });
    } 
    // If authenticated but not admin, redirect to unauthorized or home
    else {
      this.router.navigate(['/unauthorized']);
      // or this.router.navigate(['/home']);
    }

    return false;
  }
}