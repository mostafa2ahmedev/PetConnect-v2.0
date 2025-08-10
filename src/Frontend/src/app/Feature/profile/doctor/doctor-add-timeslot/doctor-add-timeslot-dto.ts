export interface DoctorAddTimeslotDto {
  doctorId:string ;
  startTime: string;
  endTime: string;
  isActive: boolean;
  maxCapacity: number;
  bookedCount: number;
}
