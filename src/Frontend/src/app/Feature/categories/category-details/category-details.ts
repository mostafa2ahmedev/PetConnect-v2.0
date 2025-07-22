import { ChangeDetectorRef, Component } from '@angular/core';
import { Category } from '../../../models/category';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CategoryService } from '../category-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-category-details',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './category-details.html',
  styleUrl: './category-details.css',
})
export class CategoryDetails {
  category: Category = { id: 0, name: '' };
  error = '';
  loading = true;
  saving = false;
  deleting = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private categoryService: CategoryService,
    private cdRef: ChangeDetectorRef,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!isNaN(id)) {
      this.categoryService.getCategories().subscribe({
        next: (all) => {
          const found = all.find((c) => c.id === id);
          if (found) {
            this.category = { ...found };
            this.cdRef.detectChanges();
          } else {
            this.error = 'Category not found';
          }
          this.loading = false;
        },
        error: () => {
          this.error = 'Error loading category';
          this.loading = false;
        },
      });
    } else {
      this.error = 'Invalid category ID';
      this.loading = false;
    }
  }

  updateCategory(): void {
    if (!this.category.name || !this.category.name.trim()) {
      alert('Name is required');
      return;
    }

    this.saving = true;
    this.categoryService.updateCategory(this.category).subscribe({
      next: () => {
        this.alert.success('Category updated successfully!');

        this.router.navigate(['/categories']);
      },
      error: (err) => {
        console.error('Update failed', err);
        this.alert.error('Failed to update category');

        this.saving = false;
      },
    });
  }

  deleteCategory(): void {
    console.log('dellll', this.category.id);
    // if (!confirm('Are you sure you want to delete this category?')) return;

    this.deleting = true;
    this.categoryService.deleteCategory(this.category.id).subscribe({
      next: () => {
        this.alert.success('Category Deleted Successfully');
        this.router.navigate(['/categories']);
      },
      error: (err) => {
        console.error('Delete failed', err);
        alert('Failed to delete category');
        this.deleting = false;
      },
    });
  }
}
