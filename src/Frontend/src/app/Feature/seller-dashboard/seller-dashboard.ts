import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-seller-dashboard',
  standalone: true,      // ✅ لازم ده
  imports: [
    CommonModule,
    RouterModule         // ✅ ده عشان router-outlet يشتغل
  ],
  templateUrl: './seller-dashboard.html',  // ✅ اسم الملف يكون كده
  styleUrls: ['./seller-dashboard.css']    // ✅ اسم الملف يكون كده
})
export class SellerDashboardComponent {
  statistics = {
    totalProducts: 5
  };

  pendingOrders = [
    { id: 1, product: 'Cat Toy', qty: 2 },
    { id: 2, product: 'Dog Food', qty: 1 }
  ];

  loadingProfile = false;

  profileData = {
    fName: 'Samar',
    lName: 'Ali',
    email: 'samar@example.com',
    phoneNumber: '0123456789',
    street: '123 Street',
    city: 'Cairo'
  };

  sidebarVisible = true;

  toggleSidebar() {
    this.sidebarVisible = !this.sidebarVisible;
  }
}
