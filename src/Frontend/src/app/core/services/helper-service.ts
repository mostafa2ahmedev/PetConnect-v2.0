import { Injectable,inject } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class HelperService {
  router = inject(Router)
  refreshPage(){
    this.router.navigate([this.router.url])
  }
}
