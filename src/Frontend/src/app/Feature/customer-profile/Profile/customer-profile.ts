import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AdoptionService } from '../../../core/services/adoption-service';
import { AdoptionResponse } from '../../../models/adoption-response';
import { CustomerService } from '../customer-service';
import { CusotmerPet } from '../../../models/cusotmer-pet';
import { CustomerPofileDetails } from '../../../models/customer-pofile-details';
import { AlertService } from '../../../core/services/alert-service';
import { AdoptionDecision } from '../../../models/adoption-decision';
import { AccountService } from '../../../core/services/account-service';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-customer-profile',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './customer-profile.html',
  styleUrl: './customer-profile.css',
})
export class CustomerProfile implements OnInit, OnDestroy {
  loadingReuests: boolean = true;
  loadingProfile: boolean = true;
  loadingPets: boolean = true;
  myPets: CusotmerPet[] = [];
  ReceivedRequests: any[] = [];
  profileData: CustomerPofileDetails | null = null;
  petCount: number = 0;
  selectedPetFilter: string = '';
  sortOrder: 'asc' | 'desc' = 'desc';
  sentRequests: AdoptionResponse[] = [];
  requestedPetIds: number[] = [];
  private notifSub!: Subscription;

  constructor(
    private adoptionService: AdoptionService,
    private customerService: CustomerService,
    private alert: AlertService,
    private cdRef: ChangeDetectorRef,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadSentRequests();
    this.loadCustomerPets();
    this.loadReceivedRequests();
    this.loadProfile();

    this.notifSub = this.notificationService.newNotification$.subscribe(
      (notif) => {
        if (!notif) return;

        // If notification is adoption-related
        console.log(
          '🔄 Reloading adoption requests due to notification:',
          notif.message
        );
        this.loadSentRequests();
        this.loadCustomerPets();
        this.loadReceivedRequests();
      }
    );
  }
  ngOnDestroy(): void {
    if (this.notifSub) {
      this.notifSub.unsubscribe();
    }
  }
  loadSentRequests(): void {
    this.adoptionService.getIncomingRequests().subscribe({
      next: (requests) => {
        console.log('Sent Requests:', requests);
        this.sentRequests = requests;
        this.requestedPetIds = requests.map((r) => r.petId);
        this.loadingReuests = false;
      },
      error: (err) => {
        console.error('Failed to load customer sent requests', err);
        this.loadingReuests = false;
      },
    });
  }
  loadCustomerPets(): void {
    this.customerService.getCustomerPets().subscribe({
      next: (pets) => {
        this.myPets = pets;
        this.petCount = pets.length;
        this.loadingPets = false;
        console.log('Owned Pets:', this.myPets);
      },
      error: (err) => {
        console.error('Failed to load owned pets', err);
      },
    });
  }
  loadReceivedRequests(): void {
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
  loadProfile(): void {
    this.customerService.getCustomerProfile().subscribe((data) => {
      console.log('Profile Data:', data);
      this.profileData = data;
      this.loadingProfile = false;
    });
  }
  get uniquePetNames(): string[] {
    const names = this.ReceivedRequests.map((r) => r.petName);
    return [...new Set(names)];
  }

  get filteredAndSortedRequests() {
    let filtered = this.ReceivedRequests;

    if (this.selectedPetFilter) {
      filtered = filtered.filter((r) => r.petName === this.selectedPetFilter);
    }

    return filtered.sort((a, b) => {
      const dateA = new Date(a.adoptionDate).getTime();
      const dateB = new Date(b.adoptionDate).getTime();
      return this.sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    });
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }

  cancelAdoptionRequest(pet: AdoptionResponse): void {
    const request = this.sentRequests.find((r) => r.petId === pet.petId);
    if (!request) return;

    const body = {
      petId: pet.petId,
      recCustomerId: request.recCustomerId,
      adoptionDate: this.adoptionService.padDate(
        request.adoptionDate.replace('T', ' ').replace('Z', '')
      ),
    };
    console.log('Cancelling request:', body);
    this.adoptionService.cancelRequest(body).subscribe({
      next: () => {
        this.alert.success('Request cancelled.');
        this.requestedPetIds = this.requestedPetIds.filter(
          (id) => id !== pet.petId
        );
        this.loadSentRequests();
        this.cdRef.detectChanges();
      },
      error: (err) => this.alert.error('Failed to cancel request.'),
    });
  }

  approveOrCancel(request: any, status: number): void {
    const decision = {
      petId: request.petId,
      reqCustomerId: request.reqCustomerId, // ✅ correct spelling
      adoptionDate: request.adoptionDate.replace('T', ' ').replace('Z', ''),
      adoptionStatus: status,
    };

    this.adoptionService.approveOrCancelRequest(decision).subscribe({
      next: () => {
        this.alert.success(
          status === 0
            ? 'Adoption request approved.'
            : 'Adoption request cancelled.'
        );
        // Update UI instantly
        request.adoptionStatus = status;
        this.loadSentRequests();
        this.loadCustomerPets();
        this.loadReceivedRequests();
      },
      error: () => {
        this.alert.error('Failed to update request.');
      },
    });
  }
}
