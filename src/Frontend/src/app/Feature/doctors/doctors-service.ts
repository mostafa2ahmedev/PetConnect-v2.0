import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable ,inject} from '@angular/core';
import { IDoctor } from './idoctor';
import { Observable } from 'rxjs';
import { IDoctorEdit } from '../doctor-edit-profile/idoctor-edit';

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {
  httpClient = inject(HttpClient)
  getAll(name:any,maxPrice:any,speciality:any):Observable<IDoctor[]|string>{
    let params = new HttpParams();
    if(name)
      params=params.set("name", name);
    if(maxPrice)
      params=params.set("maxPrice",maxPrice)
    if(speciality)
      params=params.set("specialty",speciality);
    return this.httpClient.get< IDoctor[]| string>("https://localhost:7102/api/Doctors",{params})
  }
  getById(id:string):Observable<IDoctor|string>{
    return this.httpClient.get<IDoctor|string>(`https://localhost:7102/api/Doctors/${id}`)
  }
  editByIdWithFile
    (id: string, doctor: IDoctorEdit): Observable<any> {
  const formData = new FormData();

  // Required fields
  formData.append("Id", doctor.id);
  formData.append("FName", doctor.FName);
  formData.append("LName", doctor.LName);
  formData.append("Gender", doctor.Gender);
  formData.append("PetSpecialty", doctor.PetSpecialty);
  formData.append("PricePerHour", doctor.PricePerHour.toString());
  formData.append("Street", doctor.Street);
  formData.append("City", doctor.City);

  // Optional URLs
  formData.append("CertificateUrl", doctor.CertificateUrl || "");
  formData.append("ImgUrl", doctor.ImgUrl || "");

  // Optional files
  if (doctor.ImageFile instanceof File) {
    formData.append("ImageFile", doctor.ImageFile, doctor.ImageFile.name);
  }

  if (doctor.CertificateFile instanceof File) {
    formData.append("CertificateFile", doctor.CertificateFile, doctor.CertificateFile.name);
  }

  // Make the HTTP request
  return this.httpClient.put(`https://localhost:7102/api/Doctors/${id}`, formData);
}
  }

