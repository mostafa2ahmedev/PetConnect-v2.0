import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PetService } from '../pet-service';
import { CommonModule } from '@angular/common';
import { PetDetailsModel } from '../../../models/pet-details';
import { EnumService } from '../../../core/services/enum-service';
import { AlertService } from '../../../core/services/alert-service';

@Component({
  selector: 'app-pet-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './pet-details.html',
  styleUrl: './pet-details.css',
})
export class PetDetails implements OnInit {
  pet!: PetDetailsModel;
  loading = true;
  error = '';
  ownershipMap: { [key: number]: string } = {};
  UrlId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private petService: PetService,
    private enumservice: EnumService,
    private router: Router,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
    this.UrlId = Number(this.route.snapshot.paramMap.get('id'));
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!isNaN(id)) {
      this.petService.getPetById(id).subscribe({
        next: (response) => {
          this.pet = response;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load pet details.';
          this.loading = false;
          console.error(err);
        },
      });
    } else {
      this.error = 'Invalid pet ID.';
      this.loading = false;
    }

    this.enumservice.loadAllEnums().subscribe();
  }

  deletePetById(id: number): void {
    // if (!confirm('Are you sure you want to delete this pet?')) return;

    this.petService
      .deletePet(Number(this.route.snapshot.paramMap.get('id')))
      .subscribe({
        next: () => {
          this.alert.success('Pet deleted successfully!');
          this.router.navigate(['/pets']);
        },
        error: (err) => {
          this.alert.error('Failed to delete pet.');

          console.error(err);
        },
      });
  }
  getOwnershipLabel(code: number): string {
    return this.enumservice.getOwnershipLabel(code);
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
