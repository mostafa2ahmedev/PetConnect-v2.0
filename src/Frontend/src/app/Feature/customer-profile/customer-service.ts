import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Pet } from '../../models/pet';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CusotmerPet } from '../../models/cusotmer-pet';
import { CustomerPofileDetails } from '../../models/customer-pofile-details';
import { UpdateCustomerProfileRequest } from '../../models/update-customer-profile-request';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private readonly baseUrl = environment.apiBaseUrl + '/customer';

  constructor(private http: HttpClient) {}

  getCustomerPets(): Observable<CusotmerPet[]> {
    return this.http
      .get<any>(`${this.baseUrl}/CustomerPets`)
      .pipe(map((response) => response.data as CusotmerPet[]));
  }
  getCustomerProfile(): Observable<CustomerPofileDetails> {
    return this.http
      .get<any>(`${this.baseUrl}/Profile`)
      .pipe(map((response) => response.data as CustomerPofileDetails));
  }
  updateCustomerProfile(
    data: CustomerPofileDetails,
    selectedFile: File | null
  ): Observable<any> {
    const formData = new FormData();

    formData.append('FName', data.fName);
    formData.append('LName', data.lName);
    formData.append('Gender', data.gender.toString());
    formData.append('Street', data.street);
    formData.append('City', data.city);
    formData.append('Country', data.country);
    formData.append('userName', data.userName);
    formData.append('email', data.email);
    formData.append('phoneNumber', data.phoneNumber.toString());

    if (selectedFile) {
      formData.append('ImageFile', selectedFile);
    }

    return this.http.put(`${this.baseUrl}/UpdateProfile`, formData);
  }

  getCustomerById(id: string): Observable<CustomerPofileDetails> {
    return this.http
      .get<any>(`${this.baseUrl}/${id}`)
      .pipe(map((response) => response.data as CustomerPofileDetails));
  }
}
