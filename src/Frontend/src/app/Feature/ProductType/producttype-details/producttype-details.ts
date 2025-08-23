import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductTypeService } from '../product-type-service';

@Component({
  selector: 'app-product-type-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './producttype-details.html',
  styleUrls: ['./producttype-details.css'],
})
export class ProductTypeDetailsComponent implements OnInit {
  id!: number;
  productType: any;

  constructor(
    private route: ActivatedRoute,
    private service: ProductTypeService
  ) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.service.getById(this.id).subscribe({
      next: (data) => (this.productType = data),
      error: (err) => console.error('Error loading product type:', err),
    });
  }
}
