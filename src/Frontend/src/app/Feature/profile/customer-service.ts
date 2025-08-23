import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerDto } from './customer-dto';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  httpClient = inject(HttpClient);
  server= 'https://localhost:7102'
  getCustomerById(customerId:string):Observable<{statusCode:number,data:CustomerDto}>{
    return this.httpClient.get<{statusCode:number,data:CustomerDto}>(`${this.server}/api/Customer/${customerId}`);
  }
}
