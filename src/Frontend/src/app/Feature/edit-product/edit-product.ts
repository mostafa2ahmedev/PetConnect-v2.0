import { CommonModule } from '@angular/common';
import { Component,inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SellerProductsModel } from '../seller-dashboard/seller-products-model';

@Component({
  selector: 'app-edit-product',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css'
})
export class EditProduct  implements OnInit {
  product:SellerProductsModel= {} as SellerProductsModel;
  fb = inject(FormBuilder);
  editProductGroup!: FormGroup;
  ngOnInit(): void {

    this.product= history.state.product;
    console.log(this.product)
      this.editProductGroup = this.fb.group({
    Name: new FormControl(this.product.productName),
    ProductTypeId: new FormControl(this.product.productType.id),
    Quantity: new FormControl(this.product.quantity),
    Price: new FormControl(this.product.price),
    Description: new FormControl(this.product.productDescription),
    Id:new FormControl(this.product.id)
    
  });
  console.log(this.editProductGroup.value);
  }


  onSubmit(){
    // Handle form submission
    }

    onFileSelected(event: any) {
      const file: File = event.target.files[0];
    }

    updateProduct(){

    }
    addProductGroup = this.fb.group({
      productName: [''],
      productTypeId: [''],
      quantity: [''],
      price: [''],
      description: ['']
    });
  }
