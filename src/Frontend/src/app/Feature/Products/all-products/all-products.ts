import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './all-products.html',
  styleUrls: ['./all-products.css']
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
 server = "https://localhost:7102";

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7102/api/Product').subscribe({
      next: (res) => {
        this.products = res;
      },
      error: (err) => {
        console.error('Error loading products', err);
      }
    });
  }
}
