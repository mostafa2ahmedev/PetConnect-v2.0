import { Component, inject, OnInit } from '@angular/core';
import { ShowAllDoctorTimeslotsService } from '../show-all-doctor-timeslots-service';
import { DataTimeSlotsDto } from '../../doctor-profile/data-time-slot-dto';
import { CommonModule, DatePipe, Location } from '@angular/common';
import { AlertService } from '../../../core/services/alert-service';
import { ShowAllDoctorTimeslotsDto } from '../show-all-doctor-timeslots-dto';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-show-all-doctor-timeslots',
  imports: [DatePipe, CommonModule,FormsModule,ReactiveFormsModule],
  templateUrl: './show-all-doctor-timeslots.html',
  styleUrl: './show-all-doctor-timeslots.css'
})
export class ShowAllDoctorTimeslots implements OnInit {
  showAllDocsTsService = inject(ShowAllDoctorTimeslotsService);
  allTimeSlots: DataTimeSlotsDto[]=[]
  alertService = inject(AlertService);
  locationService = inject(Location)  ;


   // Data that will be displayed after filtering and pagination
  paginatedTimeSlots: DataTimeSlotsDto[] = [];

  // --- Filtering Variables ---
  filterStatus: string = 'all'; // 'all', 'active', 'inactive'
  filterSearchId: string = '';
  filterStartDate: string = ''; // Input type="date" gives YYYY-MM-DD string
  filterEndDate: string = '';

  // --- Pagination Variables ---
  currentPage: number = 1;
  itemsPerPage: number = 6; // Adjust as needed
  totalPages: number = 1;
  totalItems: number = 0;


  ngOnInit(): void {
    this.showAllDocsTsService.ShowAllTimeSlots().subscribe({
      next:resp=>{
        // console.log(resp.data)
        this.allTimeSlots = resp.data;
        this.applyFiltersAndPagination();

      } 
    })
      // Simulate fetching data
  }

  // Main method to apply all filters and then pagination
  applyFiltersAndPagination(): void {
    let filteredSlots = [...this.allTimeSlots]; // Start with a copy of all data

    // 1. Apply Status Filter
    if (this.filterStatus === 'active') {
      filteredSlots = filteredSlots.filter(slot => slot.isActive);
    } else if (this.filterStatus === 'inactive') {
      filteredSlots = filteredSlots.filter(slot => !slot.isActive);
    }


    // 2. Apply Search by ID Filter
    if (this.filterSearchId) {
      const searchTerm = this.filterSearchId.toLowerCase();
      filteredSlots = filteredSlots.filter(slot =>
        slot.id.toLowerCase().includes(searchTerm)
      );
    }

    // 3. Apply Date Range Filter
    if (this.filterStartDate) {
      const startOfDay = new Date(this.filterStartDate);
      startOfDay.setHours(0, 0, 0, 0); // Set to start of the day
      filteredSlots = filteredSlots.filter(slot => new Date(slot.startTime) >= startOfDay);
    }
    if (this.filterEndDate) {
      const endOfDay = new Date(this.filterEndDate);
      endOfDay.setHours(23, 59, 59, 999); // Set to end of the day
      filteredSlots = filteredSlots.filter(slot => new Date(slot.startTime) <= endOfDay);
    }

    // After filtering, update total items and pages
    this.totalItems = filteredSlots.length;
    this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage);

    // Ensure current page is valid after filtering (e.g., if filter makes current page empty)
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages > 0 ? this.totalPages : 1;
    }
    if (this.currentPage < 1) {
      this.currentPage = 1; // Ensure it's at least 1
    }

    // 4. Apply Pagination
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedTimeSlots = filteredSlots.slice(startIndex, endIndex);
  }

  // --- Pagination Methods ---
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.applyFiltersAndPagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.applyFiltersAndPagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.applyFiltersAndPagination();
    }
  }


  // Method to get page numbers for the pagination display
  getPages(): number[] {
    const pages: number[] = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pages.push(i);
    }
    return pages;
  }

changeStatusToActive(slot:ShowAllDoctorTimeslotsDto){

  this.showAllDocsTsService.changeStatusToActive(slot).subscribe({
    next:resp=>{
      slot.isActive = !slot.isActive; // Update the slot's isActive property
      console.log(resp);
      console.log(slot);
      this.alertService.success(resp.data);
    this.applyFiltersAndPagination();

    },
    error:err=>{
      console.error(err.data);
    }
  })

}

goBack(){
  this.locationService.back();
}
}
