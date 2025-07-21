import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable ,inject} from '@angular/core';
import { IDoctor } from './idoctor';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {
  httpClient = inject(HttpClient)
  getAll(name:any,maxPrice:any,speciality:any):Observable<IDoctor|IDoctor[]|string>{
    let params = new HttpParams();
    if(name)
    params=params.set("name", name);
    if(maxPrice)
    params=params.set("maxPrice",maxPrice)
    if(speciality)
    params=params.set("specialty",speciality);
    return this.httpClient.get<IDoctor | IDoctor[] | string>("https://localhost:7102/api/Doctors",{params})
  }
  getById(id:string):Observable<IDoctor|string>{
    return this.httpClient.get<IDoctor| string>(`https://localhost:7102/api/Doctors/${id}`)
  }
  editById(id:string, editedDoctor:IDoctor):Observable<string>{
    return this.httpClient.put<string>(`https://localhost:7102/api/Doctors/${id}`,editedDoctor)
  }
}
