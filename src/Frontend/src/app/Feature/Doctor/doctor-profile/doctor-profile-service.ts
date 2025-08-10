import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { DataTimeSlotsDto } from './data-time-slot-dto';
import { ApiResponse } from '../../../models/api-response';
import { TimeSlotsWithStatusDTO } from './time-slots-with-status-dto';

@Injectable({
  providedIn: 'root',
})
export class DoctorProfileService {
  httpClient = inject(HttpClient);
  server = 'https://localhost:7102';
  getTimeSlotsForDoctor(
    doctorId: string
  ): Observable<{ data: DataTimeSlotsDto[]; statusCode: number }> {
    return this.httpClient.get<{
      data: DataTimeSlotsDto[];
      statusCode: number;
    }>(`${this.server}/api/TimeSlots/${doctorId}`);
  }
  getTimeSlotsForBookingWithStatus(
    doctorId: string
  ): Observable<ApiResponse<TimeSlotsWithStatusDTO[]>> {
    return this.httpClient.get<ApiResponse<TimeSlotsWithStatusDTO[]>>(
      `${this.server}/api/TimeSlots/view/${doctorId}`
    );
  }
  convertToTimeOnly(date: Date) {
    const timeOnly = date.toLocaleDateString('en-Us', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      timeZone: 'UTC',
    });
    return timeOnly;
  }
  /**
   * Formats a Date object into a YYYY-MM-DD string.
   * @param date The Date object to format.
   * @returns A string in YYYY-MM-DD format.
   */
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Month is 0-indexed
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  /**
   * Formats a Date object to a more readable string (e.g., "Mon, Jan 01").
   * @param date The Date object to format.
   * @returns A readable date string.
   */
  getReadableDate(date: Date): string {
    return date.toLocaleDateString('en-US', {
      weekday: 'short',
      month: 'short',
      day: 'numeric',
    });
  }
  convertTo12HourTimer(inputTime: string) {
    // const inputTime = "19:47:37";
    const date = new Date(`1970-01-01T${inputTime}`); // Just attach a dummy date

    const formattedTime = date.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true,
    });
    return formattedTime;
  }
}
