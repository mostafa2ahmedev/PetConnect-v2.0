import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Category } from '../../models/category';
import { ApiResponse } from '../../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // GET all categories
  getCategories(): Observable<Category[]> {
    return this.http
      .get<ApiResponse<Category[]>>(`${this.apiUrl}/PetCategory`)
      .pipe(map((res) => res.data));
  }

  // POST add a new category
  addCategory(name: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/PetCategory`, { name });
  }

  // PUT update an existing category
  updateCategory(category: Category): Observable<any> {
    return this.http.put(`${this.apiUrl}/PetCategory`, category);
  }

  // DELETE a category by ID (as query param)
  deleteCategory(id: number): Observable<any> {
    const params = new HttpParams().set('id', id);
    return this.http.delete(`${this.apiUrl}/PetCategory`, { params });
  }
}
