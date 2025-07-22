import { Component } from '@angular/core';

import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Breed } from '../../../models/breed';
import { Category } from '../../../models/category';
import { BreedService } from '../breed-service';
import { CategoryService } from '../../categories/category-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-all-breeds',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './all-breeds.html',
  styleUrl: './all-breeds.css',
})
export class AllBreeds {
  breeds: Breed[] = [];
  filteredBreeds: Breed[] = [];
  categories: Category[] = [];
  selectedCategoryId: number = 0;
  loading = true;

  constructor(
    private breedService: BreedService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.loadBreeds();
    this.loadCategories();
  }

  loadBreeds(): void {
    this.breedService.getAllBreeds().subscribe({
      next: (data) => {
        this.breeds = data;
        this.filteredBreeds = data;
        this.loading = false;
      },
      error: (err) => console.error('Failed to load breeds', err),
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => (this.categories = data),
      error: (err) => console.error('Failed to load categories', err),
    });
  }

  onCategoryChange(event: Event): void {
    const selectedId = Number((event.target as HTMLSelectElement).value);
    this.selectedCategoryId = selectedId;

    const selectedCategory = this.categories.find(
      (cat) => cat.id === selectedId
    )?.name;

    if (selectedCategory) {
      this.filteredBreeds = this.breeds.filter(
        (breed) => breed.categoryName === selectedCategory
      );
    } else {
      this.filteredBreeds = this.breeds;
    }
  }
}
