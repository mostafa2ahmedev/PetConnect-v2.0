import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ComparisonResult {
  facesMatch: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class FaceComparisonService {
  // عدّل البورت لو الـ API عندك على بورت مختلف
  private apiUrl = 'https://localhost:7102/api/FaceComparison';

  constructor(private http: HttpClient) { }

  compareFaces(image1: File, image2: File): Observable<ComparisonResult> {
    const formData = new FormData();
    // field names must match controller method parameters (image1, image2)
    formData.append('image1', image1, image1.name);
    formData.append('image2', image2, image2.name);

    return this.http.post<ComparisonResult>(this.apiUrl, formData);
  }
}
