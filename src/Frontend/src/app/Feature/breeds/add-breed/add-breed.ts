import { Component } from '@angular/core';
import { Category } from '../../../models/category';
import { BreedService } from '../breed-service';
import { CategoryService } from '../../categories/category-service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-breed',
  imports: [FormsModule, CommonModule],
  templateUrl: './add-breed.html',
  styleUrl: './add-breed.css',
})
export class AddBreed {
  breedName: string = '';
  selectedCategoryId: number = 0;
  categories: Category[] = [];
  error = '';
  success = '';

  constructor(
    private breedService: BreedService,
    private categoryService: CategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => (this.categories = data),
      error: (err) => {
        this.error = 'Failed to load categories.';
        console.error(err);
      },
    });
  }

  onSubmit(): void {
    if (!this.breedName || this.selectedCategoryId === 0) {
      this.error = 'Please enter breed name and select a category.';
      return;
    }

    this.breedService
      .addBreed(this.breedName, this.selectedCategoryId)
      .subscribe({
        next: () => {
          this.success = 'Breed added successfully!';
          this.router.navigate(['/breeds']);
        },
        error: (err) => {
          this.error = 'Failed to add breed.';
          console.error(err);
        },
      });
  }
}
