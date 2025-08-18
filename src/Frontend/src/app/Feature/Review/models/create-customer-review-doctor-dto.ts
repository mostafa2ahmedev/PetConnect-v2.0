export interface CreateCustomerReviewDoctorDTO {
    doctorId: string;
  content: string;
  rating: number;
  customerId: string;
  appointmentId: string; 
}
