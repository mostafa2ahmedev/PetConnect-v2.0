import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Pet } from '../../models/pet';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CusotmerPet } from '../../models/cusotmer-pet';

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
}
