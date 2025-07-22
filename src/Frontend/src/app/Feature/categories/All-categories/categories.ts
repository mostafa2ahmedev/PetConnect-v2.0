import { Component } from '@angular/core';
import { Category } from '../../../models/category';
import { CategoryService } from '../category-service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-categories',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
})
export class Categories {
  categories: Category[] = [];
  loading = true;
  error = '';

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching categories:', err);
        this.error = 'Failed to load categories.';
        this.loading = false;
      },
    });
  }
}
