import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../../models/product';
import { ApiResponse } from '../../models/api-response';
import { SellerProductsModel } from '../seller-dashboard/seller-products-model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = 'https://localhost:7102/api/Product';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': 'Bearer ' + token
    });
  }

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}`);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }

  add(product: Product): Observable<any> {
    return this.http.post(this.baseUrl, product, {
      headers: this.getAuthHeaders()
    });
  }

  update(id: number, product: Product): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, product, {
      headers: this.getAuthHeaders()
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  addWithImage(formData: FormData): Observable<any> {
    return this.http.post(this.baseUrl, formData, {
      headers: this.getAuthHeaders()
    });
  }

  updateWithImage(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, formData, {
      headers: this.getAuthHeaders()
    });
  }

  getProductsBySellerId(sellerId: string): Observable<ApiResponse<SellerProductsModel[]>> {
    return this.http.get<ApiResponse<SellerProductsModel[]>>(`https://localhost:7102/api/Seller/Products`, {
      headers: this.getAuthHeaders()
    });
  }
}
