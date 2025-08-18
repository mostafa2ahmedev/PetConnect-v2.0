import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  CreateFollowUpSupportRequestDto,
  CreateSupportRequestDto,
  CreateSupportResponseDto,
  EnumOption,
  SupportRequest,
  UpdateRequestStatusPriorityDto,
  UserSupportRequestDetails,
  UserSupportRequestInfo,
} from './support-models';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SupportServie {
  private readonly baseUrl = 'https://localhost:7102/api/Support';

  constructor(private http: HttpClient) {}

  // --------------------
  // Create requests
  // --------------------

  // Create a new support request (multipart/form-data)
  createSupportRequest(data: CreateSupportRequestDto): Observable<any> {
    const formData = new FormData();
    formData.append('Type', data.type.toString());
    formData.append('Message', data.message);
    if (data.pictureUrl) formData.append('PictureUrl', data.pictureUrl);

    return this.http.post(`${this.baseUrl}/CreateSupportRequest`, formData);
  }

  // Create a follow-up support request (multipart/form-data)
  createFollowUpRequest(data: {
    supportRequestId: number;
    message: string;
    pictureUrl?: File;
  }): Observable<any> {
    console.log('sdsdsdsdsd', data);
    const formData = new FormData();
    formData.append('Message', data.message);
    formData.append('SupportRequestId', data.supportRequestId.toString());

    if (data.pictureUrl) {
      formData.append('PictureUrl', data.pictureUrl);
    }

    return this.http.post(
      `${this.baseUrl}/CreateFollowUpSupportRequest`,
      formData
    );
  }

  // Create an admin support response (multipart/form-data)
  createSupportResponse(data: CreateSupportResponseDto): Observable<any> {
    const formData = new FormData();
    formData.append('Message', data.message);
    formData.append('Subject', data.subject);
    formData.append('SupportRequestId', data.supportRequestId.toString());
    formData.append('Status', data.status.toString());
    if (data.pictureUrl) formData.append('PictureUrl', data.pictureUrl);

    return this.http.post(`${this.baseUrl}/CreateSupportResponse`, formData);
  }

  // --------------------
  // Get requests
  // --------------------

  // Get all support requests that need admin action
  getSupportRequests(): Observable<SupportRequest[]> {
    return this.http.get<any>(`${this.baseUrl}/SupportRequests`).pipe(
      map((res) => (res.data as SupportRequest[]) || []) // safely map to array
    );
  }

  // Get requests submitted by the current user
  getUserSubmittedRequestsInfo(): Observable<UserSupportRequestInfo[]> {
    return this.http.get<any>(`${this.baseUrl}/UserSubmittedRequestsInfo`).pipe(
      map((res) => (res.data as UserSupportRequestInfo[]) || []) // safely map to array
    );
  }

  // Get request details (including messages) for a specific request
  getUserSubmittedRequestsDetails(
    supportRequestId: number
  ): Observable<UserSupportRequestDetails> {
    return this.http
      .get<any>(
        `${this.baseUrl}/UserSubmittedRequestsDetails?supportRequestId=${supportRequestId}`
      )
      .pipe(
        map((res) => res.data as UserSupportRequestDetails) // safely map to array
      );
  }

  // --------------------
  // Update requests
  // --------------------

  // Update status/priority of a request
  updateRequestStatusPriority(
    data: UpdateRequestStatusPriorityDto
  ): Observable<any> {
    return this.http.put(`${this.baseUrl}/UpdateRequestStatusPriority`, data);
  }

  // ---- New Enum Methods ----
  getSupportRequestTypes(): Observable<EnumOption[]> {
    return this.http
      .get<EnumOption[]>(`${environment.apiBaseUrl}/Enums/SupportRequest-types`)
      .pipe(map((res) => res || []));
  }

  getSupportRequestStatus(): Observable<EnumOption[]> {
    return this.http
      .get<any>(`${environment.apiBaseUrl}/Enums/SupportRequest-Status`)
      .pipe(map((res) => res || []));
  }

  getSupportRequestPriority(): Observable<EnumOption[]> {
    return this.http
      .get<any>(`${environment.apiBaseUrl}/Enums/SupportRequest-Priority`)
      .pipe(map((res) => res || []));
  }
}
