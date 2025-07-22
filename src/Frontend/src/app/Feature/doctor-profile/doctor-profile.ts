import { Component, inject, OnInit } from '@angular/core';
import { IDoctor } from '../doctors/idoctor';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DoctorsService } from '../doctors/doctors-service';
import { CurrencyPipe } from '@angular/common';
import { IDoctorEdit } from '../doctor-edit-profile/idoctor-edit';

@Component({
  selector: 'app-doctor-profile',
  imports: [CurrencyPipe,RouterLink],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfile implements OnInit{
  activeRoute = inject(ActivatedRoute);
  doctorsService = inject(DoctorsService);
  router = inject(Router);
  server = "https://localhost:7102";
  id:string="";
  doctor:IDoctor|string="";
  errorMessage=null;
  errorFound:boolean=false;
ngOnInit(): void {
  this.activeRoute.params.subscribe({
    next: (e) => {
      this.id = e['id'];
      this.doctorsService.getById(this.id).subscribe({

        next:(response) => {
          this.errorFound=false;
          this.doctor = response

        },
      
        error: err=>{
                this.errorFound=true;
                this.router.navigateByUrl("/notfound/doctor")
                this.errorMessage = err.error?.title
        }});
    }
  });
}
}
