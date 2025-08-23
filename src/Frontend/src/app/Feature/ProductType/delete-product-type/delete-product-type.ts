import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductTypeService } from '../product-type-service';

@Component({
  selector: 'app-delete-product-type',
  standalone: true,
  imports: [CommonModule , RouterModule],
  templateUrl: './delete-product-type.html',
  styleUrls: ['./delete-product-type.css'],
})
export class DeleteProductTypeComponent implements OnInit {
  id!: number;

  constructor(
    private route: ActivatedRoute,
    private service: ProductTypeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.service.delete(this.id).subscribe({
      next: () => this.router.navigate(['/product-type/all']),
      error: (err) => console.error('Error deleting product type:', err),
    });
  }
}
