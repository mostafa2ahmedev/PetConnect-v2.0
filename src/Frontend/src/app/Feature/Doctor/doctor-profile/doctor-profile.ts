import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { IDoctor } from '../../doctors/idoctor';
import {
  ActivatedRoute,
  Router,
  RouterLink,
  RouterModule,
} from '@angular/router';
import { DoctorsService } from '../../doctors/doctors-service';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { DataTimeSlotsDto } from './data-time-slot-dto';
import { DoctorProfileService } from './doctor-profile-service';
import { AccountService } from '../../../core/services/account-service';
import { JwtUser } from '../../../core/models/jwt-user';
import { AppointmentService } from './appointment-service';
import { AppointmentDto } from './appointment-dto';
import { AlertService } from '../../../core/services/alert-service';
import { TimeSlotsWithStatusDTO } from './time-slots-with-status-dto';
import { TimeSlotsCustomerService } from './time-slots-customer-service';
import { TimeSlotsWithCustomerIdStatusBookingDTO } from './time-slots-with-customer-id-status-booking-dto';
import { Blog } from '../../Blog/blog-models';
import { BlogService } from '../../Blog/blog-service';
import { ReviewMainPage } from "../../Review/review-main-page/review-main-page";
@Component({
  selector: 'app-doctor-profile',
  imports: [CurrencyPipe, DatePipe, CommonModule, RouterModule, ReviewMainPage],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css',
})
export class DoctorProfile implements OnInit {
  profileLoading = signal(true);
  activeRoute = inject(ActivatedRoute);
  doctorsService = inject(DoctorsService);
  accountService = inject(AccountService);
  timeSlotCustService = inject(TimeSlotsCustomerService);
  blogService = inject(BlogService);
  router = inject(Router);
  doctorProfileService = inject(DoctorProfileService);
  appointmentService = inject(AppointmentService);
  alert = inject(AlertService);
  server = 'https://localhost:7102';
  id: string = '';
  doctor: IDoctor | string = '';
  errorMessage = null;
  errorFound: boolean = false;
  userId = undefined;
  userRole = undefined;
  user: JwtUser = { found: false, userRole: '', userId: '' };
  appointments: AppointmentDto[] = [];
  blogs: Blog[] = [];
  loadingBlogs = true;
  // userId: string = '12345'; // Replace with real user id from auth
  topicId?: number;
  categoryId?: number;
  // State for loading dates and the data itself
  readonly loadingDates = signal<boolean>(true);
  availableDates: Date[] = [];
  selectedDate: Date | null = null;
  allTimeSlots: DataTimeSlotsDto[] = [];
  selectedSlot: DataTimeSlotsDto | null = null;
  dateSlotsMap: { [dateString: string]: TimeSlotsWithStatusDTO[] } = {};
  message: string = '';
  messageType: 'success' | 'danger' | 'info' | '' = '';

  // Pagination state using signals
  readonly currentPage = signal<number>(1);
  readonly pageSize = 4; // 3 rows of 4 items

  // Computed signal for paginated dates
  readonly paginatedDates = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.availableDates.slice(start, end);
  });

  readonly totalPages = computed(() =>
    Math.ceil(this.availableDates.length / this.pageSize)
  );

  ngOnInit(): void {
    this.activeRoute.params.subscribe({
      next: (e) => {
        this.id = e['id'];
        this.doctorsService.getById(this.id).subscribe({
          next: (response) => {
            this.errorFound = false;
            this.doctor = response;
            this.profileLoading.set(false);
          },
          error: (err) => {
            this.errorFound = true;
            this.alert.error(err.error?.title ?? 'not found');
            this.router.navigateByUrl('/notfound/doctor');
            this.errorMessage = err.error?.title;
          },
        });

        // Start loading dates here
        this.loadingDates.set(true);
        this.doctorProfileService
          .getTimeSlotsForBookingWithStatus(e['id'])
          .subscribe({
            next: (resp) => {
              resp.data.forEach((slot) => {
                const dateKey = slot.startTime.split('T')[0];
                if (!this.dateSlotsMap[dateKey]) {
                  this.dateSlotsMap[dateKey] = [];
                  this.availableDates.push(new Date(dateKey));
                }
                this.dateSlotsMap[dateKey].push({
                  startTime: slot.startTime,
                  endTime: slot.endTime,
                  isActive: slot.isActive,
                  maxCapacity: slot.maxCapacity,
                  bookedCount: slot.bookedCount,
                  id: slot.id,
                  status: slot.status,
                  doctorId: slot.doctorId,
                  isFull: slot.isFull,
                });
              });
              if (this.availableDates.length > 0) {
                this.selectDate(this.availableDates[0]);
              }
              // Once dates are loaded, set loading to false
              this.loadingDates.set(false);
            },
            error: (err) => {
              // this.alert.error(err.error.data);
              this.loadingDates.set(false); // Also set to false on error
            },
          });
      },
    });

    this.user = this.accountService.jwtTokenDecoder();
    this.appointmentService
      .getCustomerAppointments(this.user.userId)
      .subscribe({
        next: (resp) => {
          this.appointments = resp;
          console.log('appointments', this.appointments);
        },
        error: (e) => {
          if (e.status == 404) {
            console.log("didn't find any appointments");
          }
        },
      });
    this.loadUserBlogs();
  }

  CustomerHasThisAppointmentSlot(slot: TimeSlotsWithStatusDTO) {
    return this.appointmentService.customerHasAppointmentSlot(
      this.appointments,
      slot
    );
  }

  selectDate(date: Date | null): void {
    this.selectedDate = date;
    this.message = '';
    this.messageType = '';
  }

  getSlotsForSelectedDate(): TimeSlotsWithStatusDTO[] {
    if (!this.selectedDate) {
      return [];
    }
    const dateString = this.doctorProfileService.formatDate(this.selectedDate);
    return this.dateSlotsMap[dateString] || [];
  }

  bookAppointment(slot: TimeSlotsWithStatusDTO): void {
    const slotCustomer: TimeSlotsWithCustomerIdStatusBookingDTO = {
      customerId: this.accountService.jwtTokenDecoder().userId,
      bookedCount: slot.bookedCount,
      doctorId: slot.doctorId,
      endTime: slot.endTime,
      id: slot.id,
      isActive: slot.isActive,
      isFull: slot.isFull,
      maxCapacity: slot.maxCapacity,
      startTime: slot.startTime,
      status: slot.status,
    };

    console.log('bokking, ', slotCustomer);
    this.timeSlotCustService.canBookAppointment(slotCustomer).subscribe({
      next: (resp) => {
        this.selectedSlot = slot;
        this.router.navigateByUrl('/doctors/appointment', {
          state: { doctor: this.doctor, slot: this.selectedSlot },
        });
      },
      error: (err) => {
        console.log(err);
        console.log(slotCustomer);
        if (this.CustomerHasThisAppointmentSlot(slot))
          return this.alert.error('you already booked today');
        this.alert.error(err.error.data);
      },
    });
  }

  isSelected(date: Date): boolean {
    return this.selectedDate
      ? this.doctorProfileService.formatDate(this.selectedDate) ===
          this.doctorProfileService.formatDate(date)
      : false;
  }

  // --- Pagination Methods ---
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  previousPage(): void {
    this.goToPage(this.currentPage() - 1);
  }

  nextPage(): void {
    this.goToPage(this.currentPage() + 1);
  }

  getPages(): number[] {
    return Array.from({ length: this.totalPages() }, (_, i) => i + 1);
  }

  loadUserBlogs() {
    this.blogService
      .getBlogsByUserId(this.id, this.topicId, this.categoryId)
      .subscribe({
        next: (data) => {
          this.blogs = data;
          console.log(this.blogs);
          this.loadingBlogs = false;
        },
        error: (err) => {
          console.error('Error loading blogs', err);
          this.loadingBlogs = true;
        },
      });
  }
  getFullImageUrl(relativePath: string): string {
    return `${this.server}/${relativePath}`;
  }
}
