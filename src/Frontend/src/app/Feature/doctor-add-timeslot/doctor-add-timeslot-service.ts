import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DoctorAddTimeslotDto } from './doctor-add-timeslot-dto';
import { AccountService } from '../../core/services/account-service';
import { DataTimeSlotsDto } from '../doctor-profile/data-time-slot-dto';

@Injectable({
  providedIn: 'root'
})
export class DoctorAddTimeslotService {
  httpClient = inject(HttpClient);
  accountService = inject(AccountService);
  server = 'https://localhost:7102';

  addSchedule(scheduleDto:DoctorAddTimeslotDto){
   return this.httpClient.post(`${this.server}/api/TimeSlots`, scheduleDto)
  }

}
