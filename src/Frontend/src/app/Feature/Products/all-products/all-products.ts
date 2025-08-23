import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth';
import { AlertService } from '../../../core/services/alert-service';
import { CartService } from '../../cart/cart-service';
@Component({
  selector: "app-products",
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: "./all-products.html",
  styleUrls: ["./all-products.css"],
})
export class ProductsComponent implements OnInit {
  products: any[] = []
  filteredProducts: any[] = []
  paginatedProducts: any[] = []
  currentPage = 1
  itemsPerPage = 8
  totalPages = 0

  filterType = ""
  filterExactPrice: number | null = null
  filterName = ""
  minPrice: number | null = null
  maxPrice: number | null = null
  productTypes: string[] = []
  loadingProducts = true
  server = "https://localhost:7102"
  showFilters = false

  Math = Math

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private changeDetectorRef: ChangeDetectorRef,
    private cartService: CartService,
  ) {}

  ngOnInit(): void {
    this.http.get<any[]>("https://localhost:7102/api/Product").subscribe({
      next: (res) => {
        this.products = res
        this.filteredProducts = res
        this.productTypes = [...new Set(res.map((p) => p.productTypeName))]
        this.loadingProducts = false
        this.updatePagination()
      },
      error: (err) => {
        this.alertService.error("Failed to load products. Please try again later.")
      },
    })
  }

  onFilterChange() {
    this.applyFilters()
  }

  toggleFilters() {
    this.showFilters = !this.showFilters
  }

  applyFilters() {
    this.filteredProducts = this.products.filter((product) => {
      const matchesName = this.filterName === "" || product.name.toLowerCase().includes(this.filterName.toLowerCase())
      const matchesType = this.filterType === "" || product.productTypeName === this.filterType
      const matchesMinPrice = this.minPrice === null || product.price >= this.minPrice
      const matchesMaxPrice = this.maxPrice === null || product.price <= this.maxPrice

      return matchesName && matchesType && matchesMinPrice && matchesMaxPrice
    })
    this.currentPage = 1
    this.updatePagination()
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredProducts.length / this.itemsPerPage)
    const startIndex = (this.currentPage - 1) * this.itemsPerPage
    const endIndex = startIndex + this.itemsPerPage
    this.paginatedProducts = this.filteredProducts.slice(startIndex, endIndex)
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page
      this.updatePagination()
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++
      this.updatePagination()
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--
      this.updatePagination()
    }
  }

  clearFilters() {
    this.filterName = ""
    this.filterType = ""
    this.minPrice = null
    this.maxPrice = null
    this.filteredProducts = [...this.products]
    this.currentPage = 1
    this.updatePagination()
  }

  addToCart(product: any) {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(["/login"])
    } else {
      this.cartService.addToCart(product)
      this.alertService.success("Product added to cart!")
      this.changeDetectorRef.detectChanges()
    }
  }
}
