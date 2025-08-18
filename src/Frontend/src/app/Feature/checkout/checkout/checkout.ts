import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  OnDestroy,
  AfterViewChecked,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { loadStripe, Stripe, StripeCardElement } from '@stripe/stripe-js';
import { ActivatedRoute, Router,RouterModule } from '@angular/router';
import { firstValueFrom, Subscription } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CommonModule } from '@angular/common';
import { CartItem, CartService } from '../../cart/cart-service';


interface BasketItem {
  id: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
}

interface CustomerBasket {
  id: string;
  items: BasketItem[];
  clientSecret: string;
  paymentIntentId: string;
  deliveryMethodId: number;
  shippingPrice: number;
}

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.html',
  styleUrls: ['./checkout.css'],
  imports: [CommonModule,RouterModule],
})
export class CheckoutComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('cardElement') cardElementRef!: ElementRef;

  // Stripe elements
  stripe: Stripe | null = null;
  cardElement: StripeCardElement | null = null;

  // Component state
  basket: CustomerBasket | null = null;
  loading = false;
  paymentProcessing = false;
  cardComplete = false;
  basketId: string;
  cartItems: CartItem[] = [];
  private cartSubscription!: Subscription;
  private cardMounted = false;

  deliveryMethods = [
    { id: 1, name: 'Express', cost: 10, days: '1-2 Business days' },
    { id: 2, name: 'Standard', cost: 5, days: '2-5 Business days' },
    { id: 3, name: 'Economy', cost: 2, days: '5-10 Business days' },
    { id: 4, name: 'Free', cost: 0, days: '1-2 Weeks' },
  ];

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService
  ) {
    this.basketId = this.route.snapshot.paramMap.get('basketId') || '';
  }
  async ngOnInit() {
    this.cartSubscription = this.cartService.cart$.subscribe((items) => {
      this.cartItems = items;
    });

    this.stripe = await loadStripe(environment.stripePublishableKey);

    // Initialize Stripe element only once
    if (
      this.stripe &&
      this.cardElementRef?.nativeElement &&
      !this.cardElement
    ) {
      const elements = this.stripe.elements();
      this.cardElement = elements.create('card', {
        style: {
          base: {
            color: '#32325d',
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: 'antialiased',
            fontSize: '16px',
            '::placeholder': {
              color: '#aab7c4',
            },
          },
          invalid: {
            color: '#fa755a',
            iconColor: '#fa755a',
          },
        },
      });

      this.cardElement.mount(this.cardElementRef.nativeElement);
      this.cardElement.on('change', (event) => {
        this.cardComplete = event.complete;
      });
    }
  }
  ngOnDestroy() {
    if (this.cardElement) {
      this.cardElement.unmount(); // Proper cleanup when component is destroyed
    }
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }
  ngAfterViewChecked() {
    if (this.basket?.clientSecret && !this.cardMounted) {
      this.cardMounted = true;
      this.setupCardElement();
    }
  }

  async selectDelivery(methodId: number) {
    const method = this.deliveryMethods.find((m) => m.id === methodId);

    if (!method) return;
    methodId = method.id;

    console.log('Selected delivery method:', method.id);

    try {
      this.loading = true;

      // 1. Update delivery method and create payment intent
      const response = await firstValueFrom(
        this.http.post<CustomerBasket>(
          `${environment.apiBaseUrl}/Payment/${this.basketId}?deliveryMethodId=${methodId}`,
          {
            deliveryMethodId: methodId,
          }
        )
      );



      // 2. Store the response
      this.basket = response;
      await this.setupCardElement();
      // 3. REUSE existing card element instead of recreating
      if (this.stripe && this.cardElement) {
        // Just update the complete state
        this.cardElement.on('change', (event) => {
          this.cardComplete = event.complete;
        });
      }
    } catch (error) {
      console.error('Payment setup failed:', error);
      alert('Failed to initialize payment. Please try again.');
    } finally {
      this.loading = false;
    }
  }
  async confirmPayment() {
    console.log('Confirm Payment clicked');
    console.log('Stripe available:', !!this.stripe);
    console.log('Card element available:', !!this.cardElement);
    console.log('Client secret available:', !!this.basket?.clientSecret);
    console.log('Card complete:', this.cardComplete);

    if (!this.stripe) {
      console.error('Stripe not loaded');
      return;
    }

    if (!this.cardElement) {
      console.error('Card element not mounted');
      return;
    }

    if (!this.basket?.clientSecret) {
      console.error('Client secret missing');
      return;
    }

    if (!this.stripe || !this.cardElement || !this.basket?.clientSecret) {
      alert('Payment system not ready');
      return;
    }

    this.paymentProcessing = true;

    try {
      const { error, paymentIntent } = await this.stripe.confirmCardPayment(
        this.basket.clientSecret,
        {
          payment_method: {
            card: this.cardElement,
          },
        }
      );

      if (error) {
        alert(`Payment failed: ${error.message}`);
      }
      if (paymentIntent?.status === 'succeeded') {
        // 2. Prepare order data for backend
        const orderData = {
          BasketId: this.basket.id,
          //paymentIntentId: this.basket.paymentIntentId,
          DeliveryMethodId: this.basket.deliveryMethodId,
        };

        // 3. Save order to database
        // const order = await firstValueFrom(
        //   this.http.post<any>(`${environment.apiBaseUrl}/Order/legacyCode`, orderData)
        // );

        const order = await firstValueFrom(
          this.http.post<any>(
            `${environment.apiBaseUrl}/Order/legacyCode`,
            orderData,
            {
              headers: {
                'Content-Type': 'application/json',
              },
            }
          )
        );

        console.log('Order created with status PaymentReceived:', order);

        // 4. Navigate to success page with order ID
        this.router.navigate(['/order-success', order.id]);
      }
    } catch (error) {
      console.error('Payment error:', error);
      alert('Payment processing failed');
    } finally {
      this.paymentProcessing = false;
    }
  }

  getSubtotal(): number {
    return this.cartItems.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
  }

  getShippingPrice(): number {
    // Use the selected delivery method's cost or default to 0
    if (this.basket?.deliveryMethodId) {
      const method = this.deliveryMethods.find(
        (m) => m.id === this.basket?.deliveryMethodId
      );
      return method?.cost || 0;
    }
    return 0;
  }

  getTotal(): number {
    return this.getSubtotal() + this.getShippingPrice();
  }
  get basketItems(): BasketItem[] {
    return this.basket?.items ?? [];
  }
  async setupCardElement() {
    if (!this.stripe || !this.cardElementRef?.nativeElement) return;

    // If a previous card exists, unmount it
    if (this.cardElement) {
      this.cardElement.unmount();
    }

    const elements = this.stripe.elements();
    this.cardElement = elements.create('card', {
      style: {
        base: {
          color: '#32325d',
          fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
          fontSmoothing: 'antialiased',
          fontSize: '16px',
          '::placeholder': { color: '#aab7c4' },
        },
        invalid: { color: '#fa755a', iconColor: '#fa755a' },
      },
    });

    this.cardElement.mount(this.cardElementRef.nativeElement);

    this.cardElement.on('change', (event) => {
      this.cardComplete = event.complete;
    });
  }
}
