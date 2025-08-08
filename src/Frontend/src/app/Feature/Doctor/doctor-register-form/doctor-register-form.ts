import { Component } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../../core/services/account-service';
import { Gender, GENDER_OPTIONS } from '../../../core/models/gender';
import { ComparePasswordValidation } from '../../../core/validators/custom-validators';
import { PET_SPECIALTY_OPTIONS } from '../../../core/models/pet-specialalty';

@Component({
  selector: 'app-doctor-register-form',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './doctor-register-form.html',
  styleUrl: './doctor-register-form.css',
})
export class DoctorRegisterForm {
  registerForm: FormGroup;
  isSubmitted = false;

  selectedImageFile: File | null = null;
  selectedCertificateFile: File | null = null;

  genderOptions = GENDER_OPTIONS;
  petSpecialtyOptions = PET_SPECIALTY_OPTIONS;

  generalErrors: string[] = [];

  certificateError = '';
  imageError = '';

  constructor(private accountService: AccountService, private router: Router) {
    this.registerForm = new FormGroup(
      {
        fName: new FormControl('', Validators.required),
        lName: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.required, Validators.email]),
        phoneNumber: new FormControl('', [
          Validators.required,
          Validators.pattern(/^[0-9]*$/),
        ]),
        password: new FormControl('', [
          Validators.required,
          Validators.pattern(
            /^(?=.*[a-zA-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]).{6,}$/
          ),
        ]),
        confirmationPassword: new FormControl('', Validators.required),
        gender: new FormControl('', Validators.required),
        pricePerHour: new FormControl('', [
          Validators.required,
          Validators.min(0),
        ]),
        petSpecialty: new FormControl('', Validators.required),
        country: new FormControl('', Validators.required),
        city: new FormControl('', Validators.required),
        street: new FormControl('', Validators.required),
      },
      {
        validators: [
          ComparePasswordValidation('password', 'confirmationPassword'),
        ],
      }
    );
  }

  // Getters for Cleaner HTML
  get fName() {
    return this.registerForm.get('fName');
  }
  get lName() {
    return this.registerForm.get('lName');
  }
  get email() {
    return this.registerForm.get('email');
  }
  get phoneNumber() {
    return this.registerForm.get('phoneNumber');
  }
  get password() {
    return this.registerForm.get('password');
  }
  get confirmationPassword() {
    return this.registerForm.get('confirmationPassword');
  }
  get gender() {
    return this.registerForm.get('gender');
  }
  get pricePerHour() {
    return this.registerForm.get('pricePerHour');
  }
  get petSpecialty() {
    return this.registerForm.get('petSpecialty');
  }
  get country() {
    return this.registerForm.get('country');
  }
  get city() {
    return this.registerForm.get('city');
  }
  get street() {
    return this.registerForm.get('street');
  }

  onSubmit() {
    this.isSubmitted = true;
    this.generalErrors = []; // Clear previous errors

    if (
      this.registerForm.invalid ||
      !this.selectedImageFile ||
      !this.selectedCertificateFile
    ) {
      if (!this.selectedImageFile) {
        this.imageError = 'Image Is Required';
      }
      if (!this.selectedCertificateFile) {
        this.certificateError = 'Certificate Is Required';
      }
      return;
    }

    const formData = new FormData();

    Object.keys(this.registerForm.controls).forEach((key) => {
      const value = this.registerForm.get(key)?.value;
      formData.append(key, value?.toString());
    });

    formData.append('Image', this.selectedImageFile);
    formData.append('Certificate', this.selectedCertificateFile);

    this.accountService.PostDoctorRegister(formData).subscribe({
      next: (res) => {
        if (res.success) {
          this.router.navigate(['/login']);
        } else {
          this.generalErrors = res.errors || ['Registration failed'];
        }
      },
      error: (err) => {
        console.error('Doctor registration failed', err);

        if (err.error?.errors) {
          // Handle array of errors from backend
          this.generalErrors = Array.isArray(err.error.errors)
            ? err.error.errors
            : [err.error.errors];
        } else if (err.message) {
          // Handle HTTP errors
          this.generalErrors = [err.message];
        } else {
          this.generalErrors = ['An unknown error occurred'];
        }
      },
    });
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (!file) {
      this.imageError = 'Image is required.';
      return;
    }

    // Example: validate file type and size
    const allowedTypes = ['image/png', 'image/jpeg'];
    if (!allowedTypes.includes(file.type)) {
      this.imageError = 'Only JPEG and PNG images are allowed.';
    } else if (file.size > 2 * 1024 * 1024) {
      // 2MB limit
      this.imageError = 'Image must be less than 2MB.';
    } else {
      this.imageError = '';
      // Save the file to a property for submission
      this.selectedImageFile = file;
    }
  }

  onCertificateSelected(event: any) {
    const file = event.target.files[0];
    if (!file) {
      this.certificateError = 'Certificate is required.';
      return;
    }

    const allowedTypes = ['application/pdf'];
    if (!allowedTypes.includes(file.type)) {
      this.certificateError = 'Only PDF files are allowed.';
    } else if (file.size > 5 * 1024 * 1024) {
      // 5MB limit
      this.certificateError = 'Certificate must be less than 5MB.';
    } else {
      this.certificateError = '';
      this.selectedCertificateFile = file;
    }
  }
}
