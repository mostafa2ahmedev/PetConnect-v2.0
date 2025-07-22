import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Pet } from '../../models/pet';
import { ApiResponse } from '../../models/api-response';
import { PetDetailsModel } from '../../models/pet-details';
import { AddPetRequest } from '../../models/add-pet-request';

@Injectable({
  providedIn: 'root',
})
export class PetService {
  private apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getAllPets(): Observable<Pet[]> {
    return this.http
      .get<ApiResponse<Pet[]>>(`${this.apiUrl}/pet`)
      .pipe(map((res) => res.data));
  }

  getPetById(id: number): Observable<PetDetailsModel> {
    return this.http
      .get<ApiResponse<PetDetailsModel>>(`${this.apiUrl}/pet/${id}`)
      .pipe(map((res) => res.data));
  }

  getPetStatusEnum(): Observable<{ key: number; value: string }[]> {
    return this.http.get<{ key: number; value: string }[]>(
      `${this.apiUrl}/enums/pet-status-values`
    );
  }
  getPetOwnershipEnum(): Observable<{ key: number; value: string }[]> {
    return this.http.get<{ key: number; value: string }[]>(
      `${this.apiUrl}/enums/ownership-types`
    );
  }

  addPet(pet: AddPetRequest): Observable<any> {
    const formData = new FormData();
    formData.append('Name', pet.Name);
    formData.append('Status', pet.Status.toString());
    formData.append('Ownership', pet.Ownership.toString());

    formData.append('BreedId', pet.BreedId.toString());
    formData.append('ImgURL', pet.ImgURL); // adjust key if backend expects a different one
    formData.append('Age', pet.Age.toString());
    return this.http.post(`${this.apiUrl}/pet`, formData);
  }

  updatePet(id: number, pet: AddPetRequest): Observable<any> {
    const formData = new FormData();
    formData.append('Id', id.toString());
    formData.append('Name', pet.Name);
    formData.append('Status', pet.Status.toString());
    formData.append('IsApproved', 'false'); // backend sets it anyway
    formData.append('Ownership', '0'); // backend sets it anyway
    formData.append('BreedId', pet.BreedId.toString());
    formData.append('Age', pet.Age.toString());

    if (pet.ImgURL) {
      formData.append('ImgURL', pet.ImgURL);
    }

    return this.http.put(`${this.apiUrl}/pet`, formData);
  }
  deletePet(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/pet`, {
      params: { id: id },
    });
  }
}
