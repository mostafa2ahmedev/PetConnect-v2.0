import { Gender } from "./gender";

export class CustomerRegister {
  fName: string | null = null ;
  lName:  string | null = null ;
  email:  string | null = null ;
  phoneNumber:  string | null = null ;
  password: string | null = null ;
  confirmationPassword: string | null = null ;
  gender: Gender | null = null; 
  country: string | null = null ;
  city: string | null = null ;
  street: string | null = null ;
}
