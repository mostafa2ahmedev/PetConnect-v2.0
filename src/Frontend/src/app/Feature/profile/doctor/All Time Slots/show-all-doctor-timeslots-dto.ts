export interface ShowAllDoctorTimeslotsDto {
      id:string;
  startTime: string;
  endTime: string;
  isActive: boolean; // flipping it
  maxCapacity: number;
  bookedCount: number;
}
