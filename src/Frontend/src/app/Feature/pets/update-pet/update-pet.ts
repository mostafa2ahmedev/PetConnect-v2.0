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
    name: '',
    status: 0,
    isApproved: false,
    ownership: 0,
    breedId: 0,
    imageFile: null!,
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
  };

  statusOptions: { key: number; value: string }[] = [];
  loading = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private petService: PetService,
    private enumService: EnumService,
    private categoryService: CategoryService,
    private breedService: BreedService
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
          this.pet.name = response.name;
          this.pet.status = response.status;
          this.pet.breedId = this.findBreedIdByName(response.breadName);
          this.selectedCategoryId = this.findCategoryIdByName(
            response.categoryName
          );
          this.filterBreeds();
          this.loading = false;
          console.log(this.petDetail);
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
      this.pet.imageFile = file;
    }
  }

  onSubmit(form: NgForm): void {
    console.log('Form submitted:', this.pet);
    if (form.invalid) return;

    const id = this.petDetail.id;
    this.petService
      .updatePet(Number(this.route.snapshot.paramMap.get('id')), this.pet)
      .subscribe({
        next: () => {
          alert('Pet updated successfully!');
          this.router.navigate(['/pets']);
        },
        error: (err) => {
          console.error('Error updating pet:', err);
          if (err.status === 400 && err.error?.errors) {
            console.table(err.error.errors); // Shows all validation errors
          }
          alert('Failed to update pet. Please check input.');
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
      error: (err) => console.error('Error loading breeds:', err),
    });
  }

  onCategoryChange(event: Event): void {
    this.selectedCategoryId = Number((event.target as HTMLSelectElement).value);
    this.pet.breedId = 0;
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
