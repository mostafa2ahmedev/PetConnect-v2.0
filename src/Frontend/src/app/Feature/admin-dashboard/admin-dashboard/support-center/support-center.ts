import { Component, OnInit } from '@angular/core';
import {
  CreateSupportResponseDto,
  EnumOption,
  SupportRequest,
} from '../../../support/support-models';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SupportServie } from '../../../support/support-servie';
import { AlertService } from '../../../../core/services/alert-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-support-center',
  imports: [CommonModule, FormsModule],
  templateUrl: './support-center.html',
  styleUrl: './support-center.css',
})
export class SupportCenter implements OnInit {
  requests: SupportRequest[] = [];
  filteredRequests: SupportRequest[] = [];
  loading = false;
  showFilters = false;

  // Filters
  filters = { username: '', type: '', status: '', priority: '' };

  // Enum data
  requestTypes: EnumOption[] = [];
  requestStatuses: EnumOption[] = [];
  requestPriorities: EnumOption[] = [];

  // Reply handling
  replyFormVisible: { [key: number]: boolean } = {};
  replyData: {
    [key: number]: { subject: string; message: string; status: string };
  } = {};

  constructor(
    private supportService: SupportServie,
    private alertService: AlertService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTickets();
    this.loadEnums();
  }

  // Load tickets
  loadTickets() {
    this.loading = true;
    this.supportService.getSupportRequests().subscribe({
      next: (res: SupportRequest[]) => {
        this.requests = res;
        this.filteredRequests = res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.alertService.error('Failed to load support tickets.');
      },
    });
  }

  // Load enums
  loadEnums() {
    this.supportService
      .getSupportRequestTypes()
      .subscribe((data) => (this.requestTypes = data));
    this.supportService
      .getSupportRequestStatus()
      .subscribe((data) => (this.requestStatuses = data));
    this.supportService
      .getSupportRequestPriority()
      .subscribe((data) => (this.requestPriorities = data));
  }

  // Badge colors
  getTypeBadgeClass(type: string): string {
    switch (type.toLowerCase()) {
      case 'general':
        return 'bg-secondary';
      case 'bug':
        return 'bg-info';
      case 'feature':
        return 'bg-warning';
      case 'support':
        return 'bg-dark';
      default:
        return 'bg-primary';
    }
  }

  getStatusBadgeClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'open':
        return 'bg-warning text-dark';
      case 'inprogress':
        return 'bg-primary';
      case 'resolved':
        return 'bg-success';
      case 'closed':
        return 'bg-dark';
      default:
        return 'bg-secondary';
    }
  }

  getPriorityBadgeClass(priority: string): string {
    switch (priority.toLowerCase()) {
      case 'low':
        return 'bg-success';
      case 'medium':
        return 'bg-warning text-dark';
      case 'high':
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }

  // Filtering
  applyFilters() {
    this.filteredRequests = this.requests.filter(
      (r) =>
        (!this.filters.username ||
          r.userName
            .toLowerCase()
            .includes(this.filters.username.toLowerCase())) &&
        (!this.filters.type ||
          this.requestTypes
            .find((t) => t.value === r.supportRequestType)
            ?.key.toString() === this.filters.type) &&
        (!this.filters.status ||
          this.requestStatuses
            .find((s) => s.value === r.supportRequestStatus)
            ?.key.toString() === this.filters.status) &&
        (!this.filters.priority ||
          this.requestPriorities
            .find((p) => p.value === r.priority)
            ?.key.toString() === this.filters.priority)
    );
  }

  resetFilters() {
    this.filters = { username: '', type: '', status: '', priority: '' };
    this.filteredRequests = this.requests;
  }

  // Toggle reply form
  toggleReplyForm(requestId: number) {
    this.replyFormVisible[requestId] = !this.replyFormVisible[requestId];
    if (!this.replyData[requestId]) {
      this.replyData[requestId] = { subject: '', message: '', status: '' };
    }
  }

  // Send reply (dummy for now)
  sendReply(requestId: number) {
    const reply = this.replyData[requestId];
    console.log('Reply to request:', requestId, reply);
    this.alertService.success('Reply sent successfully.');
    this.toggleReplyForm(requestId);
  }
  viewTicket(ticketId: number) {
    this.router.navigate(['/admin/support-center/ticket', ticketId]);
  }
}
