// seller-dashboard.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { SellerModel } from '../../seller-dashboard/seller-model';
import { SellerProductsModel } from '../../seller-dashboard/seller-products-model';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SellerDashboardService } from '../../seller-dashboard/seller-dashboard-service';
import { AccountService } from '../../../core/services/account-service';
import { ProductTypeService } from '../../ProductType/product-type-service';
import { ProductType } from '../../../models/product-type';
import { ProductService } from '../../Products/product-service';
import { Product } from '../../../models/product';

interface OrderModel {
  id: string;
  customerName: string;
  productName: string;
  quantity: number;
  total: number;
  status: string;
  date: string;
}

@Component({
  selector: 'app-dashboard',
  imports:[FormsModule,ReactiveFormsModule,CommonModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard implements OnInit {
  sellerDashboardService=inject(SellerDashboardService);
  accountService = inject(AccountService);
  productTypeService = inject(ProductTypeService)
  productsService = inject(ProductService)
  fb = inject(FormBuilder)

  activeTab: string = 'products';
  seller: SellerModel= {}as SellerModel;
  // products: SellerProductsModel[] = [];
  products: Array<Product> =[];
  orders: OrderModel[] = [];
  showAddForm: boolean = false;
  
  // newProduct: Product = {
  //   name: '',
  //   productTypeId: 1,
  //   quantity: 0,
  //   price: 0,
  //   imgUrl: ,
  //   description: ''
  // };
  addProductGroup!:FormGroup;
  // productTypes = ['Electronics', 'Clothing', 'Home', 'Books', 'Sports'];
  productTypes:any[]=[];
  constructor() {
    // Mock data
    // this.seller = {
    //   fName: 'John',
    //   lName: 'Smith',
    //   imgUrl: 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=100&h=100&fit=crop&crop=face',
    //   gender: 0,
    //   street: '123 Main St',
    //   city: 'New York',
    //   country: 'USA'
    // };

    // this.products = [
    //   {
    //     sellerId: 'seller-1',
    //     productName: 'Wireless Headphones',
    //     productType: {id:1,name:"hi"},
    //     quantity: 25,
    //     price: 99.99,
    //     imgUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=200&h=200&fit=crop',
    //     productDescription: 'High-quality wireless headphones with noise cancellation'
    //   },
    //   {
    //     sellerId: 'seller-1',
    //     productName: 'Cotton T-Shirt',
    // productType: {id:1,name:"hi"},
    //     quantity: 50,
    //     price: 19.99,
    //     imgUrl: 'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=200&h=200&fit=crop',
    //     productDescription: '100% cotton comfortable t-shirt'
    //   },
    //   {
    //     sellerId: 'seller-1',
    //     productName: 'Coffee Mug',
    // productType: {id:1,name:"hi"},
    //     quantity: 15,
    //     price: 12.50,
    //     imgUrl: 'https://images.unsplash.com/photo-1514228742587-6b1558fcf93a?w=200&h=200&fit=crop',
    //     productDescription: 'Ceramic coffee mug with custom design'
    //   }
    // ];
    // this.products= []
    this.orders = [
      {
        id: 'ORD-001',
        customerName: 'Alice Johnson',
        productName: 'Wireless Headphones',
        quantity: 2,
        total: 199.98,
        status: 'Processing',
        date: '2024-01-15'
      },
      {
        id: 'ORD-002',
        customerName: 'Bob Wilson',
        productName: 'Cotton T-Shirt',
        quantity: 3,
        total: 59.97,
        status: 'Shipped',
        date: '2024-01-14'
      },
      {
        id: 'ORD-003',
        customerName: 'Carol Davis',
        productName: 'Coffee Mug',
        quantity: 1,
        total: 12.50,
        status: 'Delivered',
        date: '2024-01-13'
      }
    ];
  }

  ngOnInit(): void {
    const userId = this.accountService.jwtTokenDecoder().userId
    this.sellerDashboardService.getSellerData().subscribe({next:res=>{
      this.seller=res.data;
    }})

    this.productTypeService.getAll().subscribe({next:res=>{
      this.productTypes= res;
      console.log(res);
    }}) 
    this.productsService.getProductsBySellerId(userId).subscribe({
      next: resp=>{
        console.log(resp);
        this.products= resp
      }
    })
    // this.addProductGroup = this.fb.group({
    //       Name: ['', Validators.required],
    // ProductTypeId: ['', Validators.required],
    // Quantity: [0, [Validators.required, Validators.min(0)]],
    // Price: [0, [Validators.required, Validators.min(0)]],
    // ImgUrl: [''],
    // Description: ['']
    // })
    this.addProductGroup = this.fb.group({
  Name: ['', Validators.required],
  Description: ['', Validators.required],
  ImgUrl: [null, Validators.required], // Use null for file input
  Price: [0, [Validators.required, Validators.min(0)]],
  Quantity: [0, [Validators.required, Validators.min(0)]],
  ProductTypeId: [null, Validators.required] // Must be integer
});

  }

  get sellerName(): string {
    return `${this.seller.fName} ${this.seller.lName}`;
  }

  addProduct(): void {
    // if (this.isValidProduct()) {
    //   this.products.push({ ...this.newProduct });
    //   this.resetForm();
    //   this.showAddForm = false;
    // }
      // const formData = new FormData();

  // Add basic fields
  // formData.append('Name', this.newProduct.productName);
  // formData.append('ProductTypeId', String(this.newProduct.productType)); // ensure it's a string
  // formData.append('Quantity', String(this.newProduct.quantity));
  // formData.append('Price', String(this.newProduct.price));
  // formData.append('ImgUrl', this.newProduct.imgUrl || '');
  // formData.append('Description', this.newProduct.productDescription || '');

  // Send to backend (example using HttpClient)
    const formData = new FormData();
    const product = this.addProductGroup.value;

    for (const key in product) {
      formData.append(key, String(product[key] || ''));
    }

    //   if (this.addProductGroup.valid) {
    // const formValues = this.addProductGroup.value;
    // const formData = new FormData();

    // formData.append('Name', formValues.Name);
    // formData.append('Description', formValues.Description);
    // formData.append('Price', formValues.Price.toString());
    // formData.append('Quantity', formValues.Quantity.toString());
    // formData.append('ProductTypeId', formValues.ProductTypeId.toString());
    // if (formValues.ImgUrl instanceof File) {
    //   formData.append('ImgUrl', formValues.ImgUrl);
    // }
    this.productsService.addWithImage(formData).subscribe({next:resp=>{
      console.log(resp);
      console.log("success");
      
    },
  error: err=>{
    console.log(formData.values())
    console.log(err)
  }})
  }

  editProduct(product: Product): void {
    // In a real app, this would open an edit modal/form
    console.log('Edit product:', product);
  }

  deleteProduct(index: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.products.splice(index, 1);
    }
  }

  cancelAdd(): void {
    this.resetForm();
    this.showAddForm = false;
  }

  private resetForm(): void {
    // this.newProduct = {
    //   sellerId: 'seller-1',
    //   productName: '',
    // productType: {id:1,name:"hi"},
    //   quantity: 0,
    //   price: 0,
    //   imgUrl: '',
    //   productDescription: ''
    // };
  }
onFileSelected(event: any) {
  if (event.target.files.length > 0) {
    const file = event.target.files[0];
    console.log(file);
    this.addProductGroup.patchValue({ ImgUrl: file });
  }
}

  // private isValidProduct(): boolean {
  //   return !!(
  //     // this.newProduct.productName.trim() &&
  //     // this.newProduct.quantity >= 0 &&
  //     // this.newProduct.price > 0
  //   );
  // }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Processing': return 'status-processing';
      case 'Shipped': return 'status-shipped';
      case 'Delivered': return 'status-delivered';
      default: return 'status-default';
    }
  }

  onImageError(event: any): void {
    event.target.style.display = 'none';
    event.target.nextElementSibling.style.display = 'flex';
  }
}