import {  CommonModule } from '@angular/common';
import { Component, DoCheck, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../../core/services/account-service';
import { JwtUser } from '../../core/models/jwt-user';
import { IDoctor } from '../doctors/idoctor';
import { CustomerDto } from '../profile/customer-dto';

@Component({
  selector: 'app-header',
  imports: [RouterLink,CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit  {
  userFullname: string = "";
  user: JwtUser = {} as JwtUser;
  userId: string = "";
  caller: string = "";
  doctor: IDoctor = {} as IDoctor;
  customer: CustomerDto = {} as CustomerDto;

  constructor(private accontService : AccountService,private router : Router) {}

  
  ngOnInit(): void {
        // Check if the user is authenticated and set the userFullname accordingly
     if(this.accontService.isCustomer())
    this.accontService.getCustomerData()?.subscribe({
  
      next:resp=>{
      this.user = this.accontService.jwtTokenDecoder();
      this.userId = this.user.userId;
      if(resp){
        this.userFullname= `${resp.data.fName} ${resp.data.lName}`;
        resp
        this.caller="";
        this.customer = resp.data;
      }
    }})
    else if (this.accontService.isDoctor())
          this.accontService.getDoctorData()?.subscribe({
      next:resp=>{
      if(resp && typeof resp != 'string'){
        this.userFullname= `${resp.fName} ${resp.lName}`;
        this.caller= "Dr ";
        this.doctor= resp ;
      }
  }})
  }
  isAuthenticated(): boolean {
    return this.accontService.isAuthenticated();
  }
  logout(): void {
    this.accontService.logout();
    this.router.navigate(['/login']);
  }
  goToProfile(){
    console.log("entered")
    if(this.accontService.isCustomer())
    this.router.navigateByUrl(`/profile/${this.userId}`,{state:{customer:this.customer ,role:"customer"}})
    if(this.accontService.isDoctor())
    this.router.navigateByUrl(`/profile/${this.userId}`,{state:{doctor:this.doctor, role:"doctor"}})
    
  }
}
