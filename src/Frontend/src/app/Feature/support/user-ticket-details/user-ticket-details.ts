import { Component, OnInit } from '@angular/core';
import {
  CreateFollowUpSupportRequestDto,
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
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-ticket-details',
  imports: [DatePipe, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './user-ticket-details.html',
  styleUrl: './user-ticket-details.css',
})
export class UserTicketDetails implements OnInit {
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
      message: ['', Validators.required],
      attachment: [null], // file
    });
  }

  loadTicketDetails() {
    this.loading = true;
    this.supportService
      .getUserSubmittedRequestsDetails(this.ticketId)
      .subscribe({
        next: (res) => {
          this.ticketDetails = res;
          this.loading = false;
          console.log(this.ticketDetails);
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
      this.replyForm.patchValue({ attachment: file });
      console.log(file);
    }
  }

  submitReply() {
    if (this.replyForm.invalid) return;

    this.submitting = true;

    this.supportService
      .createFollowUpRequest({
        supportRequestId: this.ticketId,
        message: this.replyForm.value.message,
        pictureUrl: this.replyForm.value.attachment, // coming from file input
      })
      .subscribe({
        next: () => {
          this.submitting = false;
          this.replyForm.reset();
          this.loadTicketDetails();
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
