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
  DoCheck
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
})
export class Header implements OnInit  {
  userFullname: string = "";
  user: JwtUser = {} as JwtUser;
  userId: string = "";
  caller: string = "";
  doctor: IDoctor = {} as IDoctor;
  customer: CustomerDto = {} as CustomerDto;
    @ViewChild('notificationPanel') notificationPanel!: ElementRef;
  animations: [
    trigger('slideToggle', [
      state('open', style({ height: '*', opacity: 1, zIndex: 999 })),
      state('closed', style({ height: '0px', opacity: 0, zIndex: 0 })),
      transition('open <=> closed', [animate('300ms ease-in-out')]),
    ]),
  ],
})

  notifications: NotificationModel[] = [];
  unreadCount = 0;
  isOpen = false;
  routerEventsSub!: Subscription;

  
  ngOnInit(): void {
    //eslam
        if (this.authService.isAuthenticated()) this.loadNotifications();
    this.routerEventsSub = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.loadNotifications();
      }
    });
//eslam
        // Check if the user is authenticated and set the userFullname accordingly
     if(this.accontService.isCustomer())
    this.accontService.getCustomerData()?.subscribe({
  
      next:resp=>{
      this.user = this.accontService.jwtTokenDecoder();
      this.userId = this.user.userId;
      if(resp){
        this.userFullname= `${resp.data.fName} ${resp.data.lName}`;
        resp
        this.caller="";
        this.customer = resp.data;
      }
    }})
    else if (this.accontService.isDoctor())
          this.accontService.getDoctorData()?.subscribe({
      next:resp=>{
      if(resp && typeof resp != 'string'){
        this.userFullname= `${resp.fName} ${resp.lName}`;
        this.caller= "Dr ";
        this.doctor= resp ;
      }
  }})
  constructor(
    private accontService: AccountService,
    private router: Router,
    public authService: AuthService,
    private adoptionService: AdoptionService
  ) {}


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
    // console.log('isAuthenticated called', this.authService.getUserId());
    return this.accontService.isAuthenticated();
  }
  logout(): void {
    this.accontService.logout();
    this.router.navigate(['/login']);
  }
  goToProfile(){
    console.log("entered")
    if(this.accontService.isCustomer())
    this.router.navigateByUrl(`/profile/${this.userId}`,{state:{customer:this.customer ,role:"customer"}})
    if(this.accontService.isDoctor())
    this.router.navigateByUrl(`/profile/${this.userId}`,{state:{doctor:this.doctor, role:"doctor"}})
    

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
