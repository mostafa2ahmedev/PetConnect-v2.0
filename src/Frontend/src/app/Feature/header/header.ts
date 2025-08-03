import { JwtUser } from '../../core/models/jwt-user';
import { IDoctor } from '../doctors/idoctor';
import { CustomerDto } from '../profile/customer-dto';
import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  NavigationEnd,
  Router,
  RouterLink,
  RouterModule,
} from '@angular/router';
import { AccountService } from '../../core/services/account-service';
import { NotificationModel } from '../../models/notification-model';
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../core/services/notification-service';
import { CustomerService } from '../customer-profile/customer-service';
import { CustomerPofileDetails } from '../../models/customer-pofile-details';

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule, RouterModule],
  templateUrl: './header.html',
  styleUrls: ['./header.css'], // ✅ Fixed 'styleUrl' to 'styleUrls'
  animations: [
    trigger('slideToggle', [
      state('open', style({ height: '*', opacity: 1, zIndex: 999 })),
      state('closed', style({ height: '0px', opacity: 0, zIndex: 0 })),
      transition('open <=> closed', [animate('300ms ease-in-out')]),
    ]),
  ],
})
export class Header implements OnInit, OnDestroy {
  userFullname: string = '';
  user: JwtUser = {} as JwtUser;
  userId: string = '';
  caller: string = '';
  doctor: IDoctor = {} as IDoctor;
  customer: CustomerDto = {} as CustomerDto;
  @ViewChild('notificationPanel') notificationPanel!: ElementRef;
  profileData: CustomerPofileDetails | null = null; // Instead of a large ViewModel type

  notifications: NotificationModel[] = [];
  unreadCount = 0;
  isOpen = false;
  routerEventsSub!: Subscription;
  constructor(
    public accountService: AccountService,
    private router: Router,
    private notificationService: NotificationService,
    private customerService: CustomerService
  ) {}
  ngOnDestroy(): void {
    this.notificationService.disconnect();
  }
  ngOnInit(): void {
    if (!this.accountService.isAuthenticated()) return;

    // Step 1: Decode the token immediately if available
    const decodedUser = this.accountService.jwtTokenDecoder();
    console.log('Decoded User:', decodedUser);
    if (decodedUser && decodedUser.userId) {
      this.userId = decodedUser.userId;
      this.loadProfileAndNotifications();
    }

    // Step 2: Load profile and role-specific data
    if (this.accountService.isCustomer()) {
      console;
      this.accountService.getCustomerData()?.subscribe({
        next: (resp) => {
          if (resp) {
            this.userFullname = `${resp.data.fName} ${resp.data.lName}`;
            this.caller = '';
            this.customer = resp.data;
          }
        },
      });
    } else if (this.accountService.isDoctor()) {
      this.accountService.getDoctorData()?.subscribe({
        next: (resp) => {
          if (resp && typeof resp !== 'string') {
            this.userFullname = `${resp.fName} ${resp.lName}`;
            this.caller = 'Dr ';
            this.doctor = resp;
          }
        },
      });
    }
  }

  private loadProfileAndNotifications() {
    // Load customer profile picture/details
    this.customerService.getCustomerProfile().subscribe((data) => {
      this.profileData = data;
    });

    // Start SignalR connection only once
    const token =
      localStorage.getItem('token') || sessionStorage.getItem('token') || '';
    this.notificationService.startConnection(token);

    // Initial load of notifications
    this.loadNotifications(this.userId);

    // Listen for real-time notifications
    this.notificationService.newNotification$.subscribe((notif) => {
      if (notif) {
        this.unreadCount++;
        this.loadNotifications(this.userId);
        this.showNotificationToast(notif.message);
      }
    });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedInside = this.notificationPanel?.nativeElement.contains(
      event.target as Node
    );
    if (!clickedInside && this.isOpen) {
      this.isOpen = !this.isOpen;
    }
  }

  isAuthenticated(): boolean {
    return this.accountService.isAuthenticated();
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }

  goToProfile(): void {
    console.log('entered');
    if (this.accountService.isCustomer()) {
      this.router.navigateByUrl(`/profile/${this.userId}`, {
        state: { customer: this.customer, role: 'customer' },
      });
    }
    if (this.accountService.isDoctor()) {
      this.router.navigateByUrl(`/profile/${this.userId}`, {
        state: { doctor: this.doctor, role: 'doctor' },
      });
    }
  }

  loadNotifications(userId: string): void {
    this.notificationService.getNotifications(userId).subscribe({
      next: (notifications) => {
        console.log('📢 Notifications:', notifications);
        this.notifications = [...notifications].sort(
          (a, b) =>
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
        );
        this.unreadCount = notifications.filter((n) => !n.isRead).length;
      },
      error: (err) => {
        console.error('❌ Failed to load notifications:', err);
      },
    });
  }

  get slideState() {
    return this.isOpen ? 'open' : 'closed';
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }

  markAsRead(notif: NotificationModel) {
    this.notificationService.markAsRead(notif.notificationId).subscribe(() => {
      notif.isRead = true;
      this.unreadCount = this.notifications.filter((n) => !n.isRead).length;
    });

    if (this.accountService.isCustomer()) {
      this.router.navigateByUrl(`/profile`);
    } else {
      if (this.accountService.isDoctor()) {
        this.router.navigateByUrl(`/doc-profile`);
      }
    }
    this.isOpen = false; // Close the notification panel after clicking
  }

  deleteNotification(notif: NotificationModel, event: MouseEvent) {
    event.stopPropagation(); // Prevent triggering markAsRead
    this.notificationService
      .deleteNotification(notif.notificationId)
      .subscribe(() => {
        this.notifications = this.notifications.filter(
          (n) => n.notificationId !== notif.notificationId
        );
        this.unreadCount = this.notifications.filter((n) => !n.isRead).length;
      });
  }

  showNotificationToast(message: string) {
    const toastEl = document.createElement('div');
    toastEl.className = 'custom-toast align-items-center border-0';
    toastEl.role = 'alert';
    toastEl.innerHTML = `
    <div class="d-flex">
     <i class="bi bi-bell text-white fs-5"></i>
      <div class="toast-body">
        ${message}
      </div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto"
        aria-label="Close"></button>
    </div>
  `;

    // Close button event
    toastEl.querySelector('.btn-close')?.addEventListener('click', () => {
      toastEl.classList.remove('fade-in');
      toastEl.classList.add('fade-out');
      setTimeout(() => toastEl.remove(), 500);
    });

    document.body.appendChild(toastEl);

    // Trigger fade-in
    setTimeout(() => toastEl.classList.add('fade-in'), 10);

    // Auto remove after 5s
    setTimeout(() => {
      toastEl.classList.remove('fade-in');
      toastEl.classList.add('fade-out');
      setTimeout(() => toastEl.remove(), 500);
    }, 5000);
  }
}
