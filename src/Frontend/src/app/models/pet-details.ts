export interface PetDetailsModel {
  id: number;
  name: string;
  status: number;
  age: number;
  isApproved: boolean;
  ownership: number;
  imgUrl: string;
  breadName: string;
  categoryName: string;
  customerId: string;
  customerName?: string;
  customerStreet: string;
  customerCity: string;
  customerCountry: string;
  notes: string;
}

export interface PetDetailsModel2 {
  id: number;
  name: string;
  status: number;
  age: number;
  isApproved: boolean;
  ownership: number;
  imgUrl: string;
  breadName: string;
  categoryName: string;
  customerId: string;
  customerName?: string;
}
//customerStreet, customerCity, customerCountry, notes