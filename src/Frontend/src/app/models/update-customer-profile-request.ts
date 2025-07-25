export interface UpdateCustomerProfileRequest {
  fName: string;
  lName: string;
  gender: number;
  street: string;
  city: string;
  country: string;
  imageFile?: File; // Optional image
}
