import { Component } from '@angular/core';
import { Customer } from "./customer/customer";
import { IDoctor } from '../doctors/idoctor';
import { CustomerDto } from './customer-dto';
import { Doctor } from './doctor/doctor';

@Component({
  selector: 'app-profile',
  imports: [Customer,Doctor],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
  passedDoctor:IDoctor = history.state.doctor ;
  passedCustomer:CustomerDto = history.state.customer ;
}
