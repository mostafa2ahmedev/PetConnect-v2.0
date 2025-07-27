import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductTypeService } from '../product-type-service';
import { ProductType } from '../../../models/product-type';

@Component({
  selector: 'app-all-product-type',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './all-product-type.component.html',
  styleUrls: ['./all-product-type.component.css']
})
export class AllProductTypeComponent implements OnInit {
  productTypes: ProductType[] = [];
  loading = true;
  error = '';

  constructor(private productTypeService: ProductTypeService) {}

  ngOnInit(): void {
    this.productTypeService.getAll().subscribe({
      next: (data) => {
        this.productTypes = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load product types';
        this.loading = false;
      }
    });
  }
}
