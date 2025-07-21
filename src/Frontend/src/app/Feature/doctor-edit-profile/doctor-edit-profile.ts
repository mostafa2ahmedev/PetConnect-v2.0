import { Component , inject, OnInit} from '@angular/core';
import { DoctorsService } from '../doctors/doctors-service';
import { ActivatedRoute, Router } from '@angular/router';
import { IDoctor } from '../doctors/idoctor';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-doctor-edit-profile',
  imports: [FormsModule],
  templateUrl: './doctor-edit-profile.html',
  styleUrl: './doctor-edit-profile.css'
})
export class DoctorEditProfile implements OnInit{

  activeRoute = inject(ActivatedRoute);
  router = inject(Router)
  doctorsService = inject(DoctorsService);
  server = "https://localhost:7102";
  id:string="";
  doctor:IDoctor|string="";
    ngOnInit(): void {
        this.activeRoute.params.subscribe(e=>this.id=e['id']);
        this.doctorsService.getById(this.id).subscribe(e=>this.doctor= e)
  }
  onSubmit() {
    if (this.doctor && typeof this.doctor !== 'string') {
      this.doctorsService.editById(this.id, this.doctor).subscribe(response => {
        console.log('Doctor profile updated successfully:', response);
        this.router.navigateByUrl('/doctors');
      }, error => {
        console.error('Error updating doctor profile:', error);
      });
    } else {
      console.error('Doctor data is not valid');
    }
  }
}
