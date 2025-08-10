import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductType } from '../../models/product-type';


@Injectable({
  providedIn: 'root'
})
export class ProductTypeService {
  private baseUrl = 'https://localhost:7102/api/ProductType';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ProductType[]> {
    return this.http.get<ProductType[]>(`${this.baseUrl}`);
  }

  getById(id: number): Observable<ProductType> {
    return this.http.get<ProductType>(`${this.baseUrl}/${id}`);
  }

  add(productType: ProductType): Observable<any> {
    return this.http.post(this.baseUrl, productType);
  }

  update(id: number, productType: ProductType): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, productType);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
