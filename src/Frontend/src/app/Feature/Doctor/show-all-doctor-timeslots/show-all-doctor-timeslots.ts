import { Component, inject, OnInit } from '@angular/core';
import { ShowAllDoctorTimeslotsService } from '../show-all-doctor-timeslots-service';
import { DataTimeSlotsDto } from '../../doctor-profile/data-time-slot-dto';
import { DatePipe } from '@angular/common';
import { AlertService } from '../../../core/services/alert-service';
import { ShowAllDoctorTimeslotsDto } from '../show-all-doctor-timeslots-dto';

@Component({
  selector: 'app-show-all-doctor-timeslots',
  imports: [DatePipe],
  templateUrl: './show-all-doctor-timeslots.html',
  styleUrl: './show-all-doctor-timeslots.css'
})
export class ShowAllDoctorTimeslots implements OnInit {
  showAllDocsTsService = inject(ShowAllDoctorTimeslotsService);
  allTimeSlots: DataTimeSlotsDto[]=[]
  alertService = inject(AlertService);
  ngOnInit(): void {
    this.showAllDocsTsService.ShowAllTimeSlots().subscribe({
      next:resp=>{
        // console.log(resp.data)
        this.allTimeSlots = resp.data;
      } 
    })
  }
changeStatusToActive(slot:ShowAllDoctorTimeslotsDto){

  this.showAllDocsTsService.changeStatusToActive(slot).subscribe({
    next:resp=>{
      slot.isActive = !slot.isActive; // Update the slot's isActive property
      console.log(resp);
      console.log(slot);
      this.alertService.success(resp.data);
    },
    error:err=>{
      console.error(err.data);
    }
  })

}
}
