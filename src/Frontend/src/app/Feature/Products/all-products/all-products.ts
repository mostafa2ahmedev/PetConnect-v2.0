import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router'; 
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth';

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

  server = "https://localhost:7102";

  
  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router        
  ) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7102/api/Product').subscribe({
      next: (res) => {
        this.products = res;
        this.filteredProducts = res;
      },
      error: (err) => {
        console.error('Error loading products', err);
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
      let cart = JSON.parse(localStorage.getItem('cart') || '[]');
      cart.push(product);
      localStorage.setItem('cart', JSON.stringify(cart));
      alert('Product added to cart!');
    }
  }
 
}
