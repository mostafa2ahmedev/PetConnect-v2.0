import { inject, Injectable} from '@angular/core';
import { SellerModel } from './seller-model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../models/api-response';
import { SellerProductsModel } from './seller-products-model';

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
}
