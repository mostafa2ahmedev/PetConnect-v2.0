export interface AppointmentDto {
  id: string;
  doctorId: string;
  customerId: string;
  slotId: string;
  petId: string|null;
  status: string; // enum converted to string first letter is capital
  createdAt: string; // use Date if you plan to parse it
  isReviewable:boolean;
}
