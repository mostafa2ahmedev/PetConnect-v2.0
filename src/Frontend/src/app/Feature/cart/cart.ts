import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';  
import { RouterModule } from '@angular/router';  
import { Router } from '@angular/router';       
import { AlertService } from '../../core/services/alert-service';
import { CartService } from './cart-service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule , RouterModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  server = 'https://localhost:7102';

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private alertService : AlertService,
    private cartService : CartService
  ) {}

  ngOnInit() {
    this.cartService.cart$.subscribe({next: items=>{
      this.cartItems = items;
    }})
    // if (this.cartService) {
    //   this.cartItems = JSON.parse(savedCart);
    // }
  }

  removeItem(index: number) {
    this.cartService.removeItem(index);
  }

  clearCart() {
    this.cartService.clearCart();
  }

  checkout() {
    const customerId = this.authService.getCustomerIdFromToken();
    if (!customerId) {
      this.alertService.error('Please login first!');
      return;
    }

    const order = {
      orderDate: new Date().toISOString(), 
      customerId: customerId,
      products: this.cartItems.map(item => ({
        orderId: 0,
        productId: item.id,
        quantity: 1, 
        unitPrice: item.price
      }))
    };

  this.http.post(`${this.server}/api/Order`, order).subscribe({next: (res) => {
      this.alertService.success('Order placed successfully:');
       this.clearCart();
      this.router.navigate(['/orders']); },
       error: (err) => {
      this.alertService.error('Error placing order')
      console.log(err);
       }
      });
  }
}
  
  
