import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CustomerService } from '../customer-service';
import { UpdateCustomerProfileRequest } from '../../../models/update-customer-profile-request';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CustomerPofileDetails } from '../../../models/customer-pofile-details';
import { AlertService } from '../../../core/services/alert-service';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.html',
  imports: [CommonModule, FormsModule, RouterModule],

  styleUrl: './update-profile.css',
})
export class UpdateProfile implements OnInit {
  model: CustomerPofileDetails | null = null;
  selectedFile: File | null = null;
  isSubmitting = false;
  imgUrl: string = '';
  constructor(
    private customerService: CustomerService,
    private alert: AlertService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.customerService.getCustomerProfile().subscribe({
      next: (res) => {
        // Map the response (CustomerPofileDetails) to UpdateCustomerProfileRequest

        this.model = res;
      },
      error: () => {
        console.error('Failed to load profile');
      },
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    } else {
      this.selectedFile = null;
    }
  }

  onSubmit(form: NgForm): void {
    if (form.invalid || !this.model) {
      return;
    }

    this.isSubmitting = true;

    this.customerService
      .updateCustomerProfile(this.model, this.selectedFile)
      .subscribe({
        next: () => {
          this.alert.success('Profile updated successfully.');
          this.isSubmitting = false;
          this.router.navigate(['/profile/', this.authService.getUserId()]);
        },
        error: () => {
          this.alert.error('Failed to update profile.');
          this.isSubmitting = false;
        },
      });
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
