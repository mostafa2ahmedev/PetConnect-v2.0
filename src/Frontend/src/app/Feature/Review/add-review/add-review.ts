import { Component, inject, OnInit } from '@angular/core';
import { ReviewMainPage } from "../review-main-page/review-main-page";
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomerDto } from '../../profile/customer-dto';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-add-review',
  imports: [ReviewMainPage,FormsModule,CommonModule],
  templateUrl: './add-review.html',
  styleUrl: './add-review.css'
})
export class AddReview implements OnInit{
  customerData!: CustomerDto;
  accountService =inject(AccountService)
  ngOnInit(): void {
    this.accountService.getCustomerData().subscribe({
      next: resp=>{
        this.customerData=resp.data
      }
    })
  }
  starRating: number = 0;
setRating(rating: number): void {
    this.starRating = rating;
  }

  onSubmit(form: NgForm): void {
    if (form.valid) {
      const newReview: any = {
        name: `${this.customerData.fName} ${this.customerData.lName}`,
        role: `Customer`,
        appointmentId: history.state.appointmentId,
        imgSrc: `${this.customerData.imgUrl}` || 'https://placehold.co/100x100/1e40af/ffffff?text=User',
        content: form.value.content,
        rating: (this.starRating<=5 && this.starRating>=1) ? this.starRating : 0 
      };
      
      console.log(newReview)
      
      // Reset the form
      form.resetForm();
      this.starRating = 0;
    }
}
}
