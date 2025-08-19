import {
  Component,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  TemplateRef,
  OnInit,
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
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css'],
})
export class CartComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('cardElement') cardElementRef!: ElementRef;
  @ViewChild('checkoutModal') checkoutModal!: TemplateRef<any>;
  
  // Cart properties
  cartItems: CartItem[] = [];
  server = 'https://localhost:7102';
  
  // Processing states
  isProcessing = false; // Added this property for the loading indicator
  paymentProcessing = false;
  
  // Stripe properties
  stripe: Stripe | null = null;
  cardElement: StripeCardElement | null = null;
  currentClientSecret: string | null = null;
  cardComplete = false; // Tracks if card input is fully filled
  cardError: string | null = null;
  
  // Subscription
  private cartSubscription!: Subscription;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private cartService: CartService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    // Subscribe to cart changes
    this.cartSubscription = this.cartService.cart$.subscribe((items) => {
      this.cartItems = items;
    });
  }

  async ngAfterViewInit() {
    try {
      // Initialize Stripe
      this.stripe = await loadStripe(
        'pk_test_51RwNrCR4Cj7IY4aTDAUiEAZ6Dk3CZObpoSf1K85p8yPQkr9zVXtATZBETUA9jpcx90xTbPCbYuCCAAtjq8wAqMtu00CwanUpl3'
      );
      
      if (this.stripe && this.cardElementRef?.nativeElement) {
        const elements = this.stripe.elements();
        this.cardElement = elements.create('card', {
          style: {
            base: {
              fontSize: '16px',
              color: '#424770',
              '::placeholder': {
                color: '#aab7c4',
              },
            },
            invalid: {
              color: '#9e2146',
            },
          },
        });
        
        this.cardElement.mount(this.cardElementRef.nativeElement);
        
        // Listen for card element changes
        this.cardElement.on('change', (event) => {
          this.cardComplete = event.complete;
          this.cardError = event.error ? event.error.message : null;
        });
      }
    } catch (error) {
      console.error('Stripe initialization error:', error);
      this.alertService.error('Payment system initialization failed');
    }
  }

  ngOnDestroy() {
    // Clean up Stripe card element
    if (this.cardElement) {
      this.cardElement.unmount();
    }
    
    // Unsubscribe from cart changes
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  /**
   * Update quantity of an item in the cart
   */
  updateQuantity(index: number, quantity: number) {
    if (index < 0 || index >= this.cartItems.length) {
      return;
    }
    
    const newQuantity = Math.max(
      1,
      Math.min(quantity, this.cartItems[index].maxQuantity || 99)
    );
    
    this.cartService.updateQuantity(index, newQuantity);
  }

  /**
   * Remove an item from the cart
   */
  removeItem(index: number) {
    if (index < 0 || index >= this.cartItems.length) {
      return;
    }
    
    this.cartService.removeItem(index);
    this.alertService.success('Item removed from cart');
  }

  /**
   * Clear all items from the cart
   */
  clearCart() {
    if (this.cartItems.length === 0) {
      return;
    }
    
    this.cartService.clearCart();
    this.alertService.success('Cart cleared successfully');
  }

  /**
   * Calculate subtotal of all items in cart
   */
  getSubtotal(): number {
    return this.cartItems.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
  }

  /**
   * Calculate tax (you can modify this logic as needed)
   */
  getTax(): number {
    return this.getSubtotal() * 0.08; // 8% tax rate
  }

  /**
   * Calculate shipping cost (you can modify this logic as needed)
   */
  getShipping(): number {
    return this.getSubtotal() > 50 ? 0 : 9.99; // Free shipping over $50
  }

  /**
   * Calculate total including tax and shipping
   */
  getTotal(): number {
    return this.getSubtotal() + this.getTax() + this.getShipping();
  }

  /**
   * Proceed to checkout - creates basket and navigates to checkout page
   */
  async proceedToCheckout() {
    // Validation checks
    const customerId = this.authService.getCustomerIdFromToken();
    if (!customerId) {
      this.alertService.error('Please login first!');
      this.router.navigate(['/login']);
      return;
    }

    if (this.cartItems.length === 0) {
      this.alertService.error('Your cart is empty');
      return;
    }

    // Set processing state
    this.isProcessing = true;

    try {
      const basketId = `basket_${customerId}_${Date.now()}`;

      // Create payload that matches backend DTOs
      const payload = {
        Id: basketId,
        Items: this.cartItems.map((item) => ({
          Id: item.id,
          ProductName: item.name,
          Price: Number(item.price.toFixed(2)), // Ensure proper decimal format
          Quantity: item.quantity,
          PictureUrl: item.imgUrl,
          Brand: item.brand || 'Unknown',
          Category: item.category || 'General',
        })),
        clientSecret: null,
        paymentIntentId: null,
        deliveryMethodId: 1, // Default delivery method
        shippingPrice: this.getShipping(),
      };

      console.log('Creating basket with payload:', JSON.stringify(payload, null, 2));

      // Make API call to create basket
      const response = await this.http
        .post(`${this.server}/api/Basket`, payload, {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${this.authService.getToken()}`,
          },
        })
        .toPromise();

      console.log('Basket created successfully:', response);
      
      // Show success message
      this.alertService.success('Proceeding to checkout...');
      
      // Navigate to checkout page
      this.router.navigate(['/checkout', basketId]);
      
    } catch (error: any) {
      console.error('Checkout error:', error);

      // Handle different types of errors
      let errorMessage = 'Failed to proceed to checkout';
      
      if (error.status === 401) {
        errorMessage = 'Please login again to continue';
        this.router.navigate(['/login']);
      } else if (error.status === 400) {
        errorMessage = error.error?.message || 'Invalid cart data';
      } else if (error.status === 500) {
        errorMessage = 'Server error. Please try again later';
      } else if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.message) {
        errorMessage = error.message;
      }

      this.alertService.error(errorMessage);
    } finally {
      // Reset processing state
      this.isProcessing = false;
    }
  }

  /**
   * Continue shopping - navigate back to products page
   */
  continueShopping() {
    this.router.navigate(['/all-products']);
  }

  /**
   * Apply coupon code (placeholder for future implementation)
   */
  applyCoupon(couponCode: string) {
    if (!couponCode.trim()) {
      this.alertService.error('Please enter a coupon code');
      return;
    }
    
    // TODO: Implement coupon logic
    console.log('Coupon feature coming soon!');
  }

  /**
   * Save cart for later (placeholder for future implementation)
   */
  saveForLater() {
    if (this.cartItems.length === 0) {
      this.alertService.error('Cart is empty');
      return;
    }
    
    // TODO: Implement save for later logic
    console.log('Save for later feature coming soon!');
  }

  /**
   * Get formatted price string
   */
  getFormattedPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(price);
  }

  /**
   * Check if cart has items
   */
  get hasItems(): boolean {
    return this.cartItems.length > 0;
  }

  /**
   * Get total items count
   */
  get totalItemsCount(): number {
    return this.cartItems.reduce((sum, item) => sum + item.quantity, 0);
  }

  /**
   * Check if item is at maximum quantity
   */
  isAtMaxQuantity(item: CartItem): boolean {
    return item.quantity >= (item.maxQuantity || 99);
  }

  /**
   * Check if item is at minimum quantity
   */
  isAtMinQuantity(item: CartItem): boolean {
    return item.quantity <= 1;
  }

  /**
   * Handle image load error
   */
  onImageError(event: any) {
    event.target.src = '/assets/images/placeholder-product.png';
  }
}