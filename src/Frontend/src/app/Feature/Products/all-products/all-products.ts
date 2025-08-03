import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router'; 
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth';
import { AlertService } from '../../../core/services/alert-service';
import { CartService } from '../../cart/cart-service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './all-products.html',
  styleUrls: ['./all-products.css']
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  filteredProducts: any[] = [];
  filterType: string = "";
  filterExactPrice: number | null = null;
  loadingProducts: boolean = true; // Flag to show loading state
  server = "https://localhost:7102";

  
  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router   ,
    private alertService: AlertService    ,
    private changeDetectorRef: ChangeDetectorRef,
    private cartService : CartService
  ) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7102/api/Product').subscribe({
      next: (res) => {
        this.products = res;
        this.filteredProducts = res;
        this.loadingProducts=false;
      },
      error: (err) => {
        // console.error('Error loading products', err);
        this.alertService.error('Failed to load products. Please try again later.');
      }
    });
  }

  applyFilters() {
    this.filteredProducts = this.products.filter(product => {
      const matchesType = this.filterType === '' || 
        product.productTypeName.toLowerCase().includes(this.filterType.toLowerCase());
      const matchesExactPrice = this.filterExactPrice === null || 
        product.price === this.filterExactPrice;
      return matchesType && matchesExactPrice;
    });
  }

  clearFilters() {
    this.filterType = "";
    this.filterExactPrice = null;
    this.filteredProducts = [...this.products];
  }

  addToCart(product: any) {        
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
    } else {
      // let cart = JSON.parse(localStorage.getItem('cart') || '[]');
      // cart.push(product);
      // localStorage.setItem('cart', JSON.stringify(cart));
      this.cartService.addToCart(product);
      this.alertService.success('Product added to cart!');
      this.changeDetectorRef.detectChanges();
      // alert('Product added to cart!');
    }
  }
 
}
