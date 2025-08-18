import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CreateSupportRequestDto } from './support-models';
import { SupportServie } from './support-servie';
import { AlertService } from '../../core/services/alert-service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './support.html',
  styleUrls: ['./support.css'],
})
export class Support {
  contactForm: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private supportService: SupportServie,
    private alertService: AlertService
  ) {
    this.contactForm = this.fb.group({
      type: ['', Validators.required], // service type
      message: ['', Validators.required], // message
      pictureUrl: [null], // file upload
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.contactForm.patchValue({ pictureUrl: this.selectedFile });
    }
  }

  onSubmit() {
    if (this.contactForm.valid) {
      const formValue = this.contactForm.value;

      const request: CreateSupportRequestDto = {
        type: Number(formValue.type),
        message: formValue.message,
        pictureUrl: this.selectedFile || undefined,
      };

      this.supportService.createSupportRequest(request).subscribe({
        next: (res) => {
          this.alertService.success('Sent Successfully');
          this.contactForm.reset();
          this.selectedFile = null;
        },
        error: () => this.alertService.error('Something went wrong'),
      });
    }
  }
}
