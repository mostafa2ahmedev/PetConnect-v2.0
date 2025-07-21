import { Component, inject, OnInit } from '@angular/core';
import { IDoctor } from '../doctors/idoctor';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DoctorsService } from '../doctors/doctors-service';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-doctor-profile',
  imports: [CurrencyPipe,RouterLink],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfile implements OnInit{
  activeRoute = inject(ActivatedRoute);
  doctorsService = inject(DoctorsService);
  server = "https://localhost:7102";
  id:string="";
doctor:IDoctor|string="";

ngOnInit(): void {
  this.activeRoute.params.subscribe(e=>this.id=e['id']);
  this.doctorsService.getById(this.id).subscribe(e=>this.doctor= e)
}
}
