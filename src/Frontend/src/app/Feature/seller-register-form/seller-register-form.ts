import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ComparePasswordValidation } from '../../core/validators/custom-validators';
import { AccountService } from '../../core/services/account-service';
import { Router } from '@angular/router';
import { GENDER_OPTIONS } from '../../core/models/gender';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-seller-register-form',
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './seller-register-form.html',
  styleUrl: './seller-register-form.css'
})
export class SellerRegisterForm {

   registerForm: FormGroup;
  isSubmitted: boolean = false;
  selectedImageFile: File | null = null;
  genderOptions = GENDER_OPTIONS;

  generalErrors: string[] = [];

    imageError = "";


  constructor(private accountService: AccountService, private router: Router) {
    this.registerForm = new FormGroup({
      fName: new FormControl('', Validators.required),
      lName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.pattern(/^[0-9]*$/),
      ]),
      password: new FormControl('', Validators.required),
      confirmationPassword: new FormControl('', Validators.required),
      gender: new FormControl("", Validators.required),
      imageUrl: new FormControl(''),
      country: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required),
      street: new FormControl('', Validators.required),
    },{validators : [ComparePasswordValidation('password','confirmationPassword')]}
  );
  }

  /*Getters for Cleaner HTML */
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

  get imageUrl() {
    return this.registerForm.get('imageUrl');
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

  passwordMatchValidator(group: FormGroup): { [key: string]: any } | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmationPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

onSubmit() {
  this.isSubmitted = true;
  this.generalErrors = []; // Clear previous errors

  if (this.registerForm.invalid || !this.selectedImageFile) 
    if (!this.selectedImageFile) {
        this.imageError = "Image Is Required";
            return;
      }

  const formData = new FormData();
  Object.keys(this.registerForm.controls).forEach((key) => {
    formData.append(key, this.registerForm.get(key)?.value?.toString());
  });
  formData.append('Image', this.selectedImageFile);

  this.accountService.PostSellerRegister(formData).subscribe({
    next: (res) => {
      if (res.success) {
        this.router.navigate(['/login']);
      } else {
        this.generalErrors = res.errors || ['Registration failed'];
      }
    },
    error: (err) => {
      console.error('Registration failed', err);
      
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
    }
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
  } else if (file.size > 2 * 1024 * 1024) { // 2MB limit
    this.imageError = 'Image must be less than 2MB.';
  } else {
    this.imageError = "";
    // Save the file to a property for submission
    this.selectedImageFile = file;
  }
}
}

