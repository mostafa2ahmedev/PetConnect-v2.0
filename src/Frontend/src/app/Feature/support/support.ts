import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CreateSupportRequestDto } from './support-models';
import { CommonModule } from '@angular/common';
import { SupportServie } from './support-servie';
import { AlertService } from '../../core/services/alert-service';

@Component({
  selector: 'app-support',
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './support.html',
  styleUrl: './support.css',
})
export class Support {
  contactForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private supportService: SupportServie,
    private aletService: AlertService
  ) {
    this.contactForm = this.fb.group({
      phone: ['', Validators.required],
      type: [null, Validators.required],
      message: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.contactForm.valid) {
      // Map numeric type value to the actual text from the <option>
      const selectedTypeValue = this.contactForm.value.type;

      const request: CreateSupportRequestDto = {
        type: +selectedTypeValue, // send text instead of number
        message: this.contactForm.value.message,
      };

      console.log('Submitting request:', request);

      this.supportService.createSupportRequest(request).subscribe({
        next: (res) => {
          console.log('Success:', res);
          this.aletService.success('Sent Successfully');
        },
        error: (err) => {
          console.error('Error:', err);
          this.aletService.error('Something went wrong');
        },
      });
    }
  }
}
