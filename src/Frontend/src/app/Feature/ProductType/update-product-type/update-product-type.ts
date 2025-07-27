import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductTypeService } from '../product-type-service';
import { BreedService } from '../../breeds/breed-service';
import { Breed } from '../../../models/breed';

@Component({
  selector: 'app-update-product-type',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './update-product-type.html',
  styleUrls: ['./update-product-type.css'],
})
export class UpdateProductTypeComponent implements OnInit {
  form!: FormGroup;
  id!: number;
  breeds: any[] = [];
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private service: ProductTypeService,
    private router: Router,
    private breedService: BreedService
  ) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.form = this.fb.group({
      name: ['', Validators.required],
      breedId: [null, Validators.required],
    });
    this.breedService.getAllBreeds().subscribe({
      next: (data) => (this.breeds = data),
      error: (err) => console.error('Error loading breeds:', err),
    });

    this.service.getById(this.id).subscribe({
      next: (data) => {
        console.log('ProductType data from API:', data);

        // this.form.patchValue({
        //   name: data.name,
        //   breedId: data.BreedId
        // });
      },
      error: (err) => console.error('Error loading product type:', err),
    });
  }

  onSubmit(): void {
    if (this.form.valid) {
      const updatedData = this.form.value;
      updatedData.id = this.id;

      this.service.update(this.id, updatedData).subscribe({
        next: () => this.router.navigate(['/product-type/all']),
        error: (err) => console.error('Error updating product type:', err),
      });
    }
  }
}
