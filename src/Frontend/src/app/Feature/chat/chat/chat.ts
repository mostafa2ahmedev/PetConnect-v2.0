import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatSignalrService } from '../../../core/services/chat-service';
import { ActivatedRoute, Router } from '@angular/router';
import { MessengerContact } from '../../../models/messenger-contact';
import { CustomerService } from '../../customer-profile/customer-service';
import { CustomerPofileDetails } from '../../../models/customer-pofile-details';
import { PickerModule } from '@ctrl/ngx-emoji-mart';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, PickerModule],
  templateUrl: './chat.html',
  styleUrls: ['./chat.css'],
})
export class ChatComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  showEmojiPicker = false;

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
  unreadSenders = new Set<string>();

  // chatMessages: ChatMessage[] = [];

  constructor(
    private chatService: ChatSignalrService,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private router: Router,
    private customerService: CustomerService
  ) {}
  ngOnDestroy(): void {
    this.chatService.disconnect();
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.receiverId = params['id'];
      }
    });
    this.senderId = this.accountService.getUserId();
    const token =
      localStorage.getItem('token') || sessionStorage.getItem('token');

    if (token) {
      this.chatService.connect(token);

      this.chatService.connectionStatus$.subscribe((status) => {
        this.connectionStatus = status;
      });

      this.chatService.messages$.subscribe((msgs) => {
        this.messages = msgs.filter(
          (msg) =>
            (msg.senderId === this.receiverId &&
              msg.receiverId === this.senderId) ||
            (msg.senderId === this.senderId &&
              msg.receiverId === this.receiverId)
        );

        const lastMsg = msgs[msgs.length - 1];
        if (lastMsg) {
          if (
            !(
              (lastMsg.senderId === this.receiverId &&
                lastMsg.receiverId === this.senderId) ||
              (lastMsg.senderId === this.senderId &&
                lastMsg.receiverId === this.receiverId)
            )
          ) {
            this.unreadSenders = new Set(this.unreadSenders).add(
              lastMsg.senderId
            );
          }
        }
        this.scrollToBottom();
      });
            this.chatService.userOnline$.subscribe(userId => {
        this.contacts = this.contacts.map(contact => 
          contact.userId === userId ? {...contact, isOnline: true} : contact
        );
        if (this.activeContact?.userId === userId) {
          this.activeContact.isOnline = true;
        }
      });

      this.chatService.userOffline$.subscribe(userId => {
        this.contacts = this.contacts.map(contact => 
          contact.userId === userId ? {...contact, isOnline: false} : contact
        );
        if (this.activeContact?.userId === userId) {
          this.activeContact.isOnline = false;
        }
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
    }, 200);
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

    this.scrollToBottom();
  }

  loadMessages(receiverId: string) {
    console.log('📥 Loading messages for:', receiverId);
    this.chatService.getChatMessages(receiverId).subscribe({
      next: (res) => {
        console.log('📩 Loaded chat messages:', res);
        this.messagesHistory = res;
        this.messages = [];

        this.scrollToBottom();
        // Ensure scroll happens after view update
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
    this.messages = []; // Clear current messages
    this.router.navigate(['../', contact.userId], {
      relativeTo: this.route,
      replaceUrl: true, // Optional: prevents adding to history stack
    });
    this.unreadSenders = new Set(
      [...this.unreadSenders].filter((id) => id !== contact.userId)
    );
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

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file && this.receiverId && this.connectionStatus === 'Connected') {
      this.chatService.sendFileMessage(this.receiverId, file);
      this.scrollToBottom();
    }
  }

  extractAttachmentPath(raw: string | null): string | null {
    if (!raw) return null;

    try {
      const parsed = JSON.parse(raw);
      return `https://localhost:7102/${parsed?.imagePath}`;
    } catch (e) {
      console.warn('Invalid JSON attachment URL:', raw);
      return null;
    }
  }
  addEmoji(event: any) {
    this.message += event.emoji.native;
  }
  isImage(raw: string | null): boolean {
    const path = this.extractAttachmentPath(raw)?.toLowerCase();
    return (
      !!path &&
      (path.includes('.jpg') ||
        path.includes('.jpeg') ||
        path.includes('.png') ||
        path.includes('.gif') ||
        path.includes('.webp') ||
        path.includes('.bmp'))
    );
  }

  isPdf(raw: string | null): boolean {
    const path = this.extractAttachmentPath(raw)?.toLowerCase();
    return !!path && path.includes('.pdf');
  }
}
