export interface DoctorProfileAppointmentView {
    id: string;
  doctorName: string;
  customerName: string;
  slotStartTime: string;
  slotEndTime: string;
  petName: number;
  status: string;
  createdAt: string;
  notes: string | null;
  maxCapacity:number;
  bookedCount:number;
  petImg?:string|null;
  customerImg?:string|null;
  customerPhone?:string|null;
}
