import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { forkJoin, map, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface EnumOption {
  key: number;
  value: string;
}

@Injectable({
  providedIn: 'root',
})
export class EnumService {
  private statusMap: { [key: number]: string } = {};
  private ownershipMap: { [key: number]: string } = {};
  private statusOptions: EnumOption[] = [];
  private ownershipOptions: EnumOption[] = [];

  private apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  loadAllEnums(): Observable<void> {
    return forkJoin([
      this.http.get<EnumOption[]>(`${this.apiUrl}/enums/pet-status-values`),
      this.http.get<EnumOption[]>(`${this.apiUrl}/enums/ownership-types`),
    ]).pipe(
      tap(([statuses, ownerships]) => {
        this.statusMap = this.buildMap(statuses);
        this.ownershipMap = this.buildMap(ownerships);
        this.statusOptions = statuses;
        this.ownershipOptions = ownerships;
      }),
      map(() => void 0)
    );
  }

  getStatusLabel(code: number): string {
    return this.statusMap[code] ?? 'Unknown';
  }

  getOwnershipLabel(code: number): string {
    return this.ownershipMap[code] ?? 'Unknown';
  }

  getStatusOptions(): EnumOption[] {
    return this.statusOptions;
  }

  getOwnershipOptions(): EnumOption[] {
    return this.ownershipOptions;
  }

  private buildMap(arr: EnumOption[]): { [key: number]: string } {
    return arr.reduce((map, item) => {
      map[item.key] = item.value;
      return map;
    }, {} as { [key: number]: string });
  }
}
