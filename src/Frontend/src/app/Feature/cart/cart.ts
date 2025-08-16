import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { AlertService } from '../../core/services/alert-service';
import { CartService } from './cart-service';
import { FormsModule } from '@angular/forms';
import { loadStripe, Stripe } from '@stripe/stripe-js';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css'],
})
export class CartComponent implements OnInit, AfterViewInit {
  cartItems: any[] = [];
  server = 'https://localhost:7102';
  stripe!: Stripe | null;
  cardElement: any;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private cartService: CartService
  ) {}

  async ngOnInit() {
    // Load Stripe
    this.stripe = await loadStripe('your_stripe_publishable_key_here');

    // Subscribe to cart items
    this.cartService.cart$.subscribe((items) => {
      this.cartItems = items;
    });
  }

  ngAfterViewInit() {
    // Mount Stripe card element after view initialization
    if (this.stripe) {
      const elements = this.stripe.elements();
      this.cardElement = elements.create('card');
      this.cardElement.mount('#card-element');
    }
  }

  removeItem(index: number) {
    this.cartService.removeItem(index);
  }

  clearCart() {
    this.cartService.clearCart();
  }

  async checkout() {
    const customerId = this.authService.getCustomerIdFromToken();
    if (!customerId) {
      this.alertService.error('Please login first!');
      return;
    }

    // Prepare basket DTO
    const basketDto = {
      id: customerId, // or your basket ID logic
      items: this.cartItems.map((item) => ({
        id: item.id,
        productName: item.productName,
        quantity: item.quantity,
        price: item.price,
        pictureUrl: item.pictureUrl,
        brand: item.brand,
        category: item.category,
      })),
    };

    try {
      // Save basket to Redis
      await this.http.post(`${this.server}/api/basket`, basketDto).toPromise();

      // Create or update Stripe PaymentIntent
      const paymentResp: any = await this.http
        .post(`${this.server}/api/payment/${basketDto.id}`, {})
        .toPromise();

      const clientSecret = paymentResp.clientSecret;
      if (!clientSecret) throw new Error('Failed to get client secret');

      // Confirm payment with Stripe
      const { error, paymentIntent } = await this.stripe!.confirmCardPayment(
        clientSecret,
        {
          payment_method: {
            card: this.cardElement,
          },
        }
      );

      if (error) {
        this.alertService.error(error.message!);
        return;
      }

      // Payment successful: clear cart and navigate
      this.cartService.clearCart();
      this.alertService.success('Payment successful!');
      this.router.navigate(['/orders']);
    } catch (err: any) {
      console.error(err);
      this.alertService.error('Checkout failed!');
    }
  }
}


      // checkout() {
      //   const customerId = this.authService.getCustomerIdFromToken();
      //   if (!customerId) {
      //     this.alertService.error('Please login first!');
      //     return;
      //   }
    
      //   const order = {
      //     orderDate: new Date().toISOString(),
      //     customerId: customerId,
      //     products: this.cartItems.map(item => ({
      //       orderId: 0,
      //       productId: item.id,
      //       quantity: item.quantity,
      //       unitPrice: item.price,
      //     }))
      //   };
    
      // this.http.post(`${this.server}/api/Order`, order).subscribe({next: (res) => {
      //     this.alertService.success('Order placed successfully:');
      //      this.clearCart();
      //     this.router.navigate(['/orders']); },
      //      error: (err) => {
      //     this.alertService.error('Error placing order')
      //     console.log(err);
      //      }
      //     });
      // }