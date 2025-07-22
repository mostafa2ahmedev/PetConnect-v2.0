import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Pet } from '../../../models/pet';
import { PetService } from '../pet-service';
import { EnumService } from '../../../core/services/enum-service';

@Component({
  selector: 'app-pets',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './pets.html',
  styleUrl: './pets.css',
})
export class Pets implements OnInit {
  pets: Pet[] = [];
  statusMap: { [key: number]: string } = {};
  loading = true;
  error = '';
  constructor(
    private petService: PetService,
    private enumService: EnumService
  ) {}

  ngOnInit(): void {
    this.enumService.loadAllEnums().subscribe();

    this.loadPets();
  }

  loadPets(): void {
    this.petService.getAllPets().subscribe({
      next: (pets) => {
        this.pets = pets;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading pets:', err);
        this.error = 'Failed to load pets. Please try again later.';
        this.loading = false;
      },
    });
  }

  getStatusLabel(statusCode: number): string {
    return this.enumService.getStatusLabel(statusCode);
  }

  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
