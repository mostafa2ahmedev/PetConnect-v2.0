import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RegisterationDataService {
  profileImage: File | null = null;
  idCardImage: File | null = null;
  certificateFile: File | null = null;

  clearData(): void {
    this.profileImage = null;
    this.idCardImage = null;
    this.certificateFile = null;
  }
}
