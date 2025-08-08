import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { map, Observable } from 'rxjs';
import { Blog } from './blog-models';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class BlogService {
  private apiUrl = environment.apiBaseUrl; // e.g. "https://localhost:7102"

  constructor(private http: HttpClient) {}
  getAllBlogs(): Observable<Blog[]> {
    return this.http
      .get<ApiResponse<Blog[]>>(`${this.apiUrl}/Blog/AllBlogs`)
      .pipe(map((res) => res.data));
  }

  getSingleBlogPost(id: string): Observable<Blog> {
    return this.http
      .get<ApiResponse<Blog>>(`${this.apiUrl}/Blog/Blog/${id}`)
      .pipe(map((res) => res.data));
  }
}
