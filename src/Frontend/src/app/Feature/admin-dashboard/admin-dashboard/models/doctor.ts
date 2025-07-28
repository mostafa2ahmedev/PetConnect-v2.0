export interface Doctor {
  id: string;
  fName: string;
  lName: string;
  imgUrl: string;
  imageFile?: string;
  phoneNumber: string;
  email: string;
  petSpecialty: string;
  gender: string;
  pricePerHour: number;
  certificateUrl: string;
  certificateFile?: string;
  street: string;
  city: string;
  isApproved: boolean;
  isDeleted: boolean;
}
