import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { map, Observable } from 'rxjs';
import { Blog, BlogComment, BlogReadWrite } from './blog-models';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class BlogService {
  private apiUrl = environment.apiBaseUrl; // e.g. "https://localhost:7102"

  constructor(private http: HttpClient) {}
  getAllReadBlogs(): Observable<Blog[]> {
    return this.http
      .get<ApiResponse<Blog[]>>(`${this.apiUrl}/Blog/ReadBlogs`)
      .pipe(map((res) => res.data));
  }

  getAllReadWriteBlogs(): Observable<Blog[]> {
    return this.http
      .get<ApiResponse<Blog[]>>(`${this.apiUrl}/Blog/ReadWriteBlogs`)
      .pipe(map((res) => res.data));
  }
}
