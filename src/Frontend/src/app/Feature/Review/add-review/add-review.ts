import { Component, inject, OnInit } from '@angular/core';
import { ReviewMainPage } from "../review-main-page/review-main-page";
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomerDto } from '../../profile/customer-dto';
import { AccountService } from '../../../core/services/account-service';
import { ReviewsService } from '../reviews-service';
import { AlertService } from '../../../core/services/alert-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-review',
  imports: [FormsModule,CommonModule],
  templateUrl: './add-review.html',
  styleUrl: './add-review.css'
})
export class AddReview implements OnInit{
  customerData!: CustomerDto;
  accountService =inject(AccountService);
  reviewService=inject(ReviewsService);
  alertService=inject(AlertService)
  router = inject(Router)
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
        // name: `${this.customerData.fName} ${this.customerData.lName}`,
        // role: `Customer`,
        customerId: this.customerData.customerId,
        appointmentId: history.state.appointmentId,
        doctorId:history.state.doctorId,
        // imgSrc: `${this.customerData.imgUrl}` || 'https://placehold.co/100x100/1e40af/ffffff?text=User',
        content: form.value.content,
        rating: (this.starRating<=5 && this.starRating>=1) ? this.starRating : 0 
      };
      
      console.log(newReview)
      this.reviewService.WriteReview(newReview).subscribe({
        next:resp=>{
          this.alertService.success(resp.data);
          this.router.navigate(['/profile','reviews'])
        }
        ,
        error: err=>{
          this.alertService.error(err.data);
        }
      })
      // Reset the form
      // form.resetForm();
      this.starRating = 0;
    }
}
}
