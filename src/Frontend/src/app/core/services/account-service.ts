import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

const API_URL = environment.apiBaseUrl + '/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  public PostCustomerRegister(formData: FormData): Observable<any> {
    return this.http.post(API_URL + '/register/customer', formData);
  }

  public PostDoctorRegister(formData: FormData): Observable<any> {
    return this.http.post(API_URL + '/register/doctor', formData);
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
  isAdmin(): boolean {
    // Get roles from storage (localStorage or sessionStorage)
    const rolesString = localStorage.getItem('userRoles') || sessionStorage.getItem('userRoles');

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
