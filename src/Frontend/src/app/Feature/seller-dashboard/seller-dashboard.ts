import { Component, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsComponent } from '../Products/products/products';
import { AccountService } from '../../core/services/account-service';
import { SellerDashboardService } from './seller-dashboard-service';

//import { OrdersComponent } from '../orders/orders.component';

@Component({
  selector: 'app-seller-dashboard',
  standalone: true,
  imports: [CommonModule, ProductsComponent],
  templateUrl: './seller-dashboard.html',
  styleUrls: ['./seller-dashboard.css']
})
export class SellerDashboardComponent implements OnInit{
  ngOnInit(): void {
    this.sellerDashService.getSellerData().subscribe({
      next: resp=>{
        console.log(resp);
        // this.sellerName= `${resp.data.fName} ${resp.data.lName}`;
        this.sellerName= `${resp.data.fName}`;
      }
    })


    this.sellerDashService.getSellerProducts().subscribe({
      next:resp=>{
        console.log(resp);
      }
    })
  }
  accountService = inject(AccountService)
  sellerDashService = inject(SellerDashboardService)
  sellerName = ""
  activeTab: 'products' | 'orders' = 'products';

  logout() {
    console.log('Logging out...');
    // Your logout logic
  }
}
