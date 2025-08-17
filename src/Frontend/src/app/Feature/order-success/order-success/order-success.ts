import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-success',
  templateUrl: './order-success.html',
  imports: [CommonModule,RouterModule],
})
export class OrderSuccessComponent implements OnInit {
  order: any;
  orderId!: number;
  loading = true;

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    this.orderId = +this.route.snapshot.paramMap.get('id')!;
    this.fetchOrder();
  }

  fetchOrder() {
    this.http.get<any>(`${environment.apiBaseUrl}/order/legacycode/${this.orderId}`)
      .subscribe({
        next: (res) => {
          this.order = res;
          this.loading = false;
        },
        error: (err) => {
          console.error('Error fetching order:', err);
          this.loading = false;
        }
      });
  }
}
