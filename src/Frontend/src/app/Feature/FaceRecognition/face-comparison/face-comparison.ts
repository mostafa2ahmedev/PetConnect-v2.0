import { Component, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FaceComparisonService } from './../face-comparison.service';
import { RegisterationDataService } from '../../../core/services/registeration-data.service'; // تأكد من صحة هذا المسار

@Component({
  selector: 'app-face-comparison',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './face-comparison.html',
  styleUrls: ['./face-comparison.css'],
})
export class FaceComparisonComponent implements OnDestroy {
  // --- المتغيرات الحالية ---
  image1Preview: SafeUrl | null = null; // ID Card
  image2Preview: SafeUrl | null = null; // Profile Image
  errorMessage: string | null = null;
  isLoading = false;
  idCardFile: File | null = null;
  profileImageFile: File | null = null;
  certificateFile: File | null = null;

  // --- متغيرات جديدة للكاميرا ---
  @ViewChild('videoElement') videoElement?: ElementRef<HTMLVideoElement>;
  isCameraOpen = false;
  private stream?: MediaStream;
  private activeCameraFor: 'idCard' | 'profile' | null = null;

  constructor(
    private faceComparisonService: FaceComparisonService,
    private registrationDataService: RegisterationDataService,
    private router: Router,
    private sanitizer: DomSanitizer
  ) {}

  // --- دالة موحدة للتعامل مع الملفات ---
  private handleFile(
    file: File,
    type: 'idCard' | 'profile' | 'certificate'
  ): void {
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

  // --- دالة استقبال الملفات من حقل الإدخال ---
  onFileSelected(
    event: Event,
    type: 'idCard' | 'profile' | 'certificate'
  ): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.handleFile(file, type);
    }
  }

  // --- دالة عرض الصورة المصغرة ---
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

  // --- دوال الكاميرا ---
  async openCamera(forInput: 'idCard' | 'profile'): Promise<void> {
    if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
      this.errorMessage = 'Sorry, your browser does not support camera access.';
      return;
    }
    try {
      this.activeCameraFor = forInput;
      this.isCameraOpen = true;
      this.stream = await navigator.mediaDevices.getUserMedia({
        video: { facingMode: { ideal: 'environment' } },
      });
      if (this.videoElement) {
        this.videoElement.nativeElement.srcObject = this.stream;
      }
    } catch (err) {
      this.errorMessage =
        'Could not access the camera. Please ensure you have granted permission.';
      console.error('Camera access error:', err);
      this.isCameraOpen = false;
    }
  }

  captureImage(): void {
    if (!this.videoElement || !this.stream || !this.activeCameraFor) return;
    const video = this.videoElement.nativeElement;
    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const context = canvas.getContext('2d');
    context?.drawImage(video, 0, 0, canvas.width, canvas.height);
    canvas.toBlob((blob) => {
      if (blob) {
        const fileName = `${this.activeCameraFor}_${new Date().getTime()}.jpg`;
        const imageFile = new File([blob], fileName, { type: 'image/jpeg' });
        this.handleFile(imageFile, this.activeCameraFor!);
      }
      this.closeCamera();
    }, 'image/jpeg');
  }

  closeCamera(): void {
    this.isCameraOpen = false;
    this.activeCameraFor = null;
    this.stream?.getTracks().forEach((track) => track.stop());
  }

  ngOnDestroy(): void {
    this.closeCamera();
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
          if (result && result.facesMatch === true) {
            this.registrationDataService.idCardImage = this.idCardFile;
            this.registrationDataService.profileImage = this.profileImageFile;
            this.registrationDataService.certificateFile = this.certificateFile;
            sessionStorage.setItem('registration_step_1_complete', 'true');
            this.router.navigate(['/auth/doctor-register']);
          } else {
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
