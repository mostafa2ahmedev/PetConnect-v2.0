import { Component, ViewChild, ElementRef, AfterViewInit, OnDestroy, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';
import { AlertService } from '../../core/services/alert-service';
import { FormsModule } from '@angular/forms';
import { loadStripe, Stripe, StripeCardElement } from '@stripe/stripe-js';
import { Subscription } from 'rxjs';
import { CartService } from './cart-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

interface CartItem {
  id: number;
  name: string;
  description: string;
  price: number;
  quantity: number;
  maxQuantity?: number;
  imgUrl: string;
  brand?: string;
  category?: string;
}

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class CartComponent implements AfterViewInit, OnDestroy {
  @ViewChild('cardElement') cardElementRef!: ElementRef;
  @ViewChild('checkoutModal') checkoutModal!: TemplateRef<any>;
  cartItems: CartItem[] = [];
  server = 'https://localhost:7102';
  stripe: Stripe | null = null;
  cardElement: StripeCardElement | null = null;
  private cartSubscription!: Subscription;
  currentClientSecret: string | null = null;
  paymentProcessing = false;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private cartService: CartService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.cartSubscription = this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
    });
  }

  async ngAfterViewInit() {
    try {
      this.stripe = await loadStripe('your_publishable_key_here');
      if (this.stripe && this.cardElementRef?.nativeElement) {
        const elements = this.stripe.elements();
        this.cardElement = elements.create('card');
        this.cardElement.mount(this.cardElementRef.nativeElement);
      }
    } catch (error) {
      console.error('Stripe initialization error:', error);
      this.alertService.error('Payment system initialization failed');
    }
  }

  ngOnDestroy() {
    if (this.cardElement) {
      this.cardElement.unmount();
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  updateQuantity(index: number, quantity: number) {
    const newQuantity = Math.max(1, Math.min(quantity, this.cartItems[index].maxQuantity || 99));
    this.cartService.updateQuantity(index, newQuantity);
  }

  removeItem(index: number) {
    this.cartService.removeItem(index);
  }

  clearCart() {
    this.cartService.clearCart();
  }

  async proceedToCheckout() {
    const customerId = this.authService.getCustomerIdFromToken();
    if (!customerId) {
      this.alertService.error('Please login first!');
      return;
    }

    if (this.cartItems.length === 0) {
      this.alertService.error('Your cart is empty');
      return;
    }

    try {
      // 1. Save cart to Redis
      const basketId = `basket_${customerId}`;
      const basketDto = {
        id: basketId,
        items: this.cartItems.map(item => ({
          id: item.id,
          productName: item.name,
          price: item.price,
          quantity: item.quantity,
          pictureUrl: item.imgUrl
        }))
      };

      await this.http.post(`${this.server}/api/Basket`, basketDto).toPromise();

      // 2. Create payment intent
      const paymentResp = await this.http.post<{ clientSecret: string }>(
        `${this.server}/api/Payment/${basketId}`, 
        {}
      ).toPromise();

      if (!paymentResp?.clientSecret) {
        throw new Error('Failed to get payment client secret');
      }

      this.currentClientSecret = paymentResp.clientSecret;
      
      // 3. Open checkout modal
      this.modalService.open(this.checkoutModal, { 
        size: 'lg',
        backdrop: 'static' // Prevent closing by clicking outside
      });

    } catch (error: any) {
      console.error('Checkout preparation error:', error);
      this.alertService.error(error.error?.message || error.message || 'Checkout preparation failed');
    }
  }

  async confirmPayment() {
    if (!this.currentClientSecret || !this.stripe || !this.cardElement) {
      this.alertService.error('Payment system not ready');
      return;
    }

    this.paymentProcessing = true;

    try {
      const { error, paymentIntent } = await this.stripe.confirmCardPayment(
        this.currentClientSecret,
        {
          payment_method: {
            card: this.cardElement,
            billing_details: {
              name:  'Customer'
            }
          }
        }
      );

      if (error) {
        throw error;
      }

      if (paymentIntent?.status !== 'succeeded') {
        throw new Error('Payment not completed');
      }

      // Success - close modal and clear cart
      this.modalService.dismissAll();
      this.cartService.clearCart();
      this.alertService.success('Payment successful!');
      this.router.navigate(['/orders']);

    } catch (error: any) {
      console.error('Payment confirmation error:', error);
      this.alertService.error(error.message || 'Payment failed');
    } finally {
      this.paymentProcessing = false;
    }
  }
}