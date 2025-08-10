// seller-dashboard.component.ts
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
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
import { AlertService } from '../../../core/services/alert-service';
import { Router } from '@angular/router';
import { SellerOrderProduct } from './seller-order-product';
import { OrderStatusEnum } from './order-status-enum';
import { OrderSellerAction } from './order-seller-action';



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
  changeDetectorRef = inject(ChangeDetectorRef);
  alertService = inject(AlertService);
  fb = inject(FormBuilder)
  router = inject(Router);

  orderStatusEnum!: OrderStatusEnum ;
  activeTab: string = 'products';
  seller: SellerModel= {}as SellerModel;
  products: Array<SellerProductsModel> =[];
  showAddForm: boolean = false;
  selectedImageFile: File | null = null;

  addProductGroup!:FormGroup;
  productTypes:any[]=[];
  orders:SellerOrderProduct[] = [];
  
  constructor() {

  }

  ngOnInit(): void {
    const userId = this.accountService.jwtTokenDecoder().userId
    this.sellerDashboardService.getSellerData().subscribe({next:res=>{
      this.seller=res.data;
    }})

    this.productTypeService.getAll().subscribe({next:res=>{
      this.productTypes= res;
    }}) 
    this.productsService.getProductsBySellerId(userId).subscribe({
      next: resp=>{
        this.products= resp.data
        console.log(this.products);
      }
    })
    this.sellerDashboardService.getSellerOrders().subscribe({
      next: resp=>{
        this.orders = resp.data;
        console.log(this.orders);
 
      }
    });

    this.addProductGroup = this.fb.group({
  Name: ['', Validators.required],
  Description: ['', Validators.required],
  Price: [0, [Validators.required, Validators.min(0)]],
  Quantity: [0, [Validators.required, Validators.min(0)]],
  ProductTypeId: [null, Validators.required] // Must be integer
});

  }

  get sellerName(): string {
    return `${this.seller.fName} ${this.seller.lName}`;
  }

  addProduct(): void {

    const formData = new FormData();
    const product = this.addProductGroup.value;

    for (const key in product) {
      if (product[key] === null || product[key] === undefined) 
        continue;
      formData.append(key, String(product[key] || ''));
    }
    if (this.selectedImageFile) {
    formData.append('ImgUrl', this.selectedImageFile);
    }

    for (const [key, value] of formData.entries()) {
    console.log(`${key}:`, value);
    }

    this.productsService.addWithImage(formData).subscribe({next:resp=>{
      this.alertService.success('Product added successfully');
      this.showAddForm = false;
      this.getToSellerDashboard()
    },
  error: err=>{
    console.log(formData.values())
    this.alertService.error('Failed to add product');
  }})
  }

  editProduct(product: SellerProductsModel): void {
    this.router.navigateByUrl('products/edit', { state: { product } });
    

  }

  deleteProduct(productId: number): void {
    this.sellerDashboardService.deleteProduct(productId).subscribe({
      next: res => {
        this.alertService.success('Product deleted successfully');
        this.getToSellerDashboard()
      },
      error: err => {
        this.alertService.error('Failed to delete product');
      }
    });
  }

  cancelAdd(): void {
    this.resetForm();
    this.showAddForm = false;
  }

  private resetForm(): void {

  }
  onFileSelected(event: any) {
  const file = event.target.files[0];
  if (!file) {
    return;
  }

  // Example: validate file type and size
  const allowedTypes = ['image/png', 'image/jpeg'];
  if (!allowedTypes.includes(file.type)) {
    // this.onImageError('Only JPEG and PNG images are allowed.');
  } else if (file.size > 2 * 1024 * 1024) { // 2MB limit
    // this.onImageError('Image must be less than 2MB.');
  } else {
    // this.onImageError('');
    // Save the file to a property for submission
    this.selectedImageFile = file;
  }
}


  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending': return 'status-pending';
      case 'Shipped': return 'status-shipped';
      case 'Deny': return 'status-deny';
      default: return 'status-default';
    }
  }

  onImageError(event: any): void {
    event.target.style.display = 'none';
    event.target.nextElementSibling.style.display = 'flex';
  }

    getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102/assets/ProductImages/${relativePath}`;
  }

  getOrderStatusEnum(status:number){
   return OrderStatusEnum[status] 
  }
  shipOrder(orderAction:OrderSellerAction){
    orderAction.orderProductStatus= 1 ;
    
    this.sellerDashboardService.changeOrderStatus(orderAction).subscribe({next:resp=>{
      this.alertService.success(resp.data);
      this.getToSellerDashboardOrder();
      this.activeTab='orders';
    }});
  }

    denyOrder(orderAction:OrderSellerAction){
    orderAction.orderProductStatus= 2 ;
    this.sellerDashboardService.changeOrderStatus(orderAction).subscribe({next:resp=>{
      this.alertService.success(resp.data);
            this.getToSellerDashboardOrder();
    }});
  }
  getProfileImage(fileName:string){
    return `https://localhost:7102${fileName}`;
    
  }
  getToSellerDashboard(){
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
  this.router.navigate(['seller']);
});
  }
    getToSellerDashboardOrder(){
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
  this.router.navigate(['seller']);
  this.activeTab='orders'
});
  }

  addToStock(product: SellerProductsModel): void {
  const qtyToAdd = Number(product.addQuantity);

  if (qtyToAdd > 0) {

    this.alertService.confirm(`are you sure you want to add ${product.quantity} to ${product.productName}?`).then(confirmed=>{
      if(confirmed){
        product.quantity += qtyToAdd;

        const mappedProduct = {Id: product.id,Quantity: product.quantity,
          Description:product.productDescription,
        Price:product.price,ProductTypeId:product.productType.id}
        this.productsService.update(mappedProduct).subscribe({next:
          rep=>{
            this.alertService.success("Added successfully")
            this.getToSellerDashboard();
          }
        })

      }
    })

    // this.productService.updateStock(product.id, product.quantity).subscribe(...)
  }
}
}