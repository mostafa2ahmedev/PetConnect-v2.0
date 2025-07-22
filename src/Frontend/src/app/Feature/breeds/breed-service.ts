import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Breed } from '../../models/breed';
import { ApiResponse } from '../../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class BreedService {
  private apiUrl = `${environment.apiBaseUrl}/petbread`;

  constructor(private http: HttpClient) {}

  getAllBreeds(): Observable<Breed[]> {
    return this.http
      .get<ApiResponse<Breed[]>>(this.apiUrl)
      .pipe(map((res) => res.data));
  }

  // POST add a new breed
  addBreed(name: string, categoryId: number): Observable<any> {
    const formData = new FormData();
    formData.append('Name', name);
    formData.append('CategoryId', categoryId.toString());

    return this.http.post(`${this.apiUrl}`, formData);
  }

  // PUT update an existing breed
  updateBreed(breed: Breed): Observable<any> {
    console.log('Updating breed:', breed.id);
    const formData = new FormData();
    formData.append('Id', breed.id.toString());
    formData.append('Name', breed.name);
    formData.append('CategoryId', breed.categoryId.toString()); // or String(breed.categoryId)
    return this.http.put(`${this.apiUrl}`, formData);
  }

  // DELETE a breed by ID (as query param)
  deleteBreed(id: number): Observable<any> {
    const params = new HttpParams().set('id', id.toString());
    return this.http.delete(`${this.apiUrl}`, { params });
  }

  getBreedById(id: number): Observable<Breed> {
    return this.http
      .get<ApiResponse<Breed>>(`${this.apiUrl}/${id}`)
      .pipe(map((res) => res.data));
  }
}
