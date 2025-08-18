import { Component, OnInit } from '@angular/core';
import { UserSupportRequestInfo } from '../support-models';
import { SupportServie } from '../support-servie';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-submitted-requests',
  imports: [CommonModule, RouterModule],
  templateUrl: './user-submitted-requests.html',
  styleUrl: './user-submitted-requests.css',
})
export class UserSubmittedRequests implements OnInit {
  tickets: UserSupportRequestInfo[] = [];
  loading = true;
  error: string | null = null;

  constructor(private supportService: SupportServie) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.loading = true;
    this.supportService.getUserSubmittedRequestsInfo().subscribe({
      next: (data) => {
        this.tickets = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load tickets';
        this.loading = false;
      },
    });
  }
}
