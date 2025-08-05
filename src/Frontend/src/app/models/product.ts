import { ProductType } from './product-type';

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  productTypeId: number;
  productType?: ProductType;
  // image?: File;
  quantity:number;
  imgUrl:File;

}
