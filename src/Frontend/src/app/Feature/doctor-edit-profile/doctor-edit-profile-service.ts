import { Injectable } from '@angular/core';
import { IDoctor } from '../doctors/idoctor';
import { IDoctorEdit } from './idoctor-edit';

@Injectable({
  providedIn: 'root'
})
export class DoctorEditProfileService {
  
    doctorCastToEdit(doctor:IDoctor|string): IDoctorEdit {
      if (typeof doctor === 'string') {
        throw new Error('Doctor data is not valid');
      }
      return {
        id: doctor.id,
        FName: doctor.fName,
        LName: doctor.lName,
        ImgUrl: doctor.imgUrl,
        ImageFile: doctor.imageFile,
        PetSpecialty: doctor.petSpecialty,
        Gender: doctor.gender,
        PricePerHour: doctor.pricePerHour,
        CertificateUrl: doctor.certificateUrl,
        CertificateFile: doctor.certificateFile,
        Street: doctor.street,
        City: doctor.city,
        IsApproved: doctor.isApproved,
      };
    } 
}
