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
    name: '',
    status: 0,
    isApproved: false,
    ownership: 0,
    breedId: 0,
    imageFile: null!,
  };

  statusOptions: { key: number; value: string }[] = [];
  ownershipOptions: { key: number; value: string }[] = [];

  constructor(
    private petService: PetService,
    private router: Router,
    private enumService: EnumService,
    private categoryService: CategoryService,
    private breedService: BreedService
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
      this.pet.imageFile = file;
    }
  }

  onSubmit(form: NgForm): void {
    if (form.invalid || !this.pet.imageFile) {
      return;
    }

    this.petService.addPet(this.pet).subscribe({
      next: () => {
        alert('Pet added successfully!');
        this.router.navigate(['/pets']);
      },
      error: (err) => {
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
    this.pet.breedId = 0;

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
