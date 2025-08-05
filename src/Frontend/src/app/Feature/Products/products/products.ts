import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-products',
 standalone: true,
  templateUrl: './products.html',
  imports: [FormsModule , CommonModule],
  styleUrls: ['./products.css']
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  categories = ['Wooden Gift Boxes', 'Candles', 'Decor'];

  newProduct: any;
  isEditing = false;
  editingProductId: number | null = null;

  seller: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    const sellerData = localStorage.getItem('user') || sessionStorage.getItem('user');
    if (sellerData) {
  this.seller = JSON.parse(sellerData); 
  this.getProducts();
  this.newProduct = this.getEmptyProduct();
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
      sellerId: this.seller?.id || '',
      sellerName: this.seller?.name || ''
    };
  }

  getProducts() {
    this.http.get<any[]>(`https://localhost:7102/products?sellerId=${this.seller.id}`)
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
    this.http.post('https://localhost:7102/products', this.newProduct)
      .subscribe(() => {
        this.getProducts();
        this.newProduct = this.getEmptyProduct();
      });
  }

  deleteProduct(id: number) {
    this.http.delete(`https://localhost:7102/products/${id}`)
      .subscribe(() => this.getProducts());
  }

  editProduct(product: any) {
    this.newProduct = { ...product };
    this.isEditing = true;
    this.editingProductId = product.id;
  }

  saveChanges() {
    if (this.editingProductId !== null) {
      this.http.put(`https://localhost:7102/products/${this.editingProductId}`, this.newProduct)
        .subscribe(() => {
          this.getProducts();
          this.newProduct = this.getEmptyProduct();
          this.isEditing = false;
          this.editingProductId = null;
        });
    }
  }
}
