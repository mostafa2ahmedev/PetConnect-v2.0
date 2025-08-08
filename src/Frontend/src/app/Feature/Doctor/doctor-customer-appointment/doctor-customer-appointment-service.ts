import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from '../../../core/services/account-service';
import { PetDetailsModel } from '../../../models/pet-details';
import { Observable } from 'rxjs';
import { PetsForCustomerDto } from './pets-for-customer-dto';
import { AddAppointmentDto } from './add-appointment-dto';
import { DataTimeSlotsDto } from '../doctor-profile/data-time-slot-dto';
import { EditTimeslotDto } from '../../Timeslot/edit-timeslot-dto';
import { DoctorCustomerAppointmentView } from './doctor-customer-appointment-view';
import { DoctorProfileAppointmentView } from './doctor-profile-appointment-view';

@Injectable({
  providedIn: 'root',
})
export class DoctorCustomerAppointmentService {
  httpClient = inject(HttpClient);
  accountService = inject(AccountService);
  server = 'https://localhost:7102';
  getPetsForCustomer(): Observable<{
    statusCode: number;
    data: PetsForCustomerDto[];
  }> {
    const user = this.accountService.jwtTokenDecoder();
    const headers = new HttpHeaders({ Accept: 'application/json' });
    return this.httpClient.get<{
      statusCode: number;
      data: PetsForCustomerDto[];
    }>(`${this.server}/api/Pet/Customer/${user.userId}`, { headers });
    //  return this.httpClient.get<{statusCode:number,result:PetsForCustomerDto[]}>(`${this.server}/api/Customer/CustomerPets`, { headers });
  }
  postPetAppointmentForDoctor(body: any) {
    return this.httpClient.post(`${this.server}/api/Appointment`, body);
  }

  increaseBookedCountByOne(slot: DataTimeSlotsDto, doctorId: string) {
    slot.bookedCount += 1;
    const newEditibleSlot: EditTimeslotDto = {
      Id: slot.id,
      DoctorId: doctorId,
      StartTime: slot.startTime,
      EndTime: slot.endTime,
      MaxCapacity: slot.maxCapacity.toString(),
      BookedCount: slot.bookedCount.toString(),
      IsActive: slot.isActive,
    };
    return this.httpClient.put(`${this.server}/api/TimeSlots`, newEditibleSlot);
  }
  getAppointmentsForCurrentDoctor(): Observable<
    DoctorCustomerAppointmentView[]
  > {
    const user = this.accountService.jwtTokenDecoder();
    return this.httpClient.get<DoctorCustomerAppointmentView[]>(
      `${this.server}/api/Appointment/Doctor/${user.userId}`
    );
  }

  getAppointmentsForDoctorProfileView(): Observable<
    DoctorProfileAppointmentView[]
  > {
    const user = this.accountService.jwtTokenDecoder();
    return this.httpClient.get<DoctorProfileAppointmentView[]>(
      `${this.server}/api/Appointment/DoctorProfile/${user.userId}`
    );
  }
}
