import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DoctorsService } from './doctors-service';
import { FormsModule } from '@angular/forms';
import { IDoctor } from './idoctor';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ISpeciality } from './ispeciality';

@Component({
  selector: 'app-doctors',
  imports: [FormsModule,CurrencyPipe,RouterLink],
  templateUrl: './doctors.html',
  styleUrl: './doctors.css'
})
export class Doctors implements OnInit{
  server = "https://localhost:7102";
doctorService = inject(DoctorsService);
name:string = "";
maxPrice:number|null=null;
allDoctors:string|IDoctor[] = [];
specialty:number|null=null;
specialities:ISpeciality[]=
  [{specialityName:"Dog",value:0},
    {specialityName:"Cat",value:1},

  ];
errorFound:boolean=false;
errorMesseage:string ="";
ngOnInit() {
  this.search()
}

search(){
  this.doctorService.getAll(this.name,this.maxPrice,this.specialty).subscribe({
    next:e=>{
      this.errorFound=false;
      this.allDoctors= e;},
    error:err=>{
      this.errorFound=true;
      this.errorMesseage=err?.error
      console.log(this.errorMesseage)
      
    }
  })
}
}
