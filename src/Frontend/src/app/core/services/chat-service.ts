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
  }

  async sendMessage(receiverId: string, message: string): Promise<void> {
    try {
      await this.hubConnection.invoke('SendMessage', message, receiverId, null);
    } catch (err) {
      console.error('❌ Error sending message:', err);
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
}
