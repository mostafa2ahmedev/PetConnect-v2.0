// cart.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface CartItem {
  id: number;
  name: string;
  productName?: string;  // Alias for backend compatibility
  description: string;
  price: number;
  quantity: number;
  maxQuantity?: number;
  imgUrl: string;
  pictureUrl?: string;   // Alias for backend compatibility
  brand?: string;
  category?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartKey = 'cart';
  private cartSubject = new BehaviorSubject<CartItem[]>(this.getCartFromStorage());

  cart$ = this.cartSubject.asObservable();

  private getCartFromStorage(): CartItem[] {
    const cart = localStorage.getItem(this.cartKey);
    return cart ? JSON.parse(cart) : [];
  }

  private saveCart(cart: CartItem[]): void {
    localStorage.setItem(this.cartKey, JSON.stringify(cart));
    this.cartSubject.next(cart);
  }

  addToCart(item: CartItem): void {
    const cart = this.getCartFromStorage();
    const existingItem = cart.find(e => e.id === item.id);
    
    if (existingItem) {
      existingItem.quantity += 1;
    } else {
      cart.push({ 
        ...item,
        quantity: 1,
        // Ensure backend-compatible fields
        productName: item.productName || item.name,
        pictureUrl: item.pictureUrl || item.imgUrl
      });
    }
    this.saveCart(cart);
  }

  updateQuantity(index: number, quantity: number): void {
    const cart = this.getCartFromStorage();
    if (cart[index]) {
      cart[index].quantity = quantity;
      this.saveCart(cart);
    }
  }

  removeItem(index: number): void {
    const cart = this.getCartFromStorage();
    cart.splice(index, 1);
    this.saveCart(cart);
  }

  clearCart(): void {
    localStorage.removeItem(this.cartKey);
    this.cartSubject.next([]);
  }

  getCartLength(): number {
    return this.getCartFromStorage().length;
  }
}