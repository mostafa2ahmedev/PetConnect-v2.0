import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Pet } from '../../../models/pet';
import { PetService } from '../pet-service';
import { EnumService } from '../../../core/services/enum-service';
import { AuthService } from '../../../core/services/auth-service';
import { AdoptionService } from '../../../core/services/adoption-service';
import { AdoptionRequest } from '../../../models/adoption-request';
import { AlertService } from '../../../core/services/alert-service';
import { AdoptionResponse } from '../../../models/adoption-response';
import { Category } from '../../../models/category';
import { CategoryService } from '../../categories/category-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pets',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './pets.html',
  styleUrl: './pets.css',
})
export class Pets implements OnInit {
  pets: Pet[] = [];
  statusMap: { [key: number]: string } = {};
  loading = true;
  error = '';
  isDataReady: boolean = false;
  categories: Category[] = [];
  filters = {
    categoryId: '',
    sortOrder: '',
    location: '',
  };
  filteredPets: Pet[] = []; // visible list
  constructor(
    private petService: PetService,
    private enumService: EnumService,
    public authService: AuthService,
    public adoptionService: AdoptionService,
    private alert: AlertService,
    private cdRef: ChangeDetectorRef,
    private categoryService: CategoryService
  ) {}
  requestedPetIds: number[] = []; // pet IDs user has requested
  allSentRequests: AdoptionResponse[] = []; // all requests sent by the user
  ngOnInit(): void {
    this.enumService.loadAllEnums().subscribe();
    this.loadCategories();

    this.loadPets();
    this.loadSubmittedRequests();
  }

  loadPets(): void {
    this.petService.getAllPets().subscribe({
      next: (pets) => {
        this.pets = pets;
        this.filteredPets = pets;
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
    const recCustomerId = pet.customerId;

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
      recCustomerId: pet.customerId,
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

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => (this.categories = data),
      error: (err) => console.error('Error loading categories', err),
    });
  }
  onCategoryChange(): void {
    // Optional: You could trigger live filtering here or leave it for submit
  }
  applyFilters(): void {
    const categoryName = this.getCategoryNameById(+this.filters.categoryId);

    let result = this.pets;

    // Filter by category
    if (this.filters.categoryId) {
      result = result.filter((pet) => pet.categoryName === categoryName);
    }

    // Filter by location (partial match in street/city/country)
    if (this.filters.location) {
      const keyword = this.filters.location.toLowerCase();
      result = result.filter(
        (pet) =>
          pet.customerStreet?.toLowerCase().includes(keyword) ||
          pet.customerCity?.toLowerCase().includes(keyword) ||
          pet.customerCountry?.toLowerCase().includes(keyword)
      );
    }

    // Sort by age
    if (this.filters.sortOrder === 'asc') {
      result = result.sort((a, b) => a.age - b.age);
    } else if (this.filters.sortOrder === 'desc') {
      result = result.sort((a, b) => b.age - a.age);
    }

    this.filteredPets = result;
  }

  getCategoryNameById(id: number): string {
    const category = this.categories.find((cat) => cat.id === id);
    return category ? category.name : '';
  }
}
