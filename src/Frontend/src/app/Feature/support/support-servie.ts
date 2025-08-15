import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  CreateSupportRequestDto,
  CreateSupportResponseDto,
  SupportRequest,
} from './support-models';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SupportServie {
  private readonly baseUrl = 'https://localhost:7102/api/Support';

  constructor(private http: HttpClient) {}

  // Create a new support request
  createSupportRequest(data: CreateSupportRequestDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/CreateSupportRequest`, data);
  }

  // Get all support requests that need admin action
  getSupportRequests(): Observable<SupportRequest[]> {
    return this.http
      .get<{ status: number; data: SupportRequest[] }>(
        `${this.baseUrl}/SupportRequests`
      )
      .pipe(map((res) => res.data));
  }

  // Create a response to a support request
  createSupportResponse(data: CreateSupportResponseDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/CreateSupportResponse`, data);
  }
}
