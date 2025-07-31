import { Component, OnInit ,inject, signal} from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DoctorsService } from '../doctors/doctors-service';
import { IDoctor } from '../doctors/idoctor';
import { AlertService } from '../../core/services/alert-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-doctor-learn-more',
  imports: [CommonModule,RouterLink],
  templateUrl: './doctor-learn-more.html',
  styleUrl: './doctor-learn-more.css'
})
export class DoctorLearnMore implements OnInit{
  activateRoute = inject(ActivatedRoute)
  id!:string ;
  server:string = 'https://localhost:7102'
  doctorsService = inject(DoctorsService)
  profileLoading= signal(true);
  doctor: IDoctor={}as IDoctor;
  alertSerive = inject(AlertService);
  router = inject(Router);
  ngOnInit(): void {
  this.activateRoute.params.subscribe({
    next: (e) => {
      this.id = e['id'];
      this.doctorsService.getById(this.id).subscribe({

        next:(response) => {
          console.log("hi");
          // this.errorFound=false;
          if(typeof response != 'string'){
            this.doctor = response
            this.profileLoading.set(false);
          }
        },
      
        error: err=>{
                // this.errorFound=true;
                this.alertSerive.error(err.error?.title?? "not found")
                this.router.navigateByUrl("/notfound/doctor",)
        }}
      );}  }
    )
}
  getFullImageUrl(relativePath: string): string {
    return `${this.server}/${relativePath}`;
  }

  getBackToDoctorAfterLogin(){
    return this.router.navigate(['/doctors', this.id],{queryParams: { returnUrl: this.router.url }});
  }

}
