import {
  AfterViewInit,
  Component,
  computed,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { DoctorsService } from './doctors-service';
import { FormsModule } from '@angular/forms';
import { IDoctor } from './idoctor';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

import { ISpeciality } from './ispeciality';
import { AccountService } from '../../core/services/account-service';
import { ReviewsService } from '../Review/reviews-service';

@Component({
  selector: 'app-doctors',
  imports: [FormsModule, CurrencyPipe, RouterLink, CommonModule],
  templateUrl: './doctors.html',
  styleUrl: './doctors.css',
})
export class Doctors implements OnInit {
  server = 'https://localhost:7102';
  doctorsAreLoading = signal(true);
  doctorService = inject(DoctorsService);
  accounterService = inject(AccountService);
  reviewService = inject(ReviewsService);
  router = inject(Router);
  name: string = '';
  maxPrice: number | null = null;
  allDoctors: string | IDoctor[] = [];
  specialty: number | null = 0;
  specialities: ISpeciality[] = [
    { specialityName: 'Dog', value: 0 },
    { specialityName: 'Cat', value: 1 },
    { specialityName: 'Bird', value: 2 },
  ];
  city: string = '';
  isCustomerLogginIn = false;
  errorFound: boolean = false;
  errorMesseage: string = '';
  ngOnInit() {
    this.search();
    this.isCustomerLogginIn = this.accounterService.isCustomer();
  }

  search() {
    this.doctorService
      .getAll(this.name, this.maxPrice, this.specialty, this.city)
      .subscribe({
        next: (e) => {
          this.errorFound = false;
          this.allDoctors = e;
          this.doctorsAreLoading.set(false);
        },
        error: (err) => {
          this.errorFound = true;
          this.errorMesseage = err?.error;
          console.log(this.errorMesseage);
        },
      });
  }
  viewReviews(docId: string) {
    this.router.navigate(['/doctors', 'review'], {
      state: { doctorId: docId },
    });
  }

  getFullStars(rating: number): any[] {
  return Array(Math.floor(rating));
}

getEmptyStars(rating: number): any[] {
  return Array(5 - Math.floor(rating));
}
}
