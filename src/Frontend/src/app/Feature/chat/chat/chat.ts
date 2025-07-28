import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '../../../core/services/chat-service';
import { IMessage } from '../IMessage';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.html',
})
export class ChatComponent implements OnInit {
  message = '';
  receiverId = '';
  messages: IMessage[] = [];
  connectionStatus = '';

  constructor(private chatService: ChatSignalrService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (token) {
      this.chatService.connect(token);

      this.chatService.connectionStatus$.subscribe((status) => {
        this.connectionStatus = status;
      });

      this.chatService.messages$.subscribe((incoming: any) => {
        console.log('📥 Incoming:', incoming);

        if (Array.isArray(incoming)) {
          this.messages = incoming; // ❗ استبدال مش تراكم
        } else if (
          incoming &&
          typeof incoming === 'object' &&
          'message' in incoming
        ) {
          this.messages = [...this.messages, incoming]; // ❗ ضيف الجديد بس
        }
      });
    } else {
      alert('🚫 No token found in localStorage!');
    }
  }

  async sendMessage() {
    if (
      !this.message ||
      !this.receiverId ||
      this.connectionStatus !== 'Connected'
    )
      return;

    try {
      await this.chatService.sendMessage(this.receiverId, this.message);
      this.message = '';
    } catch (error) {
      console.error('❌ Send failed:', error);
    }
  }
}
