export interface SellerOrderProduct {
  productId: number;
  quantity: number;
  unitPrice: number;
  orderProductStatus: number;
  orderId: number;
  orderDate: string; // ISO date string
  productName: string;
  productImgUrl: string;
  customerId: string;
  customerName: string;
}

