import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AdoptionService } from '../../core/services/adoption-service';
import { AdoptionResponse } from '../../models/adoption-response';

@Component({
  selector: 'app-customer-profile',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './customer-profile.html',
  styleUrl: './customer-profile.css',
})
export class CustomerProfile {
  constructor(private adoptionService: AdoptionService) {}
  customer = {
    name: 'Eslaam Mohamed',
    email: 'eslaam@example.com',
    gender: 'Male',
    phone: '01012345678',
    createdAt: '2024-05-01',
    pets: [],
    receivedRequests: [],
    sentRequests: [] as AdoptionResponse[],
  };
  ngOnInit(): void {
    this.loadSentRequests();
  }

  loadSentRequests(): void {
    this.adoptionService.getIncomingRequests().subscribe({
      next: (requests) => {
        console.log('Sent Requests:', requests);
        this.customer.sentRequests = requests;
      },
      error: (err) =>
        console.error('Failed to load customer sent requests', err),
    });
  }
}
