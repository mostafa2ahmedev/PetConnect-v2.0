import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AdoptionRequest } from '../../models/adoption-request';
import { map, Observable } from 'rxjs';
import { AdoptionResponse } from '../../models/adoption-response';
import { AdoptionDecision } from '../../models/adoption-decision';
import { Pet } from '../../models/pet';

@Injectable({
  providedIn: 'root',
})
export class AdoptionService {
  private readonly baseUrl = environment.apiBaseUrl + '/customer';

  constructor(private http: HttpClient) {}

  // Submit adoption request
  submitRequest(request: AdoptionRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}`, request);
  }

  // Get requests where current user is the receiver (owner of the pet)
  getIncomingRequests(): Observable<AdoptionResponse[]> {
    return this.http.get<any>(`${this.baseUrl}`).pipe(
      map((response) => response.data) // because your API wraps result in { statusCode, data }
    );
  }

  // Approve or cancel a request
  approveOrCancelRequest(decision: AdoptionDecision): Observable<any> {
    return this.http.put(`${this.baseUrl}/ApproveORCancel`, decision);
  }

  // Get pets owned by the current customer
  getCustomerPets(): Observable<Pet[]> {
    return this.http.get<Pet[]>(`${this.baseUrl}/CustomerPets`);
  }
}
