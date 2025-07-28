import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PetService } from '../pet-service';
import { CommonModule } from '@angular/common';
import { PetDetailsModel } from '../../../models/pet-details';
import { EnumService } from '../../../core/services/enum-service';
import { AlertService } from '../../../core/services/alert-service';
import { AuthService } from '../../../core/services/auth-service';
import { AdoptionRequest } from '../../../models/adoption-request';
import { AdoptionService } from '../../../core/services/adoption-service';
import { Pet } from '../../../models/pet';
import { AdoptionResponse } from '../../../models/adoption-response';

@Component({
  selector: 'app-pet-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './pet-details.html',
  styleUrl: './pet-details.css',
})
export class PetDetails implements OnInit {
  pet!: PetDetailsModel;
  loading = true;
  error = '';
  ownershipMap: { [key: number]: string } = {};
  UrlId: number = 0;
  requestedPetIds: number[] = []; // pet IDs user has requested
  allSentRequests: AdoptionResponse[] = []; // all requests sent by the user
  isDataReady: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private petService: PetService,
    private enumservice: EnumService,
    private router: Router,
    private alert: AlertService,
    public authService: AuthService,
    public adoptionService: AdoptionService,
    private cdRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.UrlId = Number(this.route.snapshot.paramMap.get('id'));
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!isNaN(id)) {
      this.petService.getPetById(id).subscribe({
        next: (response) => {
          this.pet = response;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load pet details.';
          this.loading = false;
          console.error(err);
        },
      });
    } else {
      this.error = 'Invalid pet ID.';
      this.loading = false;
    }

    this.enumservice.loadAllEnums().subscribe();
    this.loadSubmittedRequests();
  }
  async deletePetById(id: number): Promise<void> {
    const confirmed = await this.alert.confirm(
      'Are you sure you want to delete this pet?',
      'Delete Confirmation',
      'Yes, delete it',
      'Cancel'
    );

    if (!confirmed) return;

    this.petService.deletePet(id).subscribe({
      next: () => {
        this.alert.success('Pet deleted successfully!');
        this.router.navigate(['/pets']);
      },
      error: (err) => {
        this.alert.error('Failed to delete pet.');
        console.error(err);
      },
    });
  }

  getOwnershipLabel(code: number): string {
    return this.enumservice.getOwnershipLabel(code);
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }

  sendAdoptionRequest(pet: PetDetailsModel) {
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

  cancelAdoptionRequest(pet: PetDetailsModel): void {
    const request = this.allSentRequests.find((r) => r.petId === pet.id);
    if (!request) return;

    const body = {
      petId: pet.id,
      recCustomerId: pet.customerId,
      adoptionDate: this.adoptionService.padDate(
        request.adoptionDate.replace('T', ' ').replace('Z', '')
      ),
    };
    console.log('Cancelling request:', body);
    this.adoptionService.cancelRequest(body).subscribe({
      next: () => {
        this.alert.success('Request cancelled.');
        this.requestedPetIds = this.requestedPetIds.filter(
          (id) => id !== pet.id
        );
        this.loadSubmittedRequests();
        this.cdRef.detectChanges();
      },
      error: (err) => this.alert.error('Failed to cancel request.'),
    });
  }
}
