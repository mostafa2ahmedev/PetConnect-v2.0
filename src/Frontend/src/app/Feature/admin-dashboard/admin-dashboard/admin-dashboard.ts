import { Component, OnInit, TemplateRef } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from './admin-service';

import { CustomerPofileDetails } from '../../../models/customer-pofile-details';
import { Router, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet, RouterModule],
})
export class AdminDashboardComponent implements OnInit {
  currentImageUrl: string = '';
  profileData: CustomerPofileDetails | null = null; // Instead of a large ViewModel type
  loadingProfile = true;
  statistics: any;
  rejectionMessage: string = '';
  chartReady = false;
  sidebarVisible = false;

  constructor(
    private adminService: AdminService,
    private sanitizer: DomSanitizer,
    private router: Router
  ) {
    this.router.events.subscribe(() => {
      this.sidebarVisible = false;
    });
  }

  ngOnInit(): void {
    this.adminService.getAdminProfile().subscribe((data) => {
      console.log('Profile Data:', data);
      this.profileData = data;
      this.loadingProfile = false;
    });
  }

  getSafeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }

  toggleSidebar() {
    this.sidebarVisible = !this.sidebarVisible;
  }
}
