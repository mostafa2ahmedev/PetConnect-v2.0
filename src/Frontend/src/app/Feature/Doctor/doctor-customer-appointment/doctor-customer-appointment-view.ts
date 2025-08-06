export interface DoctorCustomerAppointmentView {
  id: string;
  doctorId: string;
  customerId: string;
  slotId: string;
  petId: number;
  status: string;
  createdAt: string;
  notes: string | null;
}
