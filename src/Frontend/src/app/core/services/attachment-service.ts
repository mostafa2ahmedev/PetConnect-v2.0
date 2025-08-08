import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AttachmentService {
  constructor(private http: HttpClient) {}

  upload(file: File): Promise<string> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http
      .post(`${environment.apiBaseUrl}/chat/upload`, formData, {
        responseType: 'text',
      })
      .toPromise()
      .then((res) => res ?? '');
  }
}
