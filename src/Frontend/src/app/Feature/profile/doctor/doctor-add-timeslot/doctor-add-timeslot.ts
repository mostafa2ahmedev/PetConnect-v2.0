import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { HttpClient } from '@angular/common/http';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DoctorAddTimeslotService } from './doctor-add-timeslot-service';
import { Router } from '@angular/router';
import { JwtUser } from '../../../../core/models/jwt-user';
import { AccountService } from '../../../../core/services/account-service';
import { AlertService } from '../../../../core/services/alert-service';

@Component({
  selector: 'app-doctor-add-timeslot',
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatButtonModule,
    MatCheckboxModule,
    MatNativeDateModule,
    ReactiveFormsModule,
  ],
  templateUrl: './doctor-add-timeslot.html',
  styleUrl: './doctor-add-timeslot.css',
})
export class DoctorAddTimeslot {
  doctor!: JwtUser;
  accountService = inject(AccountService);
  doctorAddTsService = inject(DoctorAddTimeslotService);
  alertService = inject(AlertService);
  router = inject(Router);

  scheduleForm: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.scheduleForm = this.fb.group({
      scheduleDate: [''],
      startTime: [''],
      endTime: [''],
      maxCapacity: [1],
      isActive: [true],
    });
  }

  submitSchedule() {
    this.doctor = this.accountService.jwtTokenDecoder();
    const doctorId = this.doctor.userId;
    const { scheduleDate, startTime, endTime, maxCapacity, isActive } =
      this.scheduleForm.value;

    // Combine date and time into ISO format
    const combinedDateStartTime = new Date(
      `${this.formatDate(scheduleDate)}T${startTime}:00Z`
    ).toISOString();
    const combinedDateEndTime = new Date(
      `${this.formatDate(scheduleDate)}T${endTime}:00Z`
    ).toISOString();

    const scheduleDto = {
      doctorId: doctorId, // Replace with actual doctor ID (from auth or selection)
      startTime: combinedDateStartTime,
      endTime: combinedDateEndTime, // You can add duration logic later
      maxCapacity: maxCapacity,
      bookedCount: 0,
      isActive: isActive,
    };

    this.doctorAddTsService.addSchedule(scheduleDto).subscribe({
      next: (res) => {
        this.alertService.success('Schedule saved!');
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        console.log(err);
        this.alertService.error(err.error.data);
        // this.alertService.error('Something went wrong')
      },
    });
  }
  onCancel(): void {
    this.router.navigate(['/schedules']);
  }
  // Helper to format the date (yyyy-MM-dd)
  formatDate(dateObj: Date): string {
    const yyyy = dateObj.getFullYear();
    const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
    const dd = String(dateObj.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}
