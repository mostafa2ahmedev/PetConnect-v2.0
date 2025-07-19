import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-customer-register-form',
  imports: [FormsModule,RouterLink],
  templateUrl: './customer-register-form.html',
  styleUrl: './customer-register-form.css'
})
export class CustomerRegisterForm {
customer = {
  fname: '', lname: '', email: '', phone: '', gender: '',
  password: '', confirmPassword: '',
  country: '', city: '', street: ''
};
  onSubmit() {
    // Handle form submission logic here
  }
}
