import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { NotificationModel } from '../../models/notification-model';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = `${environment.apiBaseUrl}/Notification`;
  private hubUrl = `${environment.apiBaseUrl.replace(
    '/api',
    ''
  )}/notificationHub`;

  private hubConnection!: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<NotificationModel | null>(
    null
  );

  // Observable for components to subscribe to
  public newNotification$ = this.notificationSubject.asObservable();

  constructor(private http: HttpClient) {}

  // ==== HTTP API Methods ====
  getNotifications(userId: string): Observable<NotificationModel[]> {
    return this.http.get<NotificationModel[]>(`${this.apiUrl}/${userId}`);
  }

  markAsRead(notificationId: string): Observable<string> {
    return this.http.put<string>(
      `${this.apiUrl}/${notificationId}/read`,
      {},
      { responseType: 'text' as 'json' }
    );
  }

  deleteNotification(notificationId: string): Observable<string> {
    return this.http.delete<string>(`${this.apiUrl}/${notificationId}/delete`, {
      responseType: 'text' as 'json',
    });
  }
  /** ===== SignalR Connection for Realtime Notifications ===== **/
  startConnection(token: string): void {
    if (this.hubConnection) return; // avoid multiple connections

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7102/notificationHub', {
        accessTokenFactory: () => token,
        transport: signalR.HttpTransportType.WebSockets,
        withCredentials: false,
      })
      .withAutomaticReconnect()
      .build();

    // Listen for server-sent events
    this.hubConnection.on('ReceiveNotification', (data) => {
      console.log('📩 New realtime notification:', data);

      const notification: NotificationModel = {
        notificationId: data.notificationId || '',
        message: data.message,
        type: data.messageType || 0,
        isRead: false,
        createdAt: data.createdAt || new Date().toISOString(),
      } as NotificationModel;

      this.notificationSubject.next(notification);
    });

    // Start the connection
    this.hubConnection
      .start()
      .then(() => {
        console.log('✅ Connected to NotificationHub');
      })
      .catch((err) => {
        console.error('❌ Error connecting to NotificationHub', err);
      });
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.hubConnection = undefined!;
    }
  }

  disconnect(): void {
    if (!this.hubConnection) return;

    this.hubConnection
      .stop()
      .then(() => {
        console.log('❌ NotificationHub Disconnected');
        this.hubConnection = undefined!;
        // Optionally reset the BehaviorSubject so no stale data lingers
        this.notificationSubject.next(null);
      })
      .catch((err) => {
        console.error('❌ Error during NotificationHub disconnection:', err);
      });
  }
}
