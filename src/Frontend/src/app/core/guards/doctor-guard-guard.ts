import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account-service';

export const doctorGuardGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router)
  if(accountService.isDoctor() && accountService.isAuthenticated())
    return true;
  else if  (!accountService.isAuthenticated()) {
      router.navigate(['/login'], {
        queryParams: { returnUrl: state.url }
      });
    } 

  else {
      router.navigate(['/unauthorized']);
    }

    return false;
}
