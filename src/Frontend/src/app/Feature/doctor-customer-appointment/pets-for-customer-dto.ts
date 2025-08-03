export interface PetsForCustomerDto {
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
  customerName: string;
}
