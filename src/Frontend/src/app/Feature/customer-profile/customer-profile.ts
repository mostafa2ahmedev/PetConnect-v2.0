import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AdoptionService } from '../../core/services/adoption-service';
import { AdoptionResponse } from '../../models/adoption-response';
import { Pet } from '../../models/pet';
import { CustomerService } from './customer-service';
import { CusotmerPet } from '../../models/cusotmer-pet';

@Component({
  selector: 'app-customer-profile',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './customer-profile.html',
  styleUrl: './customer-profile.css',
})
export class CustomerProfile {
  loadingReuests: boolean = true;
  myPets: CusotmerPet[] = [];
  ReceivedRequests: any[] = [];
  constructor(
    private adoptionService: AdoptionService,
    private customerService: CustomerService
  ) {}
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
    this.customerService.getCustomerPets().subscribe({
      next: (pets) => {
        this.myPets = pets;
        console.log('Owned Pets:', this.myPets);
      },
      error: (err) => {
        console.error('Failed to load owned pets', err);
      },
    });
    this.adoptionService.getReceivedAdoptionRequests().subscribe({
      next: (pets) => {
        this.ReceivedRequests = pets;
        console.log('ReceivedRequests', this.ReceivedRequests);
      },
      error: (err) => {
        console.error('Failed to load owned pets', err);
      },
    });
  }

  loadSentRequests(): void {
    this.adoptionService.getIncomingRequests().subscribe({
      next: (requests) => {
        console.log('Sent Requests:', requests);
        this.customer.sentRequests = requests;
        this.loadingReuests = false;
      },
      error: (err) => {
        console.error('Failed to load customer sent requests', err);
        this.loadingReuests = false;
      },
    });
  }

  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
