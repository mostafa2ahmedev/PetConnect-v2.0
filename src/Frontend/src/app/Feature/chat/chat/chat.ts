import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '../../../core/services/chat-service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.html',
})
export class ChatComponent implements OnInit {
  message = '';
  receiverId = '';
  messages: any[] = [];
  connectionStatus = '';

  constructor(private chatService: ChatSignalrService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (token) {
      this.chatService.connect(token);

      this.chatService.connectionStatus$.subscribe((status) => {
        this.connectionStatus = status;
      });

      this.chatService.messages$.subscribe((msgs) => {
        this.messages = msgs;
      });
    } else {
      alert('🚫 No token found in localStorage!');
    }
  }

  async sendMessage() {
    if (!this.message || !this.receiverId || this.connectionStatus !== 'Connected') return;

    try {
      await this.chatService.sendMessage(this.receiverId, this.message);
      this.message = '';
    } catch (error) {
      console.error('❌ Send failed:', error);
    }
  }
}
