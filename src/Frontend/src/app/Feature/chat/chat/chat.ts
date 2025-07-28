import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '../../../core/services/chat-service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.html',
})
export class ChatComponent implements OnInit {
  message = '';
  receiverId = '';
  senderId = '';
  messages: any[] = [];
  connectionStatus = '';

  constructor(
    private chatService: ChatSignalrService,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.receiverId = params['id'];
      }
    });
    this.senderId = this.authService.getUserId();
    const token =
      localStorage.getItem('token') || sessionStorage.getItem('token');

    if (token) {
      this.chatService.connect(token);

      this.chatService.connectionStatus$.subscribe((status) => {
        this.connectionStatus = status;
      });

      this.chatService.messages$.subscribe((msgs) => {
        console.log('📬 big Messages received:', msgs);
        this.messages = msgs.filter(
          (msg) =>
            msg.receiverId === this.receiverId ||
            (msg.senderId && msg.senderId === this.receiverId)
        );
        console.log('📬 Messages received:', msgs);
        console.log('my id', this.senderId);
      });
    } else {
      alert('🚫 No token found in localStorage!');
    }
  }

  async sendMessage() {
    if (
      !this.message.trim() ||
      !this.receiverId.trim() ||
      this.connectionStatus !== 'Connected'
    )
      return;

    const outgoingMsg = {
      receiverId: this.receiverId,
      message: this.message,
    };

    try {
      await this.chatService.sendMessage(this.receiverId, this.message);
      this.messages.push(outgoingMsg); // Add to UI immediately

      this.message = '';
    } catch (error) {
      console.error('❌ Send failed:', error);
    }
  }
}
