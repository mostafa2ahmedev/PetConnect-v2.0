import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { JwtUser } from '../models/jwt-user';
import { DoctorsService } from '../../Feature/doctors/doctors-service';
import { CustomerService } from '../../Feature/profile/customer-service';
import { CustomerDto } from '../../Feature/profile/customer-dto';
const API_URL = environment.apiBaseUrl + '/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  doctorService = inject(DoctorsService);
  customerService = inject(CustomerService);
  constructor(private http: HttpClient) {}

  public PostCustomerRegister(formData: FormData): Observable<any> {
    return this.http.post(API_URL + '/register/customer', formData);
  }

  public PostDoctorRegister(formData: FormData): Observable<any> {
    return this.http.post(API_URL + '/register/doctor', formData);
  }

  public PostSellerRegister(formData: FormData): Observable<any> {
    return this.http.post(API_URL + '/register/Seller', formData);
  }
  public PostLogin(credentials: {
    email: string;
    password: string;
  }): Observable<any> {
    return this.http.post(API_URL + '/login', credentials);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token') || !!sessionStorage.getItem('token');
  }

  logout(): void {
    // Clear all auth-related storage
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userRoles');
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('userRoles');
  }

  jwtTokenDecoder(): JwtUser {
    const helper = new JwtHelperService();
    const token =
      localStorage.getItem('token') || sessionStorage.getItem('token') || '';
    const decodedToken = helper.decodeToken(token);
    const userId =
      decodedToken?.[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ];
    const userRole =
      decodedToken?.[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ];
    if (userId == undefined && userRole == undefined)
      return { userId: '', userRole: '', found: false };
    return { userId: userId, userRole: userRole, found: true };
  }
  getUserId(): string {
    const helper = new JwtHelperService();
    const token =
      localStorage.getItem('token') || sessionStorage.getItem('token') || '';

    if (!token || helper.isTokenExpired(token)) return '';

    const decodedToken = helper.decodeToken(token);
    const userId =
      decodedToken?.[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ];
    return userId || '';
  }
  isCustomer(): boolean {
    const user = this.jwtTokenDecoder();
    return user?.userRole == 'Customer' ? true : false;
  }
  isDoctor(): boolean {
    const user = this.jwtTokenDecoder();
    return user?.userRole == 'Doctor' ? true : false;
  }
   isSeller(): boolean {
    const user = this.jwtTokenDecoder();
    return user?.userRole == 'Seller' ? true : false;
  }

  getCustomerData(): Observable<{ statusCode: number; data: CustomerDto }> {
    const user = this.jwtTokenDecoder();
    return this.customerService.getCustomerById(user.userId);
  }
  getDoctorData() {
    const user = this.jwtTokenDecoder();
    return this.doctorService.getById(user.userId);
  }
  isAdmin(): boolean {
    // Get roles from storage (localStorage or sessionStorage)
    const rolesString =
      localStorage.getItem('userRoles') || sessionStorage.getItem('userRoles');

    if (!rolesString) return false; // No roles found

    try {
      // Parse roles array (assuming it's stored as JSON string)
      const roles: string[] = JSON.parse(rolesString);
      return roles.includes('Admin'); // Check if 'Admin' exists in the array
    } catch {
      return false; // Failed to parse roles
    }
  }

  getToken(): string | null {
    return localStorage.getItem('token') || sessionStorage.getItem('token');
  }
}
