export interface AddPetRequest {
  name: string;
  status: number;
  isApproved: boolean;
  ownership: number;
  breedId: number;
  imageFile: File;
}
