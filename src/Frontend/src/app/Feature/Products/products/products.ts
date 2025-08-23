import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';
import { SellerDashboardService } from '../../seller-dashboard/seller-dashboard-service';
import { SellerModel } from '../../seller-dashboard/seller-model';
import { ProductService } from '../product-service';


@Component({
  selector: 'app-products',
 standalone: true,
  templateUrl: './products.html',
  imports: [FormsModule , CommonModule],
  styleUrls: ['./products.css']
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  sellerDashBoardServ= inject(SellerDashboardService);
  productService = inject(ProductService)
  categories = ['Wooden Gift Boxes', 'Candles', 'Decor'];

  newProduct: any;
  isEditing = false;
  editingProductId: number | null = null;

  seller!: SellerModel;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const sellerData = localStorage.getItem('user') || sessionStorage.getItem('user');
    if (sellerData) {
  // this.seller = JSON.parse(sellerData); 
   this.sellerDashBoardServ.getSellerData().subscribe({next: e=>this.seller=e.data});
  this.getProducts();
  // this.newProduct = this.getEmptyProduct();
}

  }

  getEmptyProduct() {
    return {
      id: 0,
      name: '',
      description: '',
      imgUrl: '',
      price: 0,
      quantity: 1,
      productTypeId: 0,
      productTypeName: '',
      // sellerId: this.seller?.id || '',
      // sellerName: this.seller?.name || ''
    };
  }

  getProducts() {
    // this.http.get<any[]>(`https://localhost:7102/products?sellerId=${this.seller.id}`)
    //   .subscribe(data => this.products = data);
      this.http.get<any[]>(`https://localhost:7102/Seller/Products`)
      .subscribe(data => this.products = data);
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.onload = () => {
      this.newProduct.imgUrl = reader.result as string;
    };
    if (file) reader.readAsDataURL(file);
  }

  addProduct() {
    // this.http.post('https://localhost:7102/product', this.newProduct)
    //   .subscribe(() => {
    //     this.getProducts();
    //     this.newProduct = this.getEmptyProduct();
    //   });
    this.productService.add(this.newProduct);
  }

  deleteProduct(id: number) {
    this.http.delete(`https://localhost:7102/product/${id}`)
      .subscribe(() => this.getProducts());
  }

  editProduct(product: any) {
    this.newProduct = { ...product };
    this.isEditing = true;
    this.editingProductId = product.id;
  }

  saveChanges() {
    if (this.editingProductId !== null) {
      this.http.put(`https://localhost:7102/product/${this.editingProductId}`, this.newProduct)
        .subscribe(() => {
          this.getProducts();
          this.newProduct = this.getEmptyProduct();
          this.isEditing = false;
          this.editingProductId = null;
        });
    }
  }
}
