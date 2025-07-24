import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Pet } from '../../../models/pet';
import { PetService } from '../pet-service';
import { EnumService } from '../../../core/services/enum-service';
import { AccountService } from '../../../core/services/account-service';
import { AuthService } from '../../../core/services/auth-service';
import { AdoptionService } from '../../../core/services/adoption-service';
import { AdoptionRequest } from '../../../models/adoption-request';
import { AlertService } from '../../../core/services/alert-service';
import { AdoptionResponse } from '../../../models/adoption-response';

@Component({
  selector: 'app-pets',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './pets.html',
  styleUrl: './pets.css',
})
export class Pets implements OnInit {
  pets: Pet[] = [];
  statusMap: { [key: number]: string } = {};
  loading = true;
  error = '';
  isDataReady: boolean = false;
  constructor(
    private petService: PetService,
    private enumService: EnumService,
    public authService: AuthService,
    public adoptionService: AdoptionService,
    private alert: AlertService,
    private cdRef: ChangeDetectorRef
  ) {}
  requestedPetIds: number[] = []; // pet IDs user has requested
  allSentRequests: AdoptionResponse[] = []; // all requests sent by the user
  ngOnInit(): void {
    this.enumService.loadAllEnums().subscribe();

    this.loadPets();
    this.loadSubmittedRequests();
  }

  loadPets(): void {
    this.petService.getAllPets().subscribe({
      next: (pets) => {
        this.pets = pets;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading pets:', err);
        this.error = 'Failed to load pets. Please try again later.';
        this.loading = false;
      },
    });
  }

  getStatusLabel(statusCode: number): string {
    return this.enumService.getStatusLabel(statusCode);
  }

  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }

  sendAdoptionRequest(pet: Pet) {
    const recCustomerId = this.authService.getUserId();

    if (!recCustomerId) {
      console.error('User ID is null. User might not be logged in.');
      return;
    }

    const request: AdoptionRequest = {
      recCustomerId,
      petId: pet.id,
    };
    console.log(request);
    this.adoptionService.submitRequest(request).subscribe({
      next: () => {
        this.alert.success('Adoption request sent');
        this.loadSubmittedRequests();
        this.cdRef.detectChanges();
      },
      error: (err) => console.error(err),
    });
  }

  loadSubmittedRequests(): void {
    this.adoptionService.getIncomingRequests().subscribe({
      next: (requests) => {
        console.log(requests);
        this.allSentRequests = requests;
        this.requestedPetIds = requests.map((r) => r.petId);
        this.isDataReady = true;
      },
      error: (err) => {
        console.error('Failed to load submitted requests', err);
      },
    });
  }

  cancelAdoptionRequest(pet: Pet): void {
    const request = this.allSentRequests.find((r) => r.petId === pet.id);
    if (!request) return;

    const body = {
      petId: pet.id,
      recCustomerId: this.authService.getUserId(),
      adoptionDate: this.adoptionService.padDate(
        request.adoptionDate.replace('T', ' ').replace('Z', '')
      ),
    };
    console.log('Cancelling request:', body);
    this.adoptionService.cancelRequest(body).subscribe({
      next: () => {
        this.alert.success('Request cancelled.');

        this.loadSubmittedRequests();
        this.cdRef.detectChanges();
      },
      error: (err) => this.alert.error('Failed to cancel request.'),
    });
  }
}
