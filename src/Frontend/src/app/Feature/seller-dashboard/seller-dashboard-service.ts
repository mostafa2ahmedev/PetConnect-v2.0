import { inject, Injectable} from '@angular/core';
import { SellerModel } from './seller-model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response';
import { SellerProductsModel } from './seller-products-model';
import { SellerOrderProduct } from '../Seller/dashboard/seller-order-product';
import { OrderSellerAction } from '../Seller/dashboard/order-seller-action';

@Injectable({
  providedIn: 'root'
})
export class SellerDashboardService {
  
  httpClient = inject(HttpClient)

  getSellerData(){
    return this.httpClient.get<ApiResponse<SellerModel>>("https://localhost:7102/api/Seller/Profile");
  }
  getSellerProducts(){
    return this.httpClient.get<ApiResponse<SellerProductsModel[]>>("https://localhost:7102/api/Seller/Products");
  }
  deleteProduct(productId: number): Observable<ApiResponse<string>> {
    return this.httpClient.delete<ApiResponse<string>>(`https://localhost:7102/api/Product/${productId}`);
  }
  getSellerOrders(): Observable<ApiResponse<SellerOrderProduct[]>> {
    return this.httpClient.get<ApiResponse<SellerOrderProduct[]>>("https://localhost:7102/api/OrderProduct");
  }
  getOrderStatusEnum(): Observable<ApiResponse<string[]>> {
    return this.httpClient.get<ApiResponse<string[]>>("https://localhost:7102/api/Enums/order-product-statuses");
  }
  changeOrderStatus(orderAction:OrderSellerAction): Observable<ApiResponse<string>> {
    return this.httpClient.post<ApiResponse<string>>(`https://localhost:7102/api/OrderProduct/action`, orderAction);
  }
}
