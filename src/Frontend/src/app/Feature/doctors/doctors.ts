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
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ISpeciality } from './ispeciality';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-doctors',
  imports: [FormsModule, CurrencyPipe, RouterLink],
  templateUrl: './doctors.html',
  styleUrl: './doctors.css',
})
export class Doctors implements OnInit {
  server = 'https://localhost:7102';
  doctorsAreLoading = signal(true);
  doctorService = inject(DoctorsService);
  accounterService = inject(AccountService);
  name: string = '';
  maxPrice: number | null = null;
  allDoctors: string | IDoctor[] = [];
  specialty: number | null = 0;
  specialities: ISpeciality[] = [
    { specialityName: 'Dog', value: 1 },
    { specialityName: 'Cat', value: 2 },
  ];
  isCustomerLogginIn = false;
  errorFound: boolean = false;
  errorMesseage: string = '';
  ngOnInit() {
    this.search();
    this.isCustomerLogginIn = this.accounterService.isCustomer();
  }

  search() {
    this.doctorService
      .getAll(this.name, this.maxPrice, this.specialty)
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
}
