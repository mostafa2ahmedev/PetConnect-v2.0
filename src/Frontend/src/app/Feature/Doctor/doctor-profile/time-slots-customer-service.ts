import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { TimeSlotsWithStatusDTO } from './time-slots-with-status-dto';
import { ApiResponse } from '../../../models/api-response';
import { TimeSlotsWithCustomerIdStatusBookingDTO } from './time-slots-with-customer-id-status-booking-dto';

@Injectable({
  providedIn: 'root',
})
export class TimeSlotsCustomerService {
  httpClient = inject(HttpClient);
  server = `https://localhost:7102`;
  canBookAppointment(slot: TimeSlotsWithCustomerIdStatusBookingDTO) {
    return this.httpClient.post<ApiResponse<string>>(
      `${this.server}/api/TimeSlots/Book`,
      slot
    );
  }
}
