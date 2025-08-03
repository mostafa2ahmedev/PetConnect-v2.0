export interface AdoptionDecision {
  adoptionStatus: number; // 0: Pending, 1: Approved, 2: Canceled
  petId: number;
  reqCustomerId: string;
  adoptionDate: string;
}
