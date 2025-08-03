import { Injectable,inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataTimeSlotsDto } from '../doctor-profile/data-time-slot-dto';
import { ApiResponse } from '../../models/api-response';
import { ShowAllDoctorTimeslotsDto } from './show-all-doctor-timeslots-dto';

@Injectable({
  providedIn: 'root'
})
export class ShowAllDoctorTimeslotsService {
  accountService = inject(AccountService);
  httpClient= inject(HttpClient);
  server='https://localhost:7102';
  ShowAllTimeSlots():Observable<{statusCode:number,data: DataTimeSlotsDto[]}>{
    const user = this.accountService.jwtTokenDecoder();
    const userId = user.userId;
    return this.httpClient.get<{statusCode:number,data: DataTimeSlotsDto[]}>(`${this.server}/api/TimeSlots/all/${userId}`)
  }

  changeStatusToActive(slot:ShowAllDoctorTimeslotsDto):Observable<ApiResponse<string>>{
    return this.httpClient.put<ApiResponse<string>>(`${this.server}/api/TimeSlots/Active`,slot,    {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }
}
