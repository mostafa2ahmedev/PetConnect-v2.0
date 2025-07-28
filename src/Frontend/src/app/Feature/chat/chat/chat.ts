import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '../../../core/services/chat-service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';
import { MessengerContact } from '../../../models/messenger-contact';

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
  contacts: MessengerContact[] = [];
  activeContact: MessengerContact | null = null;
  // chatMessages: ChatMessage[] = [];

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
        this.messages = msgs;
      });
    } else {
      alert('🚫 No token found in localStorage!');
    }

    this.chatService.getMessengerContacts().subscribe({
      next: (data) => {
        console.log('📨 Messenger contacts:', data);
        this.contacts = data;
      },
      error: (err) => {
        console.error('❌ Failed to fetch messenger contacts:', err);
      },
    });
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
      // this.messages.push(outgoingMsg); // Add to UI immediately

      this.message = '';
    } catch (error) {
      console.error('❌ Send failed:', error);
    }
  }

  loadMessages(receiverId: string) {
    console.log('📥 Loading messages for:', receiverId);
    this.chatService.getChatMessages(receiverId).subscribe({
      next: (res) => {
        console.log('📩 Loaded chat messages:', res);
        this.messages = res;
      },
      error: (err) => {
        console.error('❌ Failed to load chat messages:', err);
      },
    });
  }

  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
  activateContact(contact: MessengerContact) {
    this.activeContact = contact;
    this.receiverId = contact.userId;
    this.loadMessages(this.receiverId);
  }
}
