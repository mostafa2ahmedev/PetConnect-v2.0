import { Component, OnInit } from '@angular/core';
import {
  CreateSupportResponseDto,
  SupportRequest,
  SupportResponse,
  UserSupportRequestDetails,
} from '../support-models';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { SupportServie } from '../support-servie';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ticket-details',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './ticket-details.html',
  styleUrl: './ticket-details.css',
})
export class TicketDetails implements OnInit {
  ticketId!: number;
  ticketDetails!: UserSupportRequestDetails;
  loading = false;

  replyForm!: FormGroup;
  submitting = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private supportService: SupportServie
  ) {}

  ngOnInit(): void {
    this.ticketId = Number(this.route.snapshot.paramMap.get('id'));
    this.initReplyForm();
    this.loadTicketDetails();
  }

  initReplyForm() {
    this.replyForm = this.fb.group({
      subject: ['', Validators.required],
      message: ['', Validators.required],
      status: [0, Validators.required], // default new status (e.g., Open)
      pictureUrl: [null],
    });
  }

  loadTicketDetails() {
    this.loading = true;
    this.supportService
      .getUserSubmittedRequestsDetails(this.ticketId)
      .subscribe({
        next: (res) => {
          this.ticketDetails = res;
          console.log('got', res);
          this.loading = false;
        },
        error: (err) => {
          console.error('Failed to load ticket details', err);
          this.loading = false;
        },
      });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.replyForm.patchValue({ pictureUrl: file });
    }
  }

  submitReply() {
    if (this.replyForm.invalid) return;

    const dto: CreateSupportResponseDto = {
      supportRequestId: this.ticketId,
      subject: this.replyForm.value.subject,
      message: this.replyForm.value.message,
      status: this.replyForm.value.status,
      pictureUrl: this.replyForm.value.pictureUrl,
    };

    this.submitting = true;
    this.supportService.createSupportResponse(dto).subscribe({
      next: () => {
        this.submitting = false;
        this.replyForm.reset({ status: 0 });
        this.loadTicketDetails(); // reload timeline
      },
      error: (err) => {
        this.submitting = false;
        console.error('Reply failed', err);
      },
    });
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
