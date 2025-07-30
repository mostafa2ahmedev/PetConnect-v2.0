import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '../../../core/services/chat-service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';
import { MessengerContact } from '../../../models/messenger-contact';
import { CustomerService } from '../../customer-profile/customer-service';
import { CustomerPofileDetails } from '../../../models/customer-pofile-details';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.html',
  styleUrls: ['./chat.css'],
})
export class ChatComponent implements OnInit, AfterViewInit {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  message = '';
  receiverId = '';
  senderId = '';
  messagesHistory: any[] = [];

  messages: any[] = [];
  connectionStatus = '';
  contacts: MessengerContact[] = [];
  activeContact: MessengerContact | null = null;
  profileData: CustomerPofileDetails | null = null;
  isSidebarVisible = false;

  // chatMessages: ChatMessage[] = [];

  constructor(
    private chatService: ChatSignalrService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router,
    private customerService: CustomerService
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
        this.activeContact =
          this.contacts.find((contact) => contact.userId === this.receiverId) ||
          null;
        console.log('📬 Active contact:', this.activeContact);
        if (!this.receiverId && this.contacts.length > 0) {
          this.receiverId = this.contacts[0].userId;
          this.activeContact = this.contacts[0];
          this.loadMessages(this.receiverId);

          console.log('📬 Default active contact:', this.activeContact);
        }
      },
      error: (err) => {
        console.error('❌ Failed to fetch messenger contacts:', err);
      },
    });
    this.loadMessages(this.receiverId);

    this.customerService.getCustomerProfile().subscribe((data) => {
      console.log('Profile Data:', data);
      this.profileData = data;
    });
  }
  ngAfterViewInit(): void {
    this.scrollToBottom();
  }
  private scrollToBottom(): void {
    console.log('📜 Scrolling to bottom of messages container');
    setTimeout(() => {
      try {
        this.messagesContainer.nativeElement.scrollTop =
          this.messagesContainer.nativeElement.scrollHeight;
      } catch (err) {
        console.warn('Scroll failed:', err);
      }
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
        this.messagesHistory = res;
        this.scrollToBottom();
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
    this.router.navigate(['../', contact.userId], {
      relativeTo: this.route,
      replaceUrl: true, // Optional: prevents adding to history stack
    });
    this.loadMessages(this.receiverId);
    this.toggleContactsSidebar();
  }

  formatMessageDate(isoDate: string): string {
    const date = new Date(isoDate);
    const now = new Date();

    const isToday =
      date.getDate() === now.getDate() &&
      date.getMonth() === now.getMonth() &&
      date.getFullYear() === now.getFullYear();

    return isToday
      ? date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
      : date.toLocaleDateString([], { day: '2-digit', month: '2-digit' });
  }
  toggleContactsSidebar() {
    this.isSidebarVisible = !this.isSidebarVisible;
  }
}
