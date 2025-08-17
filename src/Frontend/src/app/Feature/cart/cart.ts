import {
  Component,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  TemplateRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';
import { Router, RouterModule } from '@angular/router';
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
  imports: [CommonModule, FormsModule,RouterModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css'],
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
  cardComplete = false; // Tracks if card input is fully filled
  cardError: string | null = null;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private cartService: CartService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.cartSubscription = this.cartService.cart$.subscribe((items) => {
      this.cartItems = items;
    });
  }

  async ngAfterViewInit() {
    try {
      this.stripe = await loadStripe(
        'pk_test_51RwNrCR4Cj7IY4aTDAUiEAZ6Dk3CZObpoSf1K85p8yPQkr9zVXtATZBETUA9jpcx90xTbPCbYuCCAAtjq8wAqMtu00CwanUpl3'
      );
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
    const newQuantity = Math.max(
      1,
      Math.min(quantity, this.cartItems[index].maxQuantity || 99)
    );
    this.cartService.updateQuantity(index, newQuantity);
  }

  removeItem(index: number) {
    this.cartService.removeItem(index);
  }

  clearCart() {
    this.cartService.clearCart();
  }

  getSubtotal(): number {
    return this.cartItems.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
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
      const basketId = `basket_${customerId}`;

      // Create payload that exactly matches backend DTOs
      const payload = {
        Id: basketId,
        Items: this.cartItems.map((item) => ({
          Id: item.id,
          ProductName: item.name,
          Price: item.price, // Ensure this is decimal compatible
          Quantity: item.quantity,
          PictureUrl: item.imgUrl,
          Brand: item.brand || null,
          Category: item.category || null,
        })),
        clientSecret: null,
        paymentIntentId: null,
        deliveryMethodId: null,
        shippingPrice: null,
      };

      console.log('Sending payload:', JSON.stringify(payload));

      // Add proper error handling
      const response = await this.http
        .post(`${this.server}/api/Basket`, payload, {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${this.authService.getToken()}`,
          },
        })
        .toPromise();

      console.log('Response:', response);
      this.router.navigate(['/checkout', basketId]);
    } catch (error: any) {
      console.error('Checkout error:', error);

      // Show detailed error message
      const errorMsg =
        error.error?.message ||
        error.message ||
        'Failed to proceed to checkout';
      this.alertService.error(errorMsg);
    }
  }
}
