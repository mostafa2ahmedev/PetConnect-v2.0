import { Gender } from "./gender";
import { PetSpecialty } from "./pet-specialalty";

export class DoctorRegister {
  fName: string | null = null ;
  lName: string | null = null ;
  email: string | null = null ;
  phoneNumber: string | null = null ;
  password: string | null = null ;
  confirmationPassword: string | null = null ;
  gender: Gender | null = null; 
  image: File | null = null;
  certificate: File | null = null;
  pricePerHour: number | null = null;
  petSpecialty: PetSpecialty | null = null;
  country: string | null = null;
  city: string | null = null;
  street: string | null = null;
}
