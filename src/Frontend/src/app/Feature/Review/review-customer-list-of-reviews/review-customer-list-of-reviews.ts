import { Component, inject, type OnInit } from "@angular/core"
import { AccountService } from "../../../core/services/account-service"
import { CommonModule, DatePipe } from "@angular/common"
import { ReviewsService } from "../reviews-service"
import type { CustomerProfileListOfReviewsDTO } from "../models/customer-profile-list-of-reviews-dto"
import { AlertService } from "../../../core/services/alert-service"
import { Router } from "@angular/router"

@Component({
  selector: "app-review-customer-list-of-reviews",
  imports: [DatePipe, CommonModule],
  templateUrl: "./review-customer-list-of-reviews.html",
  styleUrl: "./review-customer-list-of-reviews.css",
})
export class ReviewCustomerListOfReviews implements OnInit {
  accountService = inject(AccountService)
  reviewService = inject(ReviewsService)
  alertService = inject(AlertService)
  router = inject(Router)
  customerReviews: CustomerProfileListOfReviewsDTO[] = []
  customerId = ""
  customerName = ""
  ngOnInit(): void {
    this.customerId = this.accountService.jwtTokenDecoder().userId
    this.accountService.getCustomerData().subscribe({
      next: (resp) => {
        this.customerName = `${resp.data.fName} ${resp.data.lName}`
      },
    })
    this.reviewService.getCustomerReviewsById(this.customerId).subscribe({
      next: (resp) => {
        this.customerReviews = resp.data
      },
    })
  }
  deleteReview(reviewId: string) {
    this.reviewService.deleteReviewById(reviewId).subscribe({
      next: (resp) => {
        this.alertService.success("Deleted Successfully")
        this.router.navigate(["/profile"])
      },
      error: (err) => {
        this.alertService.error("Something Wrong Happened")
      },
    })
  }
}
