export interface Breed {
  id: number;
  name: string;
  categoryName: string; // used for filtering by category
  categoryId: number; // needed for add/update
}
