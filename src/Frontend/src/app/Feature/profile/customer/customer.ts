import { Component, input } from '@angular/core';
import { CustomerDto } from '../customer-dto';

@Component({
  selector: 'app-customer',
  imports: [],
  templateUrl: './customer.html',
  styleUrl: './customer.css'
})
export class Customer {
server = "https://localhost:7102";
customer = input<CustomerDto | undefined>();
}
