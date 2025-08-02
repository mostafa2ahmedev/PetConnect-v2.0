import {
  Component,
  input,
  OnInit,
  ChangeDetectorRef,
  inject,
  Signal,
  computed,
  signal,
} from '@angular/core';
import { IDoctor } from '../../doctors/idoctor';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AlertService } from '../../../core/services/alert-service';
import { AccountService } from '../../../core/services/account-service';
import { DoctorCustomerAppointmentService } from '../../doctor-customer-appointment/doctor-customer-appointment-service';
import { DoctorProfileAppointmentView } from '../../doctor-customer-appointment/doctor-profile-appointment-view';
import { AppointmentService } from '../../doctor-profile/appointment-service';
import { DoctorsService } from '../../doctors/doctors-service';
@Component({
  selector: 'app-doctor',
  imports: [
    RouterLink,
    DatePipe,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './doctor.html',
  styleUrl: './doctor.css',
})
export class Doctor implements OnInit {
  loadingAppointments: boolean = true;
  loadingProfile: boolean = true;
  ReceivedRequests: any[] = [];
  profileData!: Signal<IDoctor>; // Instead of a large ViewModel type
  petCount: number = 0;
  selectedStatusFilter: string = '';
  sortOrder: 'asc' | 'desc' = 'desc';
  // sentRequests: AdoptionResponse[] = [];
  requestedPetIds: number[] = [];
  appointmentRequests: DoctorProfileAppointmentView[] = [];
  statusArr: string[] = [];
  server = 'https://localhost:7102';
  doctor = signal<IDoctor>({} as IDoctor);
  // private accountService: AccountService=inject(AccountService);
  private alert: AlertService = inject(AlertService);
  private appointmentService: AppointmentService = inject(AppointmentService);
  private doctorAppointmentService: DoctorCustomerAppointmentService = inject(
    DoctorCustomerAppointmentService
  );
  private doctorService = inject(DoctorsService);
  private accountService = inject(AccountService);
  // private changeDetector: ChangeDetectorRef=inject(ChangeDetectorRef)
  ngOnInit(): void {
    const user = this.accountService.jwtTokenDecoder();
    this.doctorService.getById(user.userId).subscribe({
      next: (resp) => {
        if (typeof resp !== 'string') this.doctor.set(resp);
      },
    });
    console.log(this.doctor().id);
    this.profileData = computed(() =>
      this.doctor() ? this.doctor() : ({} as IDoctor)
    );
    this.loadingProfile = false;
    this.doctorAppointmentService
      .getAppointmentsForDoctorProfileView()
      .subscribe({
        next: (resp) => {
          this.appointmentRequests = resp;
          console.log(resp);
          this.statusArr = Array.from(
            new Set(this.appointmentRequests.map((e) => e.status))
          );

          this.loadingAppointments = false;
        },
      });
  }
  get filteredAndSortedRequests() {
    let filtered = this.appointmentRequests;

    if (this.selectedStatusFilter) {
      filtered = filtered.filter((r) => r.status === this.selectedStatusFilter);
    }

    return filtered.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();
      return this.sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    });
  }
  getFullImageUrl(relativePath: string): string {
    return `${this.server}/${relativePath}`;
  }

  cancelRequest(appointmentId: string) {
    this.appointmentService.cancelAppointment(appointmentId).subscribe({
      next: (resp) => {
        this.alert.success(resp.data);
        const target = this.appointmentRequests.find(
          (r) => r.id === appointmentId
        );
        if (target) target.status = 'Cancelled';
      },
      error: (err) => {
        console.log(err);
        this.alert.error(err.data);
      },
    });
  }

  confirmRequest(appointmentId: string) {
    this.appointmentService.confirmAppointment(appointmentId).subscribe({
      next: (resp) => {
        this.alert.success(resp.data);
        const target = this.appointmentRequests.find(
          (r) => r.id === appointmentId
        );
        if (target) {
          target.status = 'Confirmed';
          target.bookedCount += 1;
        }
      },
      error: (err) => {
        console.log(err);
        this.alert.error(err.data);
      },
    });
  }

  completeRequest(appointmentId: string) {
    this.appointmentService.completeAppointment(appointmentId).subscribe({
      next: (resp) => {
        this.alert.success(resp.data);
        const target = this.appointmentRequests.find(
          (r) => r.id === appointmentId
        );
        if (target) target.status = 'Completed';
      },
      error: (err) => {
        console.log(err);
        this.alert.error(err.data);
      },
    });
  }

  // bookRequest(appointmentId:string){
  //       this.appointmentService.bookAppointment(appointmentId).subscribe({
  // next: resp=>{
  //   this.alert.success(resp.data);
  // },
  // error:err=>{
  //   console.log(err);
  //   this.alert.error(err.data)
  // }
  // }
}
