import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RegistrationGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    
    const step1Complete = sessionStorage.getItem('registration_step_1_complete');
    
    if (step1Complete === 'true') {
      return true; 
    } else {
      
      this.router.navigate(['/auth/face-compare']); 
      return false;
    }
  }
}
