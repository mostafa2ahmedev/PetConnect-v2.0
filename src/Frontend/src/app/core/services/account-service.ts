import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

const API_URL = environment.apiBaseUrl + '/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  
  public PostDoctorRegister(formData: FormData): Observable<any> {
    return this.http.post<any>(API_URL + '/register/doctor', formData);
  }

  public PostCustomerRegister(formData: FormData): Observable<any> {
    return this.http.post(API_URL + '/register/customer', formData);
  }

  public PostLogin(credentials: { email: string; password: string; }): Observable<any> {
    return this.http.post(API_URL + '/login', credentials);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token') || !!sessionStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userRoles');
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('userRoles');
  }
}
