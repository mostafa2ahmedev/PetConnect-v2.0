import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  ViewChild,
} from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { AccountService } from '../../core/services/account-service';
import { AuthService } from '../../core/services/auth-service';
import { AdoptionService } from '../../core/services/adoption-service';
import { NotificationModel } from '../../models/notification-model';

import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
  animations: [
    trigger('slideToggle', [
      state('open', style({ height: '*', opacity: 1, zIndex: 999 })),
      state('closed', style({ height: '0px', opacity: 0, zIndex: 0 })),
      transition('open <=> closed', [animate('300ms ease-in-out')]),
    ]),
  ],
})
export class Header implements OnInit {
  @ViewChild('notificationPanel') notificationPanel!: ElementRef;

  notifications: NotificationModel[] = [];
  unreadCount = 0;
  isOpen = false;
  routerEventsSub!: Subscription;

  constructor(
    private accontService: AccountService,
    private router: Router,
    public authService: AuthService,
    private adoptionService: AdoptionService
  ) {}
  ngOnInit(): void {
    if (this.authService.isAuthenticated()) this.loadNotifications();
    this.routerEventsSub = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.loadNotifications();
      }
    });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedInside = this.notificationPanel?.nativeElement.contains(
      event.target
    );
    if (!clickedInside && this.isOpen) {
      this.isOpen = !this.isOpen;
    }
  }
  isAuthenticated(): boolean {
    console.log('isAuthenticated called', this.authService.getUserId());
    return this.accontService.isAuthenticated();
  }
  logout(): void {
    this.accontService.logout();
    this.router.navigate(['/login']);
  }

  loadNotifications(): void {
    this.adoptionService.GetAdoptionNotifications().subscribe({
      next: (res) => {
        console.log('Notifications:', res);
        this.notifications = res;
        this.unreadCount = res.filter((n) => !n.isRead).length;
      },
      error: (err) => {
        console.error('Failed to load notifications:', err);
      },
    });
  }

  get slideState() {
    return this.isOpen ? 'open' : 'closed';
  }
}
