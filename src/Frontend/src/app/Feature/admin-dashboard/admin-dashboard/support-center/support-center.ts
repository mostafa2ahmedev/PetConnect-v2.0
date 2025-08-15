import { Component, OnInit } from '@angular/core';
import { SupportRequest } from '../../../support/support-models';
import { SupportServie } from '../../../support/support-servie';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-support-center',
  imports: [CommonModule, FormsModule],
  templateUrl: './support-center.html',
  styleUrl: './support-center.css',
})
export class SupportCenter implements OnInit {
  supportRequests: SupportRequest[] = [];
  loading = true;

  // Map numeric type to readable text & badge color
  typeMap: { [key: number]: { text: string; color: string } } = {
    0: { text: 'Support', color: 'primary' },
    1: { text: 'Consulting', color: 'info' },
    2: { text: 'Marketing', color: 'warning' },
    3: { text: 'Issue', color: 'danger' },
  };

  // Map numeric status to readable text & badge color
  statusMap: { [key: number]: { text: string; color: string } } = {
    0: { text: 'Open', color: 'success' },
    1: { text: 'Waiting Reply', color: 'warning' },
    2: { text: 'Closed', color: 'secondary' },
  };

  constructor(private supportService: SupportServie) {}

  ngOnInit(): void {
    this.supportService.getSupportRequests().subscribe({
      next: (data) => {
        this.supportRequests = data;
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

  typeKeys = Object.keys(this.typeMap).map((k) => Number(k));
  statusKeys = Object.keys(this.statusMap).map((k) => Number(k));

  filteredRequests: SupportRequest[] = [];

  applyFilters() {
    this.filteredRequests = this.supportRequests.filter((req) => {
      return (
        (!this.filters.username ||
          req.userName
            .toLowerCase()
            .includes(this.filters.username.toLowerCase())) &&
        (!this.filters.type || req.type.toString() == this.filters.type) &&
        (!this.filters.status || req.status.toString() == this.filters.status)
      );
    });
  }

  resetFilters() {
    this.filters = { username: '', type: '', status: '' };
    this.filteredRequests = [...this.supportRequests];
  }
}
