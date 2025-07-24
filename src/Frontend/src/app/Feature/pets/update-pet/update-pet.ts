import { Component } from '@angular/core';
import {
  ActivatedRoute,
  Router,
  RouterLink,
  RouterModule,
} from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Category } from '../../../models/category';
import { Breed } from '../../../models/breed';
import { AddPetRequest } from '../../../models/add-pet-request';
import { PetDetailsModel } from '../../../models/pet-details';
import { PetService } from '../pet-service';
import { EnumService } from '../../../core/services/enum-service';
import { CategoryService } from '../../categories/category-service';
import { BreedService } from '../../breeds/breed-service';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-update-pet',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './update-pet.html',
  styleUrl: './update-pet.css',
})
export class UpdatePet {
  categories: Category[] = [];
  selectedCategoryId: number = 0;
  breeds: Breed[] = [];
  filteredBreeds: Breed[] = [];

  pet: AddPetRequest = {
    Name: '',
    Status: 0,
    Age: 0,
    Ownership: 0,
    BreedId: 0,
    ImgURL: null!,
  };

  petDetail: PetDetailsModel = {
    id: 0,
    name: '',
    status: 0,
    isApproved: false,
    ownership: 0,
    breadName: '',
    imgUrl: '',
    categoryName: '',
    age: 0,
    customerId: '',
  };

  statusOptions: { key: number; value: string }[] = [];
  backendErrors: { [key: string]: string[] } = {};
  loading = true;
  imageError = '';
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private petService: PetService,
    private enumService: EnumService,
    private categoryService: CategoryService,
    private breedService: BreedService,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
    this.enumService.loadAllEnums().subscribe(() => {
      this.statusOptions = this.enumService.getStatusOptions();
    });

    this.loadCategories();
    this.loadBreeds();

    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!isNaN(id)) {
      this.petService.getPetById(id).subscribe({
        next: (response) => {
          this.petDetail = response;
          this.pet.Name = response.name;
          this.pet.Status = response.status;
          this.pet.Age = response.age;
          this.pet.BreedId = this.findBreedIdByName(response.breadName);
          this.selectedCategoryId = this.findCategoryIdByName(
            response.categoryName
          );
          this.filterBreeds();
          this.loading = false;
        },
        error: () => {
          this.error = 'Failed to load pet details.';
          this.loading = false;
        },
      });
    } else {
      this.error = 'Invalid pet ID.';
      this.loading = false;
    }
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.pet.ImgURL = file;
      this.imageError = '';
    }
  }

  onSubmit(form: NgForm): void {
    this.backendErrors = {};
    this.imageError = '';

    // Mark all controls as touched
    Object.values(form.controls).forEach((control) => control.markAsTouched());

    if (this.pet.BreedId === 0) {
      this.backendErrors['BreedId'] = ['Breed is required.'];
    }
    if (this.selectedCategoryId === 0) {
      this.backendErrors['Category'] = ['Category is required.'];
    }

    if (
      form.invalid ||
      this.pet.BreedId === 0 ||
      this.selectedCategoryId === 0
    ) {
      return;
    }

    const id = this.petDetail.id;
    this.petService.updatePet(id, this.pet).subscribe({
      next: () => {
        this.alert.success('Pet updated successfully!');
        this.router.navigate(['/pets']);
      },
      error: (err) => {
        if (err.status === 400 && err.error?.data) {
          this.backendErrors = err.error.data;
        } else {
          console.error('Unexpected error:', err);
        }
        this.alert.error('Failed to update pet.');
      },
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (cats) => {
        this.categories = cats;
      },
      error: (err) => console.error('Error loading categories:', err),
    });
  }

  loadBreeds(): void {
    this.breedService.getAllBreeds().subscribe({
      next: (breeds) => {
        this.breeds = breeds;
      },
      error: (err) => console.error('Error loading breeds', err),
    });
  }

  onCategoryChange(event: Event): void {
    this.selectedCategoryId = Number((event.target as HTMLSelectElement).value);
    this.pet.BreedId = 0;
    this.filterBreeds();
  }

  filterBreeds(): void {
    const selectedCatName = this.categories.find(
      (cat) => cat.id === this.selectedCategoryId
    )?.name;
    this.filteredBreeds = this.breeds.filter(
      (b) => b.categoryName === selectedCatName
    );
  }

  findBreedIdByName(name: string): number {
    return this.breeds.find((b) => b.name === name)?.id || 0;
  }

  findCategoryIdByName(name: string): number {
    return this.categories.find((c) => c.name === name)?.id || 0;
  }
}
