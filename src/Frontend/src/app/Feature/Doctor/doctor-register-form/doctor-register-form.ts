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

  // ------- helpers -------
  private toStr(v: unknown): string {
    return (v ?? '').toString().trim();
  }

  /** Make a safe filename: slug(prefix)-timestamp.ext */
  private makeSafeFileName(prefix: string, originalName: string): string {
    const dot = originalName.lastIndexOf('.');
    const ext = dot >= 0 ? originalName.slice(dot + 1) : 'bin';
    const base = prefix
      .toLowerCase()
      .replace(/[^a-z0-9]+/gi, '-')
      .replace(/^-+|-+$/g, '');
    return `${base}-${Date.now()}.${ext.toLowerCase()}`;
  }

  /** Replace backslashes with forward slashes for web URLs */
  private normalizePath(p?: string): string | undefined {
    return p ? p.replace(/\\/g, '/') : p;
  }

  // Getters for template
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

    const profileImage = this.registrationDataService
      .profileImage as File | null;
    const idCardImage = this.registrationDataService.idCardImage as File | null;
    const certificateFile = this.registrationDataService
      .certificateFile as File | null;

    if (!profileImage || !idCardImage || !certificateFile) {
      this.generalErrors.push(
        'Required files are missing. Please go back and upload them again.'
      );
      this.router.navigate(['/register/doctor-verification']);
      return;
    }

    // Generate deterministic, URL-safe filenames (fixes bad backend paths)
    const namePrefix =
      `${this.toStr(this.fName?.value)}-${this.toStr(this.lName?.value)}` ||
      'doctor';

    const profileSafe = this.makeSafeFileName(
      `${namePrefix}-profile`,
      profileImage.name
    );
    const idCardSafe = this.makeSafeFileName(
      `${namePrefix}-idcard`,
      idCardImage.name
    );
    const certSafe = this.makeSafeFileName(
      `${namePrefix}-certificate`,
      certificateFile.name
    );

    const formData = new FormData();

    // Text fields (ensure string values)
    formData.append('FName', this.toStr(this.fName?.value));
    formData.append('LName', this.toStr(this.lName?.value));
    formData.append('Email', this.toStr(this.email?.value));
    formData.append('PhoneNumber', this.toStr(this.phoneNumber?.value));
    formData.append('Password', this.toStr(this.password?.value));
    formData.append(
      'ConfirmationPassword',
      this.toStr(this.confirmationPassword?.value)
    );
    formData.append('Gender', this.toStr(this.gender?.value)); // if backend expects number, still ok (stringified)
    formData.append('PricePerHour', this.toStr(this.pricePerHour?.value));
    formData.append('PetSpecialty', this.toStr(this.petSpecialty?.value));
    formData.append('Country', this.toStr(this.country?.value));
    formData.append('City', this.toStr(this.city?.value));
    formData.append('Street', this.toStr(this.street?.value));

    // Files (use sanitized filenames)
    formData.append('ProfileImage', profileImage, profileSafe);
    formData.append('IdCardImage', idCardImage, idCardSafe);
    formData.append('Certificate', certificateFile, certSafe);

    // Optional hints some APIs like to receive (harmless if ignored on server)
    formData.append('ProfileImageFileName', profileSafe);
    formData.append('IdCardImageFileName', idCardSafe);
    formData.append('CertificateFileName', certSafe);

    this.accountService.PostDoctorRegister(formData).subscribe({
      next: (res: any) => {
        // If API returns a path string, normalize slashes for the web (optional)
        if (res?.data?.profileImagePath) {
          res.data.profileImagePath = this.normalizePath(
            res.data.profileImagePath
          );
        }

        if (res && res.success) {
          alert('Registration successful! Your application is under review.');
          this.registrationDataService.clearData();
          sessionStorage.removeItem('registration_step_1_complete');
          this.router.navigate(['/login']);
        } else {
          if (res && res.errors) {
            if (typeof res.errors === 'object' && !Array.isArray(res.errors)) {
              const errorMessages: string[] = [];
              for (const key in res.errors) {
                if (
                  Object.prototype.hasOwnProperty.call(res.errors, key) &&
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
