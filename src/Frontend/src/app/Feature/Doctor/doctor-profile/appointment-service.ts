import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { AppointmentDto } from './appointment-dto';
import { Observable } from 'rxjs';
import { DataTimeSlotsDto } from './data-time-slot-dto';
import { ApiResponse } from '../../../models/api-response';
@Injectable({
  providedIn: 'root',
})
export class AppointmentService {
  server = 'https://localhost:7102';
  httpClient = inject(HttpClient);
  getCustomerAppointments(customerId: string): Observable<AppointmentDto[]> {
    return this.httpClient.get<AppointmentDto[]>(
      `${this.server}/api/Appointment/Customer/${customerId}`
    );
  }
  getDetailedCustomerAppointments(): Observable<AppointmentDto[]> {
    return this.httpClient.get<AppointmentDto[]>(
      `${this.server}/api/Appointment/Customer-details/`
    );
  }

  customerHasAppointmentSlot(
    arrayOfAppointments: AppointmentDto[],
    slot: DataTimeSlotsDto
  ): boolean {
    return arrayOfAppointments.some(
      (appointment) => appointment.slotId == slot.id
    );
  }

  completeAppointment(appointmentId: string): Observable<ApiResponse<string>> {
    return this.httpClient.put<ApiResponse<string>>(
      `${this.server}/api/Appointment/${appointmentId}/complete`,
      null
    );
  }
  confirmAppointment(appointmentId: string): Observable<ApiResponse<string>> {
    return this.httpClient.put<ApiResponse<string>>(
      `${this.server}/api/Appointment/${appointmentId}/confirm`,
      null
    );
  }
  cancelAppointment(appointmentId: string): Observable<ApiResponse<string>> {
    return this.httpClient.put<ApiResponse<string>>(
      `${this.server}/api/Appointment/${appointmentId}/cancel`,
      null
    );
  }
  bookAppointment(appointmentId: string): Observable<ApiResponse<string>> {
    return this.httpClient.put<ApiResponse<string>>(
      `${this.server}/api/Appointment/${appointmentId}/book`,
      null
    );
  }
}
