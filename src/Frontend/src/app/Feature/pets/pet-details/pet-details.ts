import { Component, OnInit } from '@angular/core';
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

  constructor(
    private route: ActivatedRoute,
    private petService: PetService,
    private enumservice: EnumService,
    private router: Router,
    private alert: AlertService,
    public authService: AuthService,
    public adoptionService: AdoptionService
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

  sendAdoptionRequest(pet: PetDetailsModel): void {
    const request: AdoptionRequest = {
      petId: pet.id,
      recCustomerId: pet.customerId,
    };

    this.adoptionService.submitRequest(request).subscribe({
      next: () => {
        this.alert.success('Adoption request sent successfully!');
        // Optionally update UI state here (e.g., disable button or show cancel)
      },
      error: (err) => {
        this.alert.error('Failed to send adoption request.');
        console.error(err);
      },
    });
  }

  loadSubmittedRequests(): void {
    this.adoptionService.getIncomingRequests().subscribe({
      next: (requests) => {
        console.log(requests);
        this.requestedPetIds = requests.map((r) => r.petId);
      },
      error: (err) => {
        console.error('Failed to load submitted requests', err);
      },
    });
  }
}
