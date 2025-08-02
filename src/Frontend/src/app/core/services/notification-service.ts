import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { NotificationModel } from '../../models/notification-model';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = `${environment.apiBaseUrl}/Notification`;

  constructor(private http: HttpClient) {}

  getNotifications(userId: string): Observable<NotificationModel[]> {
    return this.http.get<NotificationModel[]>(`${this.apiUrl}/${userId}`);
  }
}
