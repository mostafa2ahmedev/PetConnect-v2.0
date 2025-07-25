import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CustomerService } from '../customer-service';
import { UpdateCustomerProfileRequest } from '../../../models/update-customer-profile-request';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.html',
  imports: [CommonModule, FormsModule, RouterModule],

  styleUrl: './update-profile.css',
})
export class UpdateProfile implements OnInit {
  model: UpdateCustomerProfileRequest | null = null;
  selectedFile?: File;
  isSubmitting = false;
  imgUrl: string = '';
  constructor(private customerService: CustomerService) {}

  ngOnInit(): void {
    this.customerService.getCustomerProfile().subscribe({
      next: (res) => {
        // Map the response (CustomerPofileDetails) to UpdateCustomerProfileRequest
        this.model = {
          fName: res.fName,
          lName: res.lName,
          gender: 0, // default or fetch separately if not included
          street: '',
          city: res.city,
          country: '',
        };
        this.imgUrl = res.imgUrl || '';
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
    }
  }

  onSubmit(form: NgForm): void {
    if (form.invalid || !this.model) {
      return;
    }

    this.isSubmitting = true;

    this.customerService
      .updateCustomerProfile({
        fName: this.model.fName,
        lName: this.model.lName,
        gender: this.model.gender,
        street: this.model.street,
        city: this.model.city,
        country: this.model.country,
        imageFile: this.selectedFile,
      })
      .subscribe({
        next: () => {
          alert('Profile updated successfully.');
          this.isSubmitting = false;
        },
        error: () => {
          alert('Failed to update profile.');
          this.isSubmitting = false;
        },
      });
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
