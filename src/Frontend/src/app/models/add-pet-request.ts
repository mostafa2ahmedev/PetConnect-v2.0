export interface AddPetRequest {
  Name: string;
  Status: number;
  Ownership: number;
  BreedId: number;
  ImgURL: File;
  Age: number;
  Notes: string;
}
