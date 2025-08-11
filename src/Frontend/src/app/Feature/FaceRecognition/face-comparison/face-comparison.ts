import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FaceComparisonService } from './../face-comparison.service';
import { RegisterationDataService } from '../../../core/services/registeration-data.service'; // تأكد من عدم وجود خطأ إملائي هنا

@Component({
  selector: 'app-face-comparison',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './face-comparison.html',
  styleUrls: ['./face-comparison.css'],
})
export class FaceComparisonComponent {
  image1Preview: SafeUrl | null = null; // ID Card
  image2Preview: SafeUrl | null = null; // Profile Image
  errorMessage: string | null = null;
  isLoading = false;

  idCardFile: File | null = null;
  profileImageFile: File | null = null;
  certificateFile: File | null = null;

  constructor(
    private faceComparisonService: FaceComparisonService,
    private registrationDataService: RegisterationDataService,
    private router: Router,
    private sanitizer: DomSanitizer
  ) {}

  onFileSelected(
    event: Event,
    type: 'idCard' | 'profile' | 'certificate'
  ): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (type === 'idCard') {
        this.idCardFile = file;
        this.previewImage(file, 1);
      } else if (type === 'profile') {
        this.profileImageFile = file;
        this.previewImage(file, 2);
      } else {
        this.certificateFile = file;
      }
      this.errorMessage = null;
    }
  }

  private previewImage(file: File, imageNumber: 1 | 2): void {
    const reader = new FileReader();
    reader.onload = () => {
      const base64 = reader.result as string;
      const safeUrl = this.sanitizer.bypassSecurityTrustUrl(base64);
      if (imageNumber === 1) this.image1Preview = safeUrl;
      else this.image2Preview = safeUrl;
    };
    reader.readAsDataURL(file);
  }

  verifyAndContinue(): void {
    if (!this.idCardFile || !this.profileImageFile || !this.certificateFile) {
      this.errorMessage =
        'Please upload all three required files: ID Card, Profile Image, and Certificate.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    this.faceComparisonService
      .compareFaces(this.idCardFile, this.profileImageFile)
      .subscribe({
        next: (result) => {
          this.isLoading = false;
          console.log('API Result:', result);

          if (result && result.facesMatch === true) {
            console.log(
              'Verification successful. Saving data and navigating...'
            );

            
            this.registrationDataService.idCardImage = this.idCardFile;
            this.registrationDataService.profileImage = this.profileImageFile;
            this.registrationDataService.certificateFile = this.certificateFile;

            sessionStorage.setItem('registration_step_1_complete', 'true');

            this.router.navigate(['/auth/doctor-register']);
          } else {
            console.error(
              'Verification failed. API response did not indicate a match.'
            );
            this.errorMessage =
              'Faces do not match or an unknown error occurred. Please try again.';
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage =
            'An error occurred during the request. Please check the backend server.';
          console.error('Comparison HTTP error:', err);
        },
      });
  }
}
