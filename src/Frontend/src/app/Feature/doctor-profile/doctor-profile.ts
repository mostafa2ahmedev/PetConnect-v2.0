import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { IDoctor } from '../doctors/idoctor';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DoctorsService } from '../doctors/doctors-service';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { IDoctorEdit } from '../doctor-edit-profile/idoctor-edit';
import { DoctorEditProfileService } from '../doctor-edit-profile/doctor-edit-profile-service';
import { DataTimeSlotsDto } from './data-time-slot-dto';
import { DoctorProfileService } from './doctor-profile-service';
import { AccountService } from '../../core/services/account-service';
import { JwtUser } from '../../core/models/jwt-user';
import { AppointmentService } from './appointment-service';
import { AppointmentDto } from './appointment-dto';
import { PetDetailsModel } from '../../models/pet-details';
import { AlertService } from '../../core/services/alert-service';
import { TimeSlotsWithStatusDTO } from './time-slots-with-status-dto';
import { TimeSlotsCustomerService } from './time-slots-customer-service';
import { TimeSlotsWithCustomerIdStatusBookingDTO } from './time-slots-with-customer-id-status-booking-dto';
@Component({
  selector: 'app-doctor-profile',
  imports: [CurrencyPipe, RouterLink, DatePipe, CommonModule],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css',
})
export class DoctorProfile implements OnInit {
  profileLoading = signal(true);
  activeRoute = inject(ActivatedRoute);
  doctorsService = inject(DoctorsService);
  accountService = inject(AccountService);
  timeSlotCustService = inject(TimeSlotsCustomerService);
  router = inject(Router);
  doctorProfileService = inject(DoctorProfileService);
  appointmentService = inject(AppointmentService);
  alert = inject(AlertService);
  server = "https://localhost:7102";
  id: string = "";
  doctor: IDoctor | string = "";

  errorMessage = null;
  errorFound: boolean = false;
  userId = undefined;
  userRole = undefined;
  user: JwtUser = { found: false, userRole: "", userId: "" };
  appointments: AppointmentDto[] = [];
  
  // State for loading dates and the data itself
  readonly loadingDates = signal<boolean>(true);

  availableDates: Date[] = [];
  selectedDate: Date | null = null;

  // All possible time slots for a given day (these will be cloned for each day)
  allTimeSlots: DataTimeSlotsDto[] = [];
  //---------------------------------------------------
  selectedSlot: DataTimeSlotsDto | null = null; // The slot selected for booking
  // selectedPet:PetDetailsModel = {} as PetDetailsModel;
  //---------------------------------------------------
  // A map to store the actual state of time slots for each date
  // Key: Date string (YYYY-MM-DD), Value: Array of AppointmentSlot
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

  readonly totalPages = computed(() => Math.ceil(this.availableDates.length / this.pageSize));


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
          error: err => {
            this.errorFound = true;
            this.alert.error(err.error?.title ?? "not found");
            this.router.navigateByUrl("/notfound/doctor");
            this.errorMessage = err.error?.title;
          }
        });

        // Start loading dates here
        this.loadingDates.set(true);
        this.doctorProfileService.getTimeSlotsForBookingWithStatus(e['id']).subscribe({
          next: resp => {
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
                isFull: slot.isFull
              });
            });
            if (this.availableDates.length > 0) {
              this.selectDate(this.availableDates[0]);
            }
            // Once dates are loaded, set loading to false
            this.loadingDates.set(false);
          },
          error: err => {
            this.alert.error(err.error.data);
            this.loadingDates.set(false); // Also set to false on error
          }
        });
      }
    });

    this.user = this.accountService.jwtTokenDecoder();
    this.appointmentService.getCustomerAppointments(this.user.userId).subscribe({
      next: resp => {
        this.appointments = resp;
      },
      error: e => {
        if (e.status == 404) {
          console.log("didn't find any appointments");
        }
      }
    });
  }

  CustomerHasThisAppointmentSlot(slot: TimeSlotsWithStatusDTO) {
    return this.appointmentService.customerHasAppointmentSlot(this.appointments, slot);
  }


          error: (err) => {
            this.errorFound = true;
            this.alert.error(err.error?.title ?? 'not found');
            this.router.navigateByUrl('/notfound/doctor');
            this.errorMessage = err.error?.title;
          },
        });

        /*** Replacing this with new DTO And new Service 
      this.doctorProfileService.getTimeSlotsForDoctor(e['id']).subscribe({
        next: resp => {
          // console.log(resp.data);
          resp.data.forEach((slot) => {
            // Extract the date part (YYYY-MM-DD) from the startTime
            const dateKey = slot.startTime.split('T')[0];
            const startTime = slot.startTime.split('T')[1];
            const endTime = slot.endTime.split('T')[1];
            if (!this.dateSlotsMap[dateKey]) {
              this.dateSlotsMap[dateKey] = [];
              this.availableDates.push(new Date(dateKey));
            }
            // Push the slot into the appropriate array
            this.dateSlotsMap[dateKey].push({
              startTime:this.doctorProfileService.convertTo12HourTimer(startTime),
              endTime:this.doctorProfileService.convertTo12HourTimer(endTime),
              isActive: slot.isActive,
              maxCapacity: slot.maxCapacity,
              bookedCount: slot.bookedCount,
              id :slot.id
            });
          });
          // Optionally, pre-select the first available date
          if (this.availableDates.length > 0) {
            this.selectDate(this.availableDates[0]);
          }
          // console.log(this.dateSlotsMap);
        }
      });*/

        // V2 Here
        this.doctorProfileService
          .getTimeSlotsForBookingWithStatus(e['id'])
          .subscribe({
            next: (resp) => {
              resp.data.forEach((slot) => {
                // Extract the date part (YYYY-MM-DD) from the startTime
                const dateKey = slot.startTime.split('T')[0];
                const startTime = slot.startTime.split('T')[1];
                const endTime = slot.endTime.split('T')[1];
                if (!this.dateSlotsMap[dateKey]) {
                  this.dateSlotsMap[dateKey] = [];
                  this.availableDates.push(new Date(dateKey));
                }
                // Push the slot into the appropriate array
                this.dateSlotsMap[dateKey].push({
                  // startTime:this.doctorProfileService.convertTo12HourTimer(startTime),
                  // endTime:this.doctorProfileService.convertTo12HourTimer(endTime),
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
              // Optionally, pre-select the first available date
              if (this.availableDates.length > 0) {
                this.selectDate(this.availableDates[0]);
              }
              // console.log(this.dateSlotsMap);
            },
          });
      },
    });
    // this.generateAvailableDates(); // Generate dates for the next 7 days

    this.user = this.accountService.jwtTokenDecoder();
    // console.log(this.user.userId);
    this.appointmentService
      .getCustomerAppointments(this.user.userId)
      .subscribe({
        next: (resp) => {
          // console.log(resp);
          this.appointments = resp;
        },
        error: (e) => {
          if (e.status == 404) {
            console.log("didn't find any appointments");
          }
        },
      });
  }

  CustomerHasThisAppointmentSlot(slot: TimeSlotsWithStatusDTO) {
    return this.appointmentService.customerHasAppointmentSlot(
      this.appointments,
      slot
    );
  }
  /**
   * Generates a list of available dates starting from tomorrow for the next 7 days.
   */
  // generateAvailableDates(): void {
  //   const today = new Date();
  //   today.setHours(0, 0, 0, 0); // Normalize to start of day

  //   for (let i = 1; i <= 7; i++) { // Next 7 days
  //     const nextDay = new Date(today);
  //     nextDay.setDate(today.getDate() + i);
  //     this.availableDates.push(nextDay);
  //   }
  // }

  /**
   * Selects a date to display its available time slots.
   * @param date The date selected by the user.
   */
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

    // this should be in appointment
    //   this.appointmentService.bookAppointment(slot.id).subscribe({
    //   next: resp=>{
    //     this.alert.success(resp.data);
    //     this.selectedSlot = slot; // Store the selected slot here!
    //   this.router.navigateByUrl("/doctors/appointment",{state:{doctor:this.doctor,slot:this.selectedSlot}});

    //   },
    //   error:err=>{
    //     console.log(err);
    //     console.log(slot)
    //           if (this.CustomerHasThisAppointmentSlot(slot))
    //                return this.alert.error("you already booked today");
    //     this.alert.error(err.error.data)
    //   }
    // })
    // Do nothing if already booked by the customer
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
      status: slot.status
    };
    this.timeSlotCustService.canBookAppointment(slotCustomer).subscribe({
      next: resp => {
        this.selectedSlot = slot;
        this.router.navigateByUrl("/doctors/appointment", { state: { doctor: this.doctor, slot: this.selectedSlot } });
      },
      error: err => {
        console.log(err);
        console.log(slotCustomer);
        if (this.CustomerHasThisAppointmentSlot(slot))
          return this.alert.error("you already booked today");
        this.alert.error(err.error.data);
      }
    });
  }


      status: slot.status,
    };
    this.timeSlotCustService.canBookAppointment(slotCustomer).subscribe({
      next: (resp) => {
        // console.log(resp.data)
        // this.alert.success(resp.data);
        this.selectedSlot = slot; // Store the selected slot here!
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

  /**
   * Checks if a date is the currently selected date.
   * Used for applying active styling.
   * @param date The date to check.
   * @returns True if it's the selected date, false otherwise.
   */
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
}
