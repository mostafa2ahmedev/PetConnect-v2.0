export interface CusotmerPet {
  id: number;
  name: string;
  age?: number; // optional because it's nullable in C#
  status: number; // enum must be defined in TS too
  imgUrl: string;
  categoryName: string;
}
