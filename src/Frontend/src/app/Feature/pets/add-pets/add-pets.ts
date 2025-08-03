import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AddPetRequest } from '../../../models/add-pet-request';
import { PetService } from '../pet-service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EnumService } from '../../../core/services/enum-service';
import { Category } from '../../../models/category';
import { CategoryService } from '../../categories/category-service';
import { BreedService } from '../../breeds/breed-service';
import { Breed } from '../../../models/breed';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-add-pets',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './add-pets.html',
  styleUrl: './add-pets.css',
})
export class AddPets {
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
    Notes: '',
  };
  imageError: string = '';
  statusOptions: { key: number; value: string }[] = [];
  ownershipOptions: { key: number; value: string }[] = [];
  backendErrors: { [key: string]: string[] } = {};

  constructor(
    private petService: PetService,
    private router: Router,
    private enumService: EnumService,
    private categoryService: CategoryService,
    private breedService: BreedService,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
    this.enumService.loadAllEnums().subscribe(() => {
      this.statusOptions = this.enumService.getStatusOptions();
      this.ownershipOptions = this.enumService.getOwnershipOptions();
    });
    this.loadCategories();
    this.loadBreeds();
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.pet.ImgURL = file;
      this.imageError = '';
    } else {
      this.imageError = 'Image is required.';
    }
  }
  onSubmit(form: NgForm): void {
    this.imageError = '';

    if (form.invalid || !this.pet.ImgURL) {
      if (form.invalid || !this.pet.ImgURL) {
        Object.values(form.controls).forEach((control) =>
          control.markAsTouched()
        );

        this.imageError = !this.pet.ImgURL ? 'Image is required.' : '';
        return;
      }
    }

    this.petService.addPet(this.pet).subscribe({
      next: () => {
        this.alert.success('Pet added successfully!');

        this.router.navigate(['/pets']);
      },
      error: (err) => {
        if (err.status === 400 && err.error?.data) {
          this.backendErrors = err.error.data;
        } else {
          console.error('Unexpected error:', err);
        }
        console.error('Error adding pet:', err);
      },
    });
  }
  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (cats) => {
        this.categories = cats;
      },
      error: (err) => {
        console.error('Error loading categories:', err);
      },
    });
  }

  loadBreeds(): void {
    this.breedService.getAllBreeds().subscribe({
      next: (breeds) => {
        this.breeds = breeds;
        console.log('Breeds loaded:', this.breeds);
      },
      error: (err) => {
        console.error('Error loading breeds', err);
      },
    });
  }

  onCategoryChange(event: Event): void {
    this.selectedCategoryId = Number((event.target as HTMLSelectElement).value);
    this.pet.BreedId = 0;

    const selectedCategory = this.categories.find(
      (cat) => cat.id === this.selectedCategoryId
    )?.name;

    if (selectedCategory) {
      this.filteredBreeds = this.breeds.filter(
        (breed) => breed.categoryName === selectedCategory
      );
    } else {
      this.filteredBreeds = [];
    }
  }
}
