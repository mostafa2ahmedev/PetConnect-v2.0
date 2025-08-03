import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../core/services/account-service';
import { Product } from '../../models/product';
import { CartService } from '../cart/cart-service';

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit{
  cartLength: number = 0;
  constructor(private accontService: AccountService, private router: Router , private cartService:CartService) {}
  ngOnInit(): void {
          this.cartService.cart$.subscribe(cart => {
      this.cartLength = cart.length;
  })
  }
  isAuthenticated(): boolean {
    return this.accontService.isAuthenticated();
  }
  logout(): void {
    this.accontService.logout();
    this.router.navigate(['/login']);
  }
}
