import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Doctor } from './models/doctor';
import { Pet } from './models/pet';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerPofileDetails } from '../../../models/customer-pofile-details';

interface AdminData {
  pendingDoctors: Doctor[];
  pendingPets: Pet[];
}

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
  });

  constructor(private http: HttpClient) {}

  getPendingData(): Observable<AdminData> {
    return this.http
      .get<{ data: AdminData }>(`${environment.apiBaseUrl}/Admin`)
      .pipe(map((response) => response.data));
  }

  approveDoctor(id: string): Observable<any> {
    return this.http.put(
      `${environment.apiBaseUrl}/Admin/doctors/${id}/approve`,
      {},
      { headers: this.headers }
    );
  }

  rejectDoctor(id: string, message: string): Observable<any> {
    return this.http.put(
      `${environment.apiBaseUrl}/Admin/doctors/${id}/reject`,
      JSON.stringify(message), // Send as raw JSON string
      { headers: this.headers }
    );
  }

  approvePet(id: number): Observable<any> {
    return this.http.put(
      `${environment.apiBaseUrl}/Admin/pets/${id}/approve`,
      {},
      { headers: this.headers }
    );
  }

  rejectPet(id: number, message: string): Observable<any> {
    return this.http.put(
      `${environment.apiBaseUrl}/Admin/pets/${id}/reject`,
      JSON.stringify(message), // Send as raw JSON string
      { headers: this.headers }
    );
  }
  getStatistics(): Observable<AdminStatistics> {
    return this.http
      .get<{ data: AdminStatistics }>(
        `${environment.apiBaseUrl}/Admin/statistics`
      )
      .pipe(map((response) => response.data));
  }

  getAdminProfile(): Observable<CustomerPofileDetails> {
    return this.http
      .get<any>(`${environment.apiBaseUrl}/Admin/Profile`)
      .pipe(map((response) => response.data as CustomerPofileDetails));
  }
}
