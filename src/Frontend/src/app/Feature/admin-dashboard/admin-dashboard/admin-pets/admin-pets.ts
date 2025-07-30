import { Component, OnInit, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Router, RouterModule } from '@angular/router';
import { Doctor } from '../models/doctor';
import { Pet } from '../models/pet';
import { CustomerPofileDetails } from '../../../../models/customer-pofile-details';
import { AdminService } from '../admin-service';

@Component({
  selector: 'app-admin-pets',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-pets.html',
  styleUrl: './admin-pets.css',
})
export class AdminPets {
  doctors: Doctor[] = [];
  pets: Pet[] = [];
  loading = true;
  currentImageUrl: string = '';
  profileData: CustomerPofileDetails | null = null; // Instead of a large ViewModel type
  loadingProfile = true;
  selectedRejectionId: string | number | null = null;
  rejectionTarget: 'doctor' | 'pet' | null = null;
  rejectionMessage: string = '';
  chartReady = false;

  constructor(
    private adminService: AdminService,
    private modalService: NgbModal,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.loadData();
    this.adminService.getAdminProfile().subscribe((data) => {
      console.log('Profile Data:', data);
      this.profileData = data;
      this.loadingProfile = false;
    });
  }

  loadData(): void {
    this.loading = true;
    this.adminService.getPendingData().subscribe({
      next: (data) => {
        this.doctors = data.pendingDoctors;
        this.pets = data.pendingPets;
        console.log(this.pets);
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load data:', err);
        alert('Failed to load data.');
        this.loading = false;
      },
    });
  }

  approvePet(id: number): void {
    this.adminService.approvePet(id).subscribe({
      next: () => (this.pets = this.pets.filter((p) => p.id !== id)),
      error: (err) => {
        console.error('Failed to approve pet:', err);
        alert('Failed to approve pet.');
      },
    });
  }

  openRejectionModal(
    content: TemplateRef<any>,
    id: string | number,
    type: 'doctor' | 'pet'
  ) {
    this.selectedRejectionId = id;
    this.rejectionTarget = type;
    this.rejectionMessage = '';
    this.modalService.open(content, { size: 'md' });
  }

  openImageModal(content: TemplateRef<any>, imageUrl: string) {
    this.currentImageUrl = imageUrl;
    this.modalService.open(content, { size: 'xl', centered: true });
  }

  confirmRejection(modalRef: any): void {
    if (!this.rejectionMessage.trim()) {
      alert('Please enter a rejection message.');
      return;
    }

    if (
      this.rejectionTarget === 'doctor' &&
      typeof this.selectedRejectionId === 'string'
    ) {
      this.adminService
        .rejectDoctor(this.selectedRejectionId, this.rejectionMessage)
        .subscribe({
          next: () => {
            this.doctors = this.doctors.filter(
              (d) => d.id !== this.selectedRejectionId
            );
            modalRef.close();
          },
          error: (err) => {
            console.error('Failed to reject doctor:', err);
            alert('Failed to reject doctor.');
          },
        });
    }

    if (
      this.rejectionTarget === 'pet' &&
      typeof this.selectedRejectionId === 'number'
    ) {
      this.adminService
        .rejectPet(this.selectedRejectionId, this.rejectionMessage)
        .subscribe({
          next: () => {
            this.pets = this.pets.filter(
              (p) => p.id !== this.selectedRejectionId
            );
            modalRef.close();
          },
          error: (err) => {
            console.error('Failed to reject pet:', err);
            alert('Failed to reject pet.');
          },
        });
    }
  }

  isImage(url: string): boolean {
    if (!url) return false;
    return ['.jpg', '.jpeg', '.png', '.gif', '.webp'].some((ext) =>
      url.toLowerCase().endsWith(ext)
    );
  }

  getSafeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
