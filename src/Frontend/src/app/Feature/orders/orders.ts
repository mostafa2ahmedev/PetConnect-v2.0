import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // DatePipe & CurrencyPipe موجودين فيه

import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule], 


  templateUrl: './orders.html',
  styleUrls: ['./orders.css']
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];
 server = "https://localhost:7102/assets/ProductImages";

currentPage: number = 1;
pageSize: number = 2;

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
deleteOrder(id: number) {
  const confirmDelete = window.confirm('Are you sure you want to delete this order?');
  if (confirmDelete) {
    this.http.delete(`https://localhost:7102/api/Order/${id}`).subscribe({
      next: () => {
        this.orders = this.orders.filter(order => order.id !== id);
      },
      error: (err) => {
        console.error('Error deleting order:', err);
      }
    });
  }
}
get pagedOrders() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.orders.slice(start, start + this.pageSize);
  }
 get totalPages(): number {
  return Math.max(1, Math.ceil(this.orders.length / this.pageSize));
}

  nextPage() {
    if (this.currentPage < Math.ceil(this.orders.length / this.pageSize)) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }


}
