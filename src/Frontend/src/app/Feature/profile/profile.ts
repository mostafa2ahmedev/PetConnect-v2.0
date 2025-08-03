import { Component, inject, OnInit } from '@angular/core';
import { Customer } from './customer/customer';
import { IDoctor } from '../doctors/idoctor';
import { CustomerDto } from './customer-dto';
import { Doctor } from './doctor/doctor';
import { AccountService } from '../../core/services/account-service';
import { DoctorsService } from '../doctors/doctors-service';

@Component({
  selector: 'app-profile',
  imports: [],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  // passedDoctor:IDoctor = history.state.doctor ;
  accountService = inject(AccountService);
  doctorService = inject(DoctorsService);
  passedDoctor!: IDoctor;
  passedCustomer: CustomerDto = history.state.customer;

  ngOnInit(): void {
    const user = this.accountService.jwtTokenDecoder();
    this.doctorService.getById(user.userId).subscribe({
      next: (resp) => {
        if (typeof resp !== 'string') this.passedDoctor = resp;
      },
    });
  }
}
