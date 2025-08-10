export interface TimeSlotsWithCustomerIdStatusBookingDTO {
      id: string;
  doctorId: string;
  startTime: string;    
  endTime: string;
  status: string;
  maxCapacity: number;
  bookedCount: number;
  isActive: boolean;
  isFull: boolean ;
  customerId:string;
}
