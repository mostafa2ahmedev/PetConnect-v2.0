import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
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
@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
  animations: [
    trigger('slideToggle', [
      state('open', style({ height: '*', opacity: 1 })),
      state('closed', style({ height: '0px', opacity: 0 })),
      transition('open <=> closed', [animate('300ms ease-in-out')]),
    ]),
  ],
})
export class Header implements OnInit {
  notifications: NotificationModel[] = [];
  unreadCount = 0;
  isOpen = false;

  constructor(
    private accontService: AccountService,
    private router: Router,
    public authService: AuthService,
    private adoptionService: AdoptionService
  ) {}
  ngOnInit(): void {
    this.loadNotifications();
  }
  isAuthenticated(): boolean {
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
