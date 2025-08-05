import { ProductType } from "../../models/product-type";

export interface SellerProductsModel {
  id: number;
      sellerId: string;
  productName: string;
  productType: ProductType; // You'll need to define or import this enum/type
  quantity: number;
  price: number;
  imgUrl?: string; // optional
  productDescription?: string; // optional
}
