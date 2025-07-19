import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-doctor-register-form',
  imports: [FormsModule,RouterLink],
  templateUrl: './doctor-register-form.html',
  styleUrl: './doctor-register-form.css'
})
export class DoctorRegisterForm {
  doctor= {gender:"",fname:"", lname:"" , password:"",
    pricePerHour:0, petSpecialty:"", email:"", phoneNumber:"", 
    country:"", city:"", street:"",certificateUrl:"",imageUrl:"", confirmPassword: ''
    ,role:"Doctor"};
  onSubmit(){}
}
