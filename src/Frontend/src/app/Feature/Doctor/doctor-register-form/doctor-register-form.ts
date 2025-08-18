import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { GENDER_OPTIONS } from '../../../core/models/gender';
import { PET_SPECIALTY_OPTIONS } from '../../../core/models/pet-specialalty';
import { AccountService } from '../../../core/services/account-service';
import { RegisterationDataService } from '../../../core/services/registeration-data.service';
import { ComparePasswordValidation } from '../../../core/validators/custom-validators';

@Component({
  selector: 'app-doctor-register-form',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './doctor-register-form.html',
  styleUrls: ['./doctor-register-form.css'],
})
export class DoctorRegisterForm implements OnInit {
  registerForm: FormGroup;
  isSubmitted = false;
  generalErrors: string[] = [];

  genderOptions = GENDER_OPTIONS;
  petSpecialtyOptions = PET_SPECIALTY_OPTIONS;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private registrationDataService: RegisterationDataService
  ) {
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

  ngOnInit(): void {
    if (
      !this.registrationDataService.profileImage ||
      !this.registrationDataService.idCardImage ||
      !this.registrationDataService.certificateFile
    ) {
      console.error('Files are missing, redirecting back to upload page.');
      this.router.navigate(['/auth/face-compare']);
    }
  }

  // Getters for easy access in template
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
    this.generalErrors = [];

    if (this.registerForm.invalid) {
      return;
    }

    const profileImage = this.registrationDataService.profileImage;
    const idCardImage = this.registrationDataService.idCardImage;
    const certificateFile = this.registrationDataService.certificateFile;

    if (!profileImage || !idCardImage || !certificateFile) {
      this.generalErrors.push(
        'Required files are missing. Please go back and upload them again.'
      );
      this.router.navigate(['/register/doctor-verification']);
      return;
    }

    const formData = new FormData();

    // إضافة كل حقول الفورم النصية
    formData.append('FName', this.registerForm.get('fName')?.value);
    formData.append('LName', this.registerForm.get('lName')?.value);
    formData.append('Email', this.registerForm.get('email')?.value);
    formData.append('PhoneNumber', this.registerForm.get('phoneNumber')?.value);
    formData.append('Password', this.registerForm.get('password')?.value);
    formData.append(
      'ConfirmationPassword',
      this.registerForm.get('confirmationPassword')?.value
    );
    formData.append('Gender', this.registerForm.get('gender')?.value);
    formData.append(
      'PricePerHour',
      this.registerForm.get('pricePerHour')?.value
    );
    formData.append(
      'PetSpecialty',
      this.registerForm.get('petSpecialty')?.value
    );
    formData.append('Country', this.registerForm.get('country')?.value);
    formData.append('City', this.registerForm.get('city')?.value);
    formData.append('Street', this.registerForm.get('street')?.value);

    // إضافة الملفات
    formData.append('ProfileImage', profileImage, profileImage.name);
    formData.append('IdCardImage', idCardImage, idCardImage.name);
    formData.append('Certificate', certificateFile, certificateFile.name);

    // إرسال الطلب
    this.accountService.PostDoctorRegister(formData).subscribe({
      next: (res) => {
        if (res && res.success) {
          alert('Registration successful! Your application is under review.');
          this.registrationDataService.clearData();
          sessionStorage.removeItem('registration_step_1_complete');
          this.router.navigate(['/login']);
        } else {
          if (res && res.errors) {
            if (typeof res.errors === 'object' && !Array.isArray(res.errors)) {
              const errorMessages = [];
              for (const key in res.errors) {
                if (
                  res.errors.hasOwnProperty(key) &&
                  Array.isArray(res.errors[key])
                ) {
                  errorMessages.push(...res.errors[key]);
                }
              }
              this.generalErrors = errorMessages;
            } else {
              this.generalErrors = res.errors;
            }
          } else {
            this.generalErrors = [
              res?.message ?? 'An unknown registration error occurred.',
            ];
          }
        }
      },
      error: (err) => {
        console.error('Doctor registration failed', err);
        this.generalErrors = [
          'An unexpected error occurred. Please try again later.',
        ];
      },
    });
  }
}
