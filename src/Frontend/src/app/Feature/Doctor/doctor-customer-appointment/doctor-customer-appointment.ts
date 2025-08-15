import { Component, OnInit, inject } from '@angular/core';
import { DoctorCustomerAppointmentService } from './doctor-customer-appointment-service';
import { PetDetailsModel, PetDetailsModel2 } from '../../../models/pet-details';
import { IDoctor } from '../../doctors/idoctor';
import { DataTimeSlotsDto } from '../doctor-profile/data-time-slot-dto';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { JwtUser } from '../../../core/models/jwt-user';
import { AccountService } from '../../../core/services/account-service';
import { Router, RouterLink } from '@angular/router';
import { AlertService } from '../../../core/services/alert-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-doctor-customer-appointment',
  imports: [FormsModule, ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './doctor-customer-appointment.html',
  styleUrl: './doctor-customer-appointment.css',
})
export class DoctorCustomerAppointment implements OnInit {
  router = inject(Router);
  doctorCustomerAppService = inject(DoctorCustomerAppointmentService);
  formBuilder = inject(FormBuilder);
  accountService = inject(AccountService);
  pets: PetDetailsModel2[] = [];
  doctor: IDoctor | string = {} as IDoctor;
  doctorObj: IDoctor = {} as IDoctor;
  slot: DataTimeSlotsDto = {} as DataTimeSlotsDto;
  doctorFullname: string = '';
  doctorId: string = '';
  user: JwtUser = {} as JwtUser;
  alertService = inject(AlertService);
  loading: boolean = true;
  appointmentForm!: FormGroup;
  ngOnInit(): void {
    this.doctor = history.state.doctor;
    this.slot = history.state.slot;
    this.user = this.accountService.jwtTokenDecoder();
    console.log(this.user.userId);
    if (typeof this.doctor != 'string') {
      this.doctorFullname = `${this.doctor.fName} ${this.doctor.lName}`;
      this.doctorId = this.doctor.id;
      this.doctorObj = this.doctor;
    }

    this.appointmentForm = this.formBuilder.group({
      petId: ['', Validators.required],
      doctorName: [{ value: '', disabled: true }, Validators.required],
      doctorId: [{ value: '' }],
      notes: [{ value: '', disabled: false }],
      slotId: ['', Validators.required],
      customerId: ['', Validators.required],
    });

    //set values programmatically
    this.appointmentForm.patchValue({
      doctorName: this.doctorFullname, // Replace dynamically
      slotId: this.slot.id, // Replace dynamically
      customerId: this.user.userId, // Replace dynamically
      doctorId: typeof this.doctor == 'string' ? '' : this.doctor.id,
    });
    this.doctorCustomerAppService.getPetsForCustomer().subscribe({
      next: (resp) => {
        this.pets = resp.data;
        this.loading = false;
        // console.log(this.pets)
        // console.log(this.doctor);
        // console.log(this.slot);
        // this.alertService.confirm("Created Successfully");
        if (this.pets.length == 0) {
          this.appointmentForm.get('notes')?.disable();
        } else {
          this.appointmentForm.get('notes')?.enable();
        }
      },
      error: (err) => {
        // this.alertService.error('Something Wrong Happened');
        this.loading = false;
      },
    });
  }

  confirmBooking() {
    if (
      this.appointmentForm.valid &&
      this.slot.bookedCount < this.slot.maxCapacity
    ) {
      this.doctorCustomerAppService
        .postPetAppointmentForDoctor(this.appointmentForm.value)
        .subscribe({
          next: (resp) => {
            // console.log(resp);
            this.alertService.success('successfully created appointment');
            this.router.navigateByUrl(`/doctors/${this.doctorId}`);
            this.doctorCustomerAppService
              .increaseBookedCountByOne(this.slot, this.doctorObj.id)
              .subscribe({
                next: (resp) => {},
              });
          },
          error: (err) => {
            this.alertService.error(err.error.data);
          },
        });
    }
  }
}
