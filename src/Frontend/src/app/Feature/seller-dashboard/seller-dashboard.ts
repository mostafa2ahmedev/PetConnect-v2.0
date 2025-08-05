import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsComponent } from '../Products/products/products';

//import { OrdersComponent } from '../orders/orders.component';

@Component({
  selector: 'app-seller-dashboard',
  standalone: true,
  imports: [CommonModule, ProductsComponent],
  templateUrl: './seller-dashboard.html',
  styleUrls: ['./seller-dashboard.css']
})
export class SellerDashboardComponent {
  sellerName = 'Samar';
  activeTab: 'products' | 'orders' = 'products';

  logout() {
    console.log('Logging out...');
    // Your logout logic
  }
}
