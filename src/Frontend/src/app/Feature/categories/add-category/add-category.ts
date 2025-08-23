import { Component } from '@angular/core';
import { Category } from '../../../models/category';
import { CategoryService } from '../category-service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-add-category',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './add-category.html',
  styleUrl: './add-category.css',
})
export class AddCategory {
  category: Category = { id: 0, name: '' };
  saving = false;
  error = '';
  fieldErrors: { [key: string]: string[] } = {}; // backend errors

  constructor(
    private categoryService: CategoryService,
    private router: Router,
    private alert: AlertService
  ) {}

  onSubmit(form: NgForm): void {
    this.fieldErrors = {};

    if (form.invalid) {
      // Mark all fields as touched and dirty to trigger validation messages
      Object.values(form.controls).forEach((control) => {
        control.markAsTouched();
        control.markAsDirty();
      });
      return;
    }

    this.saving = true;
    this.categoryService.addCategory(this.category.name).subscribe({
      next: () => {
        this.alert.success('Category added successfully!');
        this.router.navigate(['/admin/categories']);
      },
      error: (err) => {
        this.saving = false;
        if (err.status === 400 && err.error?.data) {
          this.fieldErrors = err.error.data;
        } else {
          console.error('Add failed:', err);
          this.alert.error('Failed to add category');
        }
      },
    });
  }
}
