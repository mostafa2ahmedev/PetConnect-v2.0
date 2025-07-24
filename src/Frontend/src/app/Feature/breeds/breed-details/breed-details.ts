import { Component } from '@angular/core';
import { Breed } from '../../../models/breed';
import { Category } from '../../../models/category';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BreedService } from '../breed-service';
import { CategoryService } from '../../categories/category-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-breed-details',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './breed-details.html',
  styleUrl: './breed-details.css',
})
export class BreedDetails {
  breed: Breed = { id: 0, name: '', categoryId: 0, categoryName: '' };
  categories: Category[] = [];
  error = '';
  saving = false;
  deleting = false;
  loading = true;
  fieldErrors: { [key: string]: string[] } = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private breedService: BreedService,
    private categoryService: CategoryService,
    public alert: AlertService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadCategories();
    if (!isNaN(id)) {
      // Load categories first
      this.breedService.getBreedById(id).subscribe({
        next: (res) => {
          this.breed = res;

          // 🔧 Try to find the category ID using the name
          const matched = this.categories.find(
            (c) => c.name === this.breed.categoryName
          );
          if (matched) {
            this.breed.categoryId = matched.id;
          }
          console.log('Breed loaded:', this.breed);
          this.loading = false;
        },
        error: () => {
          this.error = 'Failed to load breed.';
          this.loading = false;
        },
      });
    } else {
      this.error = 'Invalid breed ID.';
      this.loading = false;
    }
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (cats) => (this.categories = cats),
      error: () => (this.error = 'Failed to load categories.'),
    });
  }

  updateBreed(): void {
    this.fieldErrors = {};
    // const nameTrimmed = this.breed.name?.trim();

    // if (!nameTrimmed) {
    //   alert('Please enter a breed name');
    //   return;
    // }

    this.saving = true;
    this.breedService.updateBreed(this.breed).subscribe({
      next: () => {
        this.alert.success('Breed updated successfully');
        this.router.navigate(['/breeds']);
      },
      error: (err) => {
        console.error('Update failed:', err);
        this.saving = false;
        if (err.status === 400 && err.error?.data) {
          this.fieldErrors = err.error.data;
        } else {
          this.alert.error('Failed to update breed', 'Error');
        }
      },
    });
  }

  async deleteBreed(): Promise<void> {
    const confirmed = await this.alert.confirm(
      'Are you sure you want to delete this breed?',
      'Delete Confirmation',
      'Yes, delete it',
      'Cancel'
    );

    if (!confirmed) return;

    this.deleting = true;
    this.breedService.deleteBreed(this.breed.id).subscribe({
      next: () => {
        this.alert.success('Breed deleted successfully');
        this.router.navigate(['/breeds']);
      },
      error: (err) => {
        console.error('Delete failed:', err);
        this.alert.error('Failed to delete breed', 'Error');
        this.deleting = false;
      },
    });
  }
}
