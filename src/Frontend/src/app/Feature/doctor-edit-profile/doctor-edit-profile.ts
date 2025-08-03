import { Component , inject, OnInit, signal} from '@angular/core';
import { DoctorsService } from '../doctors/doctors-service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { IDoctor } from '../doctors/idoctor';
import { FormsModule } from '@angular/forms';
import { IDoctorEdit } from './idoctor-edit';
import { CommonModule , Location } from '@angular/common';
import { DoctorEditProfileService } from './doctor-edit-profile-service';

@Component({
  selector: 'app-doctor-edit-profile',
  imports: [FormsModule,RouterLink,CommonModule],
  templateUrl: './doctor-edit-profile.html',
  styleUrl: './doctor-edit-profile.css'
})
export class DoctorEditProfile implements OnInit{
  activeRoute = inject(ActivatedRoute);
  router = inject(Router)
  doctorsService = inject(DoctorsService);
  doctorEditService = inject(DoctorEditProfileService);
  locationService = inject(Location)
  server = "https://localhost:7102";
  id:string="";
  doctor:IDoctor|string="";
  editedDoctor:IDoctorEdit =null as any;
  imageError: string = '';
  certificateError: string = '';
  profileLoading = signal(true)

onFileChange(event: Event, type: 'image' | 'certificate') {
  const input = event.target as HTMLInputElement;

  if (input.files && input.files.length > 0) {
    const file = input.files[0];
    const allowedTypes = ['image/png', 'image/jpeg', 'image/jpg', 'application/pdf'];
    const maxSize = 2 * 1024 * 1024; // 2MB

    if (!allowedTypes.includes(file.type)) {
      const msg = 'Invalid file type. Please upload PNG, JPG, JPEG, or PDF.';
      type === 'image' ? this.imageError = msg : this.certificateError = msg;
      return;
    }
    if (file.size > maxSize) {
      const msg = 'File size must be under 2MB.';
      type === 'image' ? this.imageError = msg : this.certificateError = msg;
      return;
    }
    // Clear error and assign the file
    type === 'image'
      ? (this.imageError = '', this.editedDoctor.ImageFile = file)
      : (this.certificateError = '', this.editedDoctor.CertificateFile = file);
  }
}

    ngOnInit(): void {
        this.activeRoute.params.subscribe(e=>{
          this.id=e['id'];
          this.doctorsService.getById(this.id).subscribe(e=>{
          this.doctor= e
          if (typeof this.doctor !== 'string' ){
            this.doctor.imageFile = "";
            this.doctor.certificateFile = "";
          }
          this.editedDoctor = this.doctorCastToEdit();
          this.profileLoading.set(false);
        })
      })
  }
   onSubmit() {
    if (this.doctor && typeof this.doctor !== 'string') {
       this.doctorsService.editByIdWithFile(this.id, this.editedDoctor).subscribe({next:response => {
        this.router.navigateByUrl('/doctors');
      }, error: error => {
        console.error('Error updating doctor profile:', error);
      }});
    } else {
      console.error('Doctor data is not valid');
    }
  }

  doctorCastToEdit(): IDoctorEdit {
    return this.doctorEditService.doctorCastToEdit(this.doctor);
  }
  goBack(){
    this.locationService.back();
  }
}
