export interface UpdateCustomerProfileRequest {
  fName: string;
  lName: string;
  ImageFile?: string;
  gender: number;
  street: string;
  city: string;
  country: string;
  isApproved: boolean;
  userName: string;
  email: string;
  phoneNumber: string;
}
