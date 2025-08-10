// cart.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Product } from '../../models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartKey = 'cart';
  private cartSubject = new BehaviorSubject<Product[]>(this.getCartFromStorage());

  cart$ = this.cartSubject.asObservable();

  private getCartFromStorage(): Product[] {
    return JSON.parse(localStorage.getItem(this.cartKey) || '[]');
  }

  addToCart(product: Product): void {
    const cart = this.getCartFromStorage();
    if(cart.some(e=>e.id == product.id)) {
      cart.find(e=>e.id == product.id)!.quantity += 1;
    } else {
      cart.push({...product, quantity: 1 , maxQuantity: product.quantity}); // Ensure quantity is set
    }
    localStorage.setItem(this.cartKey, JSON.stringify(cart));
    this.cartSubject.next(cart); // Notify subscribers of the update
  }

  getCartLength(): number {
    return this.getCartFromStorage().length;
  }

  removeItem(index: number): void {
  const cart = this.getCartFromStorage();
  cart.splice(index, 1);
  localStorage.setItem(this.cartKey, JSON.stringify(cart));
  this.cartSubject.next(cart); // Broadcast updated cart
}

clearCart(): void {
  localStorage.removeItem(this.cartKey);
  this.cartSubject.next([]); // Notify all subscribers the cart is empty
}
}
