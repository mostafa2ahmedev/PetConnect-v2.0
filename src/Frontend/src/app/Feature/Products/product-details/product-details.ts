import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.html',
  imports: [CommonModule],
})
export class ProductDetailsComponent implements OnInit {
  product: any;
  server = 'https://localhost:7102';

  constructor(
    private route: ActivatedRoute, 
    private http: HttpClient,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.http.get(`${this.server}/api/Product/${id}`).subscribe(
      (data) => {
        this.product = data;
      },
      (error) => {
        console.error('Error fetching product details:', error);
      }
    );
  }

  addToCart() {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
    } else {
      let cart = JSON.parse(localStorage.getItem('cart') || '[]');
      cart.push(this.product);
      localStorage.setItem('cart', JSON.stringify(cart));
      alert('Product added to cart!');
    }
  }
}
