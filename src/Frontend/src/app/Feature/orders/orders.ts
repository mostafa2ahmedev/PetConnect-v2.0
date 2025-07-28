import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // DatePipe & CurrencyPipe موجودين فيه

import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule], // 👈 كفاية استيراد CommonModule


  templateUrl: './orders.html',
  styleUrls: ['./orders.css']
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];
 server = "https://localhost:7102/assets/ProductImages";


  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit() {
    const customerId = this.authService.getCustomerIdFromToken();
    if (customerId) {
      this.http.get<any[]>(`https://localhost:7102/api/Order/customer/${customerId}`)
        .subscribe({
          next: (res) => {
            this.orders = res;
          },
          error: (err) => {
            console.error('Error fetching orders:', err);
          }
        });
    } else {
      console.warn('No customer ID found in token.');
    }
  }
}
