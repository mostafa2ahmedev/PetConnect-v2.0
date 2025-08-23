import {
  Component,
  computed,
  inject,
  type Signal,
  signal,
} from '@angular/core';
import { DoctorCustomerAppointmentService } from '../../../Doctor/doctor-customer-appointment/doctor-customer-appointment-service';
import type { DoctorProfileAppointmentView } from '../../../Doctor/doctor-customer-appointment/doctor-profile-appointment-view';
import { RouterLink, RouterModule } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlertService } from '../../../../core/services/alert-service';
import { AppointmentService } from '../../../Doctor/doctor-profile/appointment-service';
import { DoctorsService } from '../../../doctors/doctors-service';
import { AccountService } from '../../../../core/services/account-service';
import type { IDoctor } from '../../../doctors/idoctor';

@Component({
  selector: 'app-doc-appointments',
  imports: [RouterLink, CommonModule, FormsModule, RouterModule, DatePipe],
  templateUrl: './doc-appointmentets.html',
  styleUrl: './doc-appointmentets.css',
})
export class DocAppointmentets {
  // --- Services Injected ---
  private alert = inject(AlertService);
  private appointmentService = inject(AppointmentService);
  private doctorAppointmentService = inject(DoctorCustomerAppointmentService);
  private doctorService = inject(DoctorsService);
  private accountService = inject(AccountService);

  // --- Component State using Signals ---
  appointmentRequests = signal<DoctorProfileAppointmentView[]>([]);
  selectedStatusFilter = signal<string>('');
  sortOrder = signal<'asc' | 'desc'>('desc');
  currentPage = signal<number>(1);
  itemsPerPage = signal<number>(6);
  loadingAppointments = signal<boolean>(true);
  loadingProfile = signal<boolean>(true);
  doctor = signal<IDoctor>({} as IDoctor);

  // --- Computed Signals ---
  profileData: Signal<IDoctor> = computed(() =>
    this.doctor() ? this.doctor() : ({} as IDoctor)
  );

  filteredRequests = computed(() => {
    const requests = this.appointmentRequests();
    let filtered = requests;

    // Apply status filter
    if (this.selectedStatusFilter()) {
      filtered = filtered.filter(
        (r) => r.status === this.selectedStatusFilter()
      );
    }

    // Apply sorting
    return filtered.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();
      return this.sortOrder() === 'asc' ? dateA - dateB : dateB - dateA;
    });
  });

  paginatedRequests = computed(() => {
    const filtered = this.filteredRequests();
    const startIndex = (this.currentPage() - 1) * this.itemsPerPage();
    const endIndex = startIndex + this.itemsPerPage();
    return filtered.slice(startIndex, endIndex);
  });

  totalPages = computed(() =>
    Math.ceil(this.filteredRequests().length / this.itemsPerPage())
  );

  totalItems = computed(() => this.filteredRequests().length);

  // --- Component Properties ---
  statusArr: string[] = [];
  server = 'https://localhost:7102';

  ngOnInit(): void {
    this.loadDoctorProfile();
    this.loadAppointments();
  }

  private loadDoctorProfile(): void {
    const user = this.accountService.jwtTokenDecoder();
    this.doctorService.getById(user.userId).subscribe({
      next: (resp) => {
        if (typeof resp !== 'string') {
          this.doctor.set(resp);
        }
        this.loadingProfile.set(false);
      },
      error: (err) => {
        console.error('Failed to load doctor profile:', err);
        this.loadingProfile.set(false);
      },
    });
  }

  private loadAppointments(): void {
    this.doctorAppointmentService
      .getAppointmentsForDoctorProfileView()
      .subscribe({
        next: (resp) => {
          this.appointmentRequests.set(resp);
          this.statusArr = Array.from(new Set(resp.map((e) => e.status)));
          this.loadingAppointments.set(false);
        },
        error: (err) => {
          console.error('Failed to load appointments:', err);
          this.alert.error('Failed to load appointments.');
          this.loadingAppointments.set(false);
        },
      });
  }

  // --- Pagination Methods ---
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((val) => val + 1);
    }
  }

  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update((val) => val - 1);
    }
  }

  getPages(): number[] {
    const pages: number[] = [];
    const total = this.totalPages();
    const current = this.currentPage();

    // Show max 5 pages around current page
    const maxPages = 5;
    let startPage = Math.max(1, current - Math.floor(maxPages / 2));
    const endPage = Math.min(total, startPage + maxPages - 1);

    // Adjust start if we're near the end
    if (endPage - startPage < maxPages - 1) {
      startPage = Math.max(1, endPage - maxPages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  // --- Utility Methods ---
  getFullImageUrl(relativePath: string): string {
    if (typeof relativePath === 'string' && relativePath.trim()) {
      return `${this.server}/${relativePath}`;
    }
    return '';
  }

  getStatusClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'pending':
        return 'pending';
      case 'confirmed':
        return 'confirmed';
      case 'completed':
        return 'completed';
      case 'cancelled':
        return 'cancelled';
      default:
        return 'pending';
    }
  }

  // --- Appointment Actions ---
  cancelRequest(appointmentId: string): void {
    if (!appointmentId) return;

    this.appointmentService.cancelAppointment(appointmentId).subscribe({
      next: (resp) => {
        this.alert.success(resp.data || 'Appointment cancelled successfully');
        this.updateAppointmentStatus(appointmentId, 'Cancelled');
      },
      error: (err) => {
        console.error('Failed to cancel appointment:', err);
        this.alert.error(err.data || 'Failed to cancel appointment');
      },
    });
  }

  confirmRequest(appointmentId: string): void {
    if (!appointmentId) return;

    this.appointmentService.confirmAppointment(appointmentId).subscribe({
      next: (resp) => {
        this.alert.success(resp.data || 'Appointment confirmed successfully');
        this.updateAppointmentStatus(appointmentId, 'Confirmed', true);
      },
      error: (err) => {
        console.error('Failed to confirm appointment:', err);
        this.alert.error(err.data || 'Failed to confirm appointment');
      },
    });
  }

  completeRequest(appointmentId: string): void {
    if (!appointmentId) return;

    this.appointmentService.completeAppointment(appointmentId).subscribe({
      next: (resp) => {
        this.alert.success(resp.data || 'Appointment completed successfully');
        this.updateAppointmentStatus(appointmentId, 'Completed');
      },
      error: (err) => {
        console.error('Failed to complete appointment:', err);
        this.alert.error(err.data || 'Failed to complete appointment');
      },
    });
  }

  private updateAppointmentStatus(
    appointmentId: string,
    newStatus: string,
    incrementBookedCount = false
  ): void {
    this.appointmentRequests.update((requests) => {
      const index = requests.findIndex((r) => r.id === appointmentId);
      if (index !== -1) {
        const updatedRequest = { ...requests[index] };
        updatedRequest.status = newStatus;

        if (incrementBookedCount) {
          updatedRequest.bookedCount += 1;
        }

        requests[index] = updatedRequest;
      }
      return [...requests];
    });
  }

  // --- TrackBy Functions for Performance ---
  trackByAppointment(
    index: number,
    item: DoctorProfileAppointmentView
  ): string {
    return item.id;
  }

  trackByStatus(index: number, item: string): string {
    return item;
  }

  trackByPage(index: number, item: number): number {
    return item;
  }

  /**
   * Safely get the first character of a name, handling both string and number types
   */
  getInitial(name: string | number): string {
    if (typeof name === 'string' && name.length > 0) {
      return name.charAt(0).toUpperCase();
    }
    if (typeof name === 'number') {
      return name.toString().charAt(0);
    }
    return '?';
  }
}
