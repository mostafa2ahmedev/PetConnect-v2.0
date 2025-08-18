// src/app/Feature/product-type/add-product-type.ts
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ProductTypeService } from '../product-type-service';
import { BreedService } from '../../breeds/breed-service';

@Component({
  selector: 'app-add-product-type',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , RouterModule],
  templateUrl: './add-product-type.html',
  styleUrls: ['./add-product-type.css'],
})
export class AddProductTypeComponent implements OnInit {
  form!: FormGroup;
  breeds: any[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private service: ProductTypeService,
    private breedService: BreedService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      breedId: [null, Validators.required],
    });

    this.breedService.getAllBreeds().subscribe({
      next: (data) => (this.breeds = data),
      error: (err) => console.error('Error loading breeds:', err),
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.service.add(this.form.value).subscribe({
        next: () => this.router.navigate(['/product-type/all']),
        error: (err) => console.error('Error adding product type:', err),
      });
    }
  }
}
