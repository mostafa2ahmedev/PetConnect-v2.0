import { Component } from '@angular/core';
import { Category } from '../../../models/category';
import { CategoryService } from '../category-service';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-category',
  imports: [FormsModule],
  templateUrl: './add-category.html',
  styleUrl: './add-category.css',
})
export class AddCategory {
  category: Category = {
    id: 0,
    name: '',
  };

  saving = false;
  error = '';

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {}

  onSubmit(form: NgForm): void {
    if (form.invalid) return;

    this.saving = true;
    this.categoryService.addCategory(this.category.name).subscribe({
      next: () => {
        alert('Category added successfully!');
        this.router.navigate(['/categories']);
      },
      error: (err) => {
        console.error('Add failed:', err);
        this.error = 'Failed to add category.';
        this.saving = false;
      },
    });
  }
}
