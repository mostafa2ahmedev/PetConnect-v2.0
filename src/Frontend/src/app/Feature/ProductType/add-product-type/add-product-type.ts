import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ProductTypeService } from '../product-type-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-product-type',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-product-type.html',
  styleUrls: ['./add-product-type.css'],
})
export class AddProductTypeComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private productTypeService: ProductTypeService,
    private router: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      breedId: [null, Validators.required],
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.productTypeService.add(this.form.value).subscribe({
        next: () => this.router.navigate(['/product-type/all']),
        error: (err) => console.error('Error adding product type:', err),
      });
    }
  }
}
