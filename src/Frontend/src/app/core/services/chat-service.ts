import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { MessengerContact } from '../../models/messenger-contact';
import { ChatMessage } from '../../models/chat-message';

@Injectable({
  providedIn: 'root',
})
export class ChatSignalrService {
  private hubConnection!: signalR.HubConnection;
  public messages$ = new BehaviorSubject<any[]>([]);
  public connectionStatus$ = new BehaviorSubject<string>('Disconnected');
  public userOnline$ = new BehaviorSubject<string | null>(null);
  public userOffline$ = new BehaviorSubject<string | null>(null);
  private connectionEstablished = false;
  constructor(private http: HttpClient) {}

  connect(token: string): void {
    if (this.connectionEstablished) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7102/chat', {
        accessTokenFactory: () => token,
        transport: signalR.HttpTransportType.WebSockets,
        withCredentials: false, // مهم جداً
      })
      .withAutomaticReconnect()
      .build();

    this.registerOnServerEvents();

    this.hubConnection
      .start()
      .then(() => {
        this.connectionEstablished = true;
        this.connectionStatus$.next('Connected');
        console.log('✅ SignalR Connected');
      })
      .catch((err) => {
        this.connectionStatus$.next('Disconnected');
        console.error('❌ SignalR connection error:', err);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection?.on('ReceiveMessage', (message) => {
      const currentMessages = this.messages$.value;
      this.messages$.next([...currentMessages, message]);
    });

    this.hubConnection?.on('MessageSentConfirmation', (confirmation) => {
      console.log('✅ Message sent confirmation:', confirmation);
    });
    this.hubConnection?.on('UserOnline', (userId: string) => {
      this.userOnline$.next(userId);
    });

    this.hubConnection?.on('UserOffline', (userId: string) => {
      this.userOffline$.next(userId);
    });
  }

  async sendMessage(
    receiverId: string,
    message: string,
    attachmentUrl: string | null = null
  ): Promise<void> {
    try {
      await this.hubConnection.invoke(
        'SendMessage',
        message,
        receiverId,
        attachmentUrl
      );
    } catch (err) {
      console.error('❌ Error sending message:', err);
    }
  }

  uploadChatFile(file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(`${environment.apiBaseUrl}/chat/upload`, formData, {
      responseType: 'text', // or 'json' depending on API
    });
  }
  async sendFileMessage(receiverId: string, file: File): Promise<void> {
    try {
      const formData = new FormData();
      formData.append('file', file);

      const attachmentUrl = await this.http
        .post(`${environment.apiBaseUrl}/chat/upload`, formData, {
          responseType: 'text',
        })
        .toPromise();

      await this.sendMessage(receiverId, '', attachmentUrl);
    } catch (err) {
      console.error('❌ Error sending file message:', err);
    }
  }
  getMessengerContacts() {
    return this.http.get<MessengerContact[]>(
      `${environment.apiBaseUrl}/chat/Messenger`
    );
  }
  getChatMessages(receiverId: string): Observable<ChatMessage[]> {
    return this.http.get<ChatMessage[]>(
      `${environment.apiBaseUrl}/Chat/Load/${receiverId}`
    );
  }

  disconnect(): void {
    if (!this.connectionEstablished || !this.hubConnection) return;

    this.hubConnection
      .stop()
      .then(() => {
        this.connectionStatus$.next('Disconnected');
        this.connectionEstablished = false;
        console.log('❌ SignalR Disconnected');
      })
      .catch((err) => {
        console.error('❌ Error during SignalR disconnection:', err);
      });
  }
}
