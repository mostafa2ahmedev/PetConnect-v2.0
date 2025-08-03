export interface TimeSlotsWithStatusDTO {
  id: string;
  doctorId: string;
  startTime: string;    
  endTime: string;
  status: string;
  maxCapacity: number;
  bookedCount: number;
  isActive: boolean;
  isFull: boolean ;
}
