
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
import { CommonModule, CurrencyPipe, DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Assume these services and interfaces exist
import { AlertService } from '../../../core/services/alert-service';
import { AccountService } from '../../../core/services/account-service';
import { DoctorCustomerAppointmentService } from '../../doctor-customer-appointment/doctor-customer-appointment-service';
import { DoctorProfileAppointmentView } from '../../doctor-customer-appointment/doctor-profile-appointment-view';
import { AppointmentService } from '../../doctor-profile/appointment-service';
import { DoctorsService } from '../../doctors/doctors-service';

@Component({
  selector: 'app-doctor',
  standalone: true,

  imports: [
    RouterLink,
    DatePipe,
    CommonModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
  ],
  templateUrl: './doctor.html',
  styleUrl: './doctor.css',
})
export class Doctor implements OnInit {

  // --- Services Injected (good practice) ---
  private alert = inject(AlertService);
  private appointmentService = inject(AppointmentService);
  private doctorAppointmentService = inject(DoctorCustomerAppointmentService);
  private doctorService = inject(DoctorsService);
  private accountService = inject(AccountService);

  // --- Component State using Signals (Reactive Data Flow) ---
  // The raw data from the API
  appointmentRequests = signal<DoctorProfileAppointmentView[]>([]);

  // Filter and pagination state
  selectedStatusFilter = signal<string>('');
  sortOrder = signal<'asc' | 'desc'>('desc');
  currentPage = signal<number>(1);
  itemsPerPage = signal<number>(6);

  // --- Computed Signals for Reactive Filtering and Pagination ---

  // 1. Filtered and Sorted list (re-runs whenever requests, status, or sort order changes)
  filteredRequests = computed(() => {
    const requests = this.appointmentRequests();
    let filtered = requests;

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

    // Apply status filter
    if (this.selectedStatusFilter()) {
      filtered = filtered.filter(r => r.status === this.selectedStatusFilter());
    }

    // Apply sorting
    return filtered.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();
      return this.sortOrder() === 'asc' ? dateA - dateB : dateB - dateA;
    });
  });

  // 2. Paginated list (re-runs whenever filteredRequests or pagination state changes)
  paginatedRequests = computed(() => {
    const filtered = this.filteredRequests();
    const startIndex = (this.currentPage() - 1) * this.itemsPerPage();
    const endIndex = startIndex + this.itemsPerPage();
    return filtered.slice(startIndex, endIndex);
  });

  // 3. Computed for total pages (re-runs whenever filteredRequests or itemsPerPage changes)
  totalPages = computed(() => Math.ceil(this.filteredRequests().length / this.itemsPerPage()));
  totalItems = computed(() => this.filteredRequests().length);

  // --- Other Component Properties & Methods ---
  loadingAppointments = signal<boolean>(true);
  loadingProfile = signal<boolean>(true);
  doctor = signal<IDoctor>({} as IDoctor);
  profileData: Signal<IDoctor> = computed(() => this.doctor() ? this.doctor() : {} as IDoctor);
  statusArr: string[] = [];
  server = "https://localhost:7102";

  ngOnInit(): void {
    const user = this.accountService.jwtTokenDecoder();
    this.doctorService.getById(user.userId).subscribe({
      next: resp => {
        if (typeof resp !== 'string') {
          this.doctor.set(resp);
        }
      }
    });
    this.loadingProfile.set(false);

    this.doctorAppointmentService.getAppointmentsForDoctorProfileView().subscribe({
      next: resp => {
        this.appointmentRequests.set(resp);
        this.statusArr = Array.from(new Set(resp.map(e => e.status)));
        this.loadingAppointments.set(false);
        // We no longer need to call a function here, as computed signals handle the flow automatically.
      },
      error: err => {
        this.alert.error('Failed to load appointments.');
        this.loadingAppointments.set(false);
      }
    });
  }

  // --- Pagination Methods (now just update the signal) ---
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update(val => val + 1);
    }
  }

  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update(val => val - 1);
    }
  }

  getPages(): number[] {
    const pages: number[] = [];
    const total = this.totalPages();
    for (let i = 1; i <= total; i++) {
      pages.push(i);
    }
    return pages;
  }
  
  // --- Other methods remain largely unchanged, but now update signals ---
  getFullImageUrl(relativePath: string): string {
    if(typeof relativePath == 'string')
    return `${this.server}/${relativePath}`;
    return "";
  }

  cancelRequest(appointmentId: string) {
    this.appointmentService.cancelAppointment(appointmentId).subscribe({
      next: resp => {
        this.alert.success(resp.data);
        // Find and update the request in the signal
        this.appointmentRequests.update(requests => {
          const index = requests.findIndex(r => r.id === appointmentId);
          if (index !== -1) {
            requests[index].status = 'Cancelled';
          }
          return [...requests]; // Return a new array to trigger signal update
        });
      },
      error: err => {
        console.log(err);
        this.alert.error(err.data);
      }

    });
  }

  confirmRequest(appointmentId: string) {
    this.appointmentService.confirmAppointment(appointmentId).subscribe({
      next: resp => {
        this.alert.success(resp.data);
        this.appointmentRequests.update(requests => {
          const index = requests.findIndex(r => r.id === appointmentId);
          if (index !== -1) {
            requests[index].status = 'Confirmed';
            requests[index].bookedCount += 1;
          }
          return [...requests];
        });
      },
      error: err => {
        console.log(err);
        this.alert.error(err.data);
      }

    });
  }

  completeRequest(appointmentId: string) {
    this.appointmentService.completeAppointment(appointmentId).subscribe({
      next: resp => {
        this.alert.success(resp.data);
        this.appointmentRequests.update(requests => {
          const index = requests.findIndex(r => r.id === appointmentId);
          if (index !== -1) {
            requests[index].status = 'Completed';
          }
          return [...requests];
        });
      },
      error: err => {
        console.log(err);
        this.alert.error(err.data);
      }
    });
  }

}
