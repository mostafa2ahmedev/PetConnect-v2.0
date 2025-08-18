import { Component,computed,inject,signal, ViewChild } from '@angular/core';
import { DoctorProfileService } from '../doctor-profile/doctor-profile-service' ;
import { AccountService } from '../../../core/services/account-service';
import { JwtUser } from '../../../core/models/jwt-user';
import { AppointmentService } from '../doctor-profile/appointment-service';
import { AppointmentDto } from '../doctor-profile/appointment-dto';
import { AlertService } from '../../../core/services/alert-service';
import { TimeSlotsWithStatusDTO } from '../doctor-profile/time-slots-with-status-dto';
import { TimeSlotsCustomerService } from '../doctor-profile/time-slots-customer-service';
import { TimeSlotsWithCustomerIdStatusBookingDTO } from '../doctor-profile/time-slots-with-customer-id-status-booking-dto';
import { Blog } from '../../Blog/blog-models';
import { BlogService } from '../../Blog/blog-service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DoctorsService } from '../../doctors/doctors-service';
import { IDoctor } from '../../doctors/idoctor';
import { DataTimeSlotsDto } from '../doctor-profile/data-time-slot-dto';
import { TabCVDDTO } from './tab-cvd-dto';
import { CommonModule, Location } from '@angular/common';
import { DoctorReviewModel } from '../../Review/models/doctor-review-model';
import { ReviewsService } from '../../Review/reviews-service';
import { Navigation, Pagination } from 'swiper/modules';
import Swiper from 'swiper';
import { NgbCarouselModule, NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-customer-view-doctor',
  imports: [CommonModule,RouterLink,NgbCarouselModule],
  templateUrl: './customer-view-doctor.html',
  styleUrl: './customer-view-doctor.css'
})
export class CustomerViewDoctor {
    // Inject services
  activeRoute = inject(ActivatedRoute);
  doctorsService = inject(DoctorsService);
  accountService = inject(AccountService);
  timeSlotCustService = inject(TimeSlotsCustomerService);
  blogService = inject(BlogService);
  router = inject(Router);
  doctorProfileService = inject(DoctorProfileService);
  appointmentService = inject(AppointmentService);
  alert = inject(AlertService);
  reviewService = inject(ReviewsService);
  location = inject(Location);

    // New properties for blog pagination
  blogsPerPage: number = 6; // You can adjust this number

  // General properties
  activeTab = "booking"
    tabs: TabCVDDTO[] = [
    {
      id: "booking",
      label: "Book Appointment",
      icon: '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path>',
    },
    {
      id: "reviews",
      label: "Reviews",
      icon: '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z"></path>',
    },
    {
      id: "blog",
      label: "Blogs",
      icon: '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"></path>',
    },
  ]

  profileLoading = signal(true);
  server = 'https://localhost:7102';
  id: string = '';
  doctor: IDoctor | string = '';
  errorMessage = null;
  errorFound: boolean = false;
  userId = undefined;
  userRole = undefined;
  user: JwtUser = { found: false, userRole: '', userId: '' };
  appointments: AppointmentDto[] = [];
  
  // Blog-specific properties
  blogs: Blog[] = [];
  loadingBlogs = true;
  topicId?: number;
  categoryId?: number;

  // Appointment-specific properties
  readonly loadingDates = signal<boolean>(true);
  availableDates: Date[] = [];
  selectedDate: Date | null = null;
  allTimeSlots: DataTimeSlotsDto[] = [];
  selectedSlot: DataTimeSlotsDto | null = null;
  dateSlotsMap: { [dateString: string]: TimeSlotsWithStatusDTO[] } = {};
  message: string = '';
  messageType: 'success' | 'danger' | 'info' | '' = '';
  readonly currentPage = signal<number>(1);
  readonly pageSize = 4; // 3 rows of 4 items

  // Testimonial-specific properties
  testimonials: DoctorReviewModel[] = [];
  @ViewChild(NgbCarousel) carousel!: NgbCarousel;
  
  // Computed signals for pagination
  readonly paginatedDates = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.availableDates.slice(start, end);
  });
  readonly totalPages = computed(() =>
    Math.ceil(this.availableDates.length / this.pageSize)
  );

  ngOnInit(): void {
    // Get doctor ID from URL parameters and fetch doctor data
    this.activeRoute.params.subscribe({
      next: (e) => {
        this.id = e['id'];
        this.doctorsService.getById(this.id).subscribe({
          next: (response) => {
            this.errorFound = false;
            this.doctor = response;
            this.profileLoading.set(false);

            // Once doctor data is loaded, fetch reviews
            this.loadDoctorReviews();
          },
          error: (err) => {
            this.errorFound = true;
            this.alert.error(err.error?.title ?? 'not found');
            this.router.navigateByUrl('/notfound/doctor');
            this.errorMessage = err.error?.title;
          },
        });

        // Fetch available appointment dates
        this.loadingDates.set(true);
        this.doctorProfileService.getTimeSlotsForBookingWithStatus(e['id']).subscribe({
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
            this.loadingDates.set(false);
          },
          error: (err) => {
            this.loadingDates.set(false);
          },
        });
      },
    });

    // Fetch user appointments and blogs
    this.user = this.accountService.jwtTokenDecoder();
    this.appointmentService.getCustomerAppointments(this.user.userId).subscribe({
      next: (resp) => {
        this.appointments = resp;
      },
      error: (e) => {
        if (e.status == 404) {
          console.log("didn't find any appointments");
        }
      },
    });
    this.loadUserBlogs();
  }

 

  // Blog methods
  loadUserBlogs() {
    this.blogService.getBlogsByUserId(this.id, this.topicId, this.categoryId).subscribe({
      next: (data) => {
        this.blogs = data;
        this.loadingBlogs = false;
      },
      error: (err) => {
        console.error('Error loading blogs', err);
        this.loadingBlogs = true;
      },
    });
  }

  // Appointment methods
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

    this.timeSlotCustService.canBookAppointment(slotCustomer).subscribe({
      next: (resp) => {
        this.selectedSlot = slot;
        this.router.navigateByUrl('/doctors/appointment', {
          state: { doctor: this.doctor, slot: this.selectedSlot },
        });
      },
      error: (err) => {
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

  // Pagination Methods
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

  // Review methods
  loadDoctorReviews() {
    if (this.id) {
      this.reviewService.getDoctorReviewsById(this.id).subscribe({
        next: (resp) => {
          this.testimonials = resp.data;
        },
        error: (err) => {
          console.error('Error loading reviews', err);
        },
      });
    }
  }

  initSwiper() {
    new Swiper('.testimonials-slider', {
      modules: [Navigation, Pagination],
      loop: true,
      speed: 800,
      autoplay: {
        delay: 5000,
      },
      slidesPerView: 1,
      spaceBetween: 30,
      pagination: {
        el: '.swiper-pagination',
        type: 'bullets',
        clickable: true,
      },
      breakpoints: {
        768: {
          slidesPerView: 1,
        },
        1200: {
          slidesPerView: 1,
        },
      },
    });
  }

  goBack() {
    this.location.back();
  }

  makeReview() {
    this.router.navigate(['doctors', 'add-review'], {
      state: { doctorObj: this.doctor },
    });
  }

  arrayOfRating(rating: number) {
    return Array.from({ length: rating }, (_, i) => i);
  }

  // Common utility method
  getFullImageUrl(relativePath: string): string {
    return `${this.server}/${relativePath}`;
  }
  getFullCertifactionUrl(relativePath:string):string{
    return `${this.server}${relativePath}`;

  }
    setActiveTab(tabId: string): void {
    this.activeTab = tabId
    console.log(this.activeTab)
  }

    prev() {
    this.carousel.prev();
  }

  next() {
    this.carousel.next();
  }
    paginatedBlogs(): any[] {
    const startIndex = (this.currentPage() - 1) * this.blogsPerPage;
    const endIndex = startIndex + this.blogsPerPage;
    return this.blogs.slice(startIndex, endIndex);
  }

  /**
   * Calculates the total number of pages needed for all blogs.
   * @returns {number} The total number of pages.
   */
  // totalPages(): number {
  //   return Math.ceil(this.blogs.length / this.blogsPerPage);
  // }

  /**
   * Changes the current page for blogs.
   * @param {number} pageNumber The page number to navigate to.
   */
  blogCurrentPage=1;
  blogGoToPage(pageNumber: number): void {
    if (pageNumber >= 1 && pageNumber <= this.totalPages()) {
      this.blogCurrentPage = pageNumber;
    }
  }
}
