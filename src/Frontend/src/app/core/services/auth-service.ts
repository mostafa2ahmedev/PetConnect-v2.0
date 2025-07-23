import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly tokenKey = 'token';
  private readonly userIdKey = 'userId';
  private readonly userRolesKey = 'userRoles';

  // Read from either session or local storage
  private getFromStorage(key: string): string | null {
    return sessionStorage.getItem(key) || localStorage.getItem(key);
  }

  isAuthenticated(): boolean {
    return !!this.getFromStorage(this.tokenKey);
  }

  getToken(): string | null {
    return this.getFromStorage(this.tokenKey);
  }

  getUserId(): string | null {
    return this.getFromStorage(this.userIdKey);
  }

  getUserRoles(): string[] {
    const rolesJson = this.getFromStorage(this.userRolesKey);
    try {
      return rolesJson ? JSON.parse(rolesJson) : [];
    } catch (err) {
      console.error('Failed to parse user roles:', err);
      return [];
    }
  }

  hasRole(role: string): boolean {
    return this.getUserRoles().includes(role);
  }

  isCustomer(): boolean {
    return this.hasRole('Customer');
  }
  isCusDoctor(): boolean {
    return this.hasRole('Doctor');
  }
  isAdmin(): boolean {
    return this.hasRole('Admin');
  }
  // logout(): void {
  //   sessionStorage.removeItem(this.tokenKey);
  //   sessionStorage.removeItem(this.userIdKey);
  //   sessionStorage.removeItem(this.userRolesKey);
  //   localStorage.removeItem(this.tokenKey);
  //   localStorage.removeItem(this.userIdKey);
  //   localStorage.removeItem(this.userRolesKey);
  // }
}
