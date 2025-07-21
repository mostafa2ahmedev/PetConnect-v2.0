import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DoctorsService } from './doctors-service';
import { FormsModule } from '@angular/forms';
import { IDoctor } from './idoctor';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

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
allDoctors:any;
specialty:number|null=null;
specialities:{specialityName:string,value:number}[]=
  [{specialityName:"Dog",value:0},
    {specialityName:"Cat",value:1},

  ];
ngOnInit() {
  this.search()
}
getAll(){
  // this.doctorService.getAll().subscribe();
}
getById(){
  // this.doctorService.getById().subscribe();
}
editById(){
  // this.doctorService.editById().subscribe();
}
search(){
  this.doctorService.getAll(this.name,this.maxPrice,this.specialty).subscribe(e=>{this.allDoctors= e; console.log(e)});
}
}
