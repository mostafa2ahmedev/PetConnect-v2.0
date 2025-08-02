import { JwtUser } from '../../core/models/jwt-user';
import { IDoctor } from '../doctors/idoctor';
import { CustomerDto } from '../profile/customer-dto';
import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
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
export class Header implements OnInit {
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
  ngOnInit(): void {
    if (this.accountService.isAuthenticated()) {
      this.loadNotifications(this.userId);
      this.customerService.getCustomerProfile().subscribe((data) => {
        console.log('Profile Data:', data);
        this.profileData = data;
      });
    }
    this.routerEventsSub = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.loadNotifications(this.userId);
      }
    });

    if (this.accountService.isCustomer()) {
      this.accountService.getCustomerData()?.subscribe({
        next: (resp) => {
          this.user = this.accountService.jwtTokenDecoder();
          this.userId = this.user.userId;
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
          if (resp && typeof resp != 'string') {
            this.userFullname = `${resp.fName} ${resp.lName}`;
            this.caller = 'Dr ';
            this.doctor = resp;
          }
        },
      });
    }
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
        this.notifications = notifications;
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
}
