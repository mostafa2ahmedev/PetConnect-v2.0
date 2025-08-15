import { Component, OnInit } from '@angular/core';
import {
  CreateSupportResponseDto,
  SupportRequest,
} from '../../../support/support-models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SupportServie } from '../../../support/support-servie';
import { AlertService } from '../../../../core/services/alert-service';

@Component({
  selector: 'app-support-center',
  imports: [CommonModule, FormsModule],
  templateUrl: './support-center.html',
  styleUrl: './support-center.css',
})
export class SupportCenter implements OnInit {
  supportRequests: SupportRequest[] = [];
  loading = true;
  showFilters = false;
  // Map numeric type to readable text & badge color
  typeMap: { [key: string]: { text: string; color: string } } = {
    Support: { text: 'Support', color: 'primary' },
    Consulting: { text: 'Consulting', color: 'info' },
    Marketing: { text: 'Marketing', color: 'dark' },
    Issue: { text: 'Issue', color: 'danger' },
  };

  statusMap: { [key: string]: { text: string; color: string } } = {
    Open: { text: 'Open', color: 'primary' },
    WaitingReply: { text: 'Waiting Reply', color: 'dark' },
    Closed: { text: 'Closed', color: 'secondary' },
  };

  // Keys for looping
  typeKeys: string[] = Object.keys(this.typeMap);
  statusKeys: string[] = Object.keys(this.statusMap);
  constructor(
    private supportService: SupportServie,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadTickets();
  }
  loadTickets() {
    this.supportService.getSupportRequests().subscribe({
      next: (data) => {
        this.supportRequests = data;
        console.log('supportRequests', this.supportRequests);
        this.filteredRequests = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading support requests:', err);
        this.loading = false;
      },
    });
  }
  filters = {
    username: '',
    type: '',
    status: '',
  };

  filteredRequests: SupportRequest[] = [];

  applyFilters() {
    this.filteredRequests = this.supportRequests.filter((req) => {
      return (
        (!this.filters.username ||
          req.userName
            .toLowerCase()
            .includes(this.filters.username.toLowerCase())) &&
        (!this.filters.type ||
          req.supportRequestType.toString() == this.filters.type) &&
        (!this.filters.status ||
          req.supportRequestStatus.toString() == this.filters.status)
      );
    });
  }

  resetFilters() {
    this.filters = { username: '', type: '', status: '' };
    this.filteredRequests = [...this.supportRequests];
  }

  replyFormVisible: { [key: number]: boolean } = {};
  replyData: { [key: number]: CreateSupportResponseDto } = {};

  toggleReplyForm(requestId: number) {
    this.replyFormVisible[requestId] = !this.replyFormVisible[requestId];
    if (!this.replyData[requestId]) {
      this.replyData[requestId] = {
        message: '',
        subject: '',
        supportRequestId: requestId,
        status: 2, // default status, change if needed
      };
    }
  }

  sendReply(requestId: number) {
    const payload = this.replyData[requestId];
    payload.status = this.statusKeys.indexOf(payload.status.toString());
    console.log(payload);

    if (!payload.message.trim() || !payload.subject.trim()) {
      this.alertService.error('Please fill out both subject and message.');
      return;
    }

    this.supportService.createSupportResponse(payload).subscribe({
      next: () => {
        this.alertService.success('Reply sent successfully!');
        this.replyFormVisible[requestId] = false;
        this.loadTickets();
      },
      error: (err) => {
        console.error(err);
        this.alertService.error('Failed to send reply.');
      },
    });
  }
}
