import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '../../models/api-response';
import { DoctorReviewModel } from './models/doctor-review-model';
import { CreateCustomerReviewDoctorDTO } from './models/create-customer-review-doctor-dto';
import { CustomerProfileListOfReviewsDTO } from './models/customer-profile-list-of-reviews-dto';

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {
  httpClient = inject(HttpClient);
  server="https://localhost:7102";
  getDoctorReviewsById(doctorId: string){
   return this.httpClient.get<ApiResponse<DoctorReviewModel[]>>(`${this.server}/api/Reviews/by-doctor/${doctorId}`);
  }
  canWriteReview(appointmentId:string){
    return this.httpClient.post(`${this.server}/api/Reviews/IsReviewable`,{appointmentId});
  }

  WriteReview(reviewDTO:CreateCustomerReviewDoctorDTO ){
    return this.httpClient.post<ApiResponse<string>>(`${this.server}/api/Reviews`,reviewDTO);
  }
  getCustomerReviewsById(custId: string){
    return this.httpClient.get<ApiResponse<CustomerProfileListOfReviewsDTO[]>>(`${this.server}/api/Reviews/by-customer/${custId}`);
  }
  deleteReviewById(reviewId:string){
    return this.httpClient.delete<ApiResponse<string>>(`${this.server}/api/Reviews/${reviewId}`);
  }
}
