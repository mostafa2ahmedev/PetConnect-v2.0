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
    return this.http.post(`${this.apiUrl}`, { name, categoryId });
  }

  // PUT update an existing breed
  updateBreed(breed: Breed): Observable<any> {
    return this.http.put(`${this.apiUrl}`, breed);
  }

  // DELETE a breed by ID (as query param)
  deleteBreed(id: number): Observable<any> {
    const params = new HttpParams().set('id', id);
    return this.http.delete(`${this.apiUrl}/PetBread`, { params });
  }
}
