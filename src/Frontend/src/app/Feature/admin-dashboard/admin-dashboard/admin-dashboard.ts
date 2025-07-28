import { Component, OnInit, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Doctor } from './models/doctor';
import { Pet } from './models/pet';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from './admin-service';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexNonAxisChartSeries,
  ApexChart,
  ApexResponsive,
  ApexLegend,
} from 'ng-apexcharts';
import { CustomerPofileDetails } from '../../../models/customer-pofile-details';
import { Router, RouterModule, RouterOutlet } from '@angular/router';

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: string[];
  legend: ApexLegend;
  colors: string[];
  plotOptions: ApexPlotOptions;
  dataLabels: ApexDataLabels;
};
@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgApexchartsModule,
    RouterOutlet,
    RouterModule,
  ],
})
export class AdminDashboardComponent implements OnInit {
  doctors: Doctor[] = [];
  pets: Pet[] = [];
  loading = true;
  currentImageUrl: string = '';
  profileData: CustomerPofileDetails | null = null; // Instead of a large ViewModel type
  loadingProfile = true;
  selectedRejectionId: string | number | null = null;
  rejectionTarget: 'doctor' | 'pet' | null = null;
  statistics: any;
  rejectionMessage: string = '';
  chartReady = false;
  sidebarVisible = false;

  PetChartOptions: ChartOptions = {
    series: [], // For adoption, rescue, owned

    chart: {
      type: 'pie',
    },
    labels: ['For Adoption', 'Rescue', 'Owned'],
    colors: ['#5D57F4', '#212529', '#949494ff'], // ✅ Custom colors here
    dataLabels: {
      enabled: true,
      style: {
        colors: ['#eeeeee'], // ✅ Text color for percentages on each slice
        fontSize: '10px',
        fontWeight: 'bold',
      },
      dropShadow: {
        enabled: false,
      },
    },
    legend: {
      position: 'bottom',
    },
    plotOptions: {
      pie: {
        donut: {
          size: '70%', // smaller = thinner stroke; try '50%', '40%', etc.
          labels: {
            show: false,
            name: {
              show: true,
              fontSize: '12px',
              fontWeight: 'bold',
              color: '#343a40', // Label name color
            },
            value: {
              show: true,
              fontSize: '16px',
              color: '#5D57F4', // ✅ Percentage number color
            },
            total: {
              show: true,
              label: 'Total',
              color: '#212529', // Total label color
              fontSize: '12px',
            },
          },
        },
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 320,
          },
          legend: {
            position: 'bottom',
          },
        },
      },
    ],
  };
  DocChartOptions: ChartOptions = {
    series: [], // For adoption, rescue, owned

    chart: {
      type: 'pie',
    },
    labels: ['Approved', 'Not Approved'],
    colors: ['#5D57F4', '#212529'], // ✅ Custom colors here
    dataLabels: {
      enabled: true,
      style: {
        colors: ['#ffffff'], // ✅ Text color for percentages on each slice
        fontSize: '16px',
        fontWeight: 'bold',
      },
      dropShadow: {
        enabled: false,
      },
    },
    legend: {
      position: 'bottom',
    },
    plotOptions: {
      pie: {
        donut: {
          size: '70%', // smaller = thinner stroke; try '50%', '40%', etc.
          labels: {
            show: false,
            name: {
              show: true,
              fontSize: '12px',
              fontWeight: 'bold',
              color: '#343a40', // Label name color
            },
            value: {
              show: true,
              fontSize: '16px',
              color: '#5D57F4', // ✅ Percentage number color
            },
            total: {
              show: true,
              label: 'Total',
              color: '#212529', // Total label color
              fontSize: '12px',
            },
          },
        },
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 320,
          },
          legend: {
            position: 'bottom',
          },
        },
      },
    ],
  };
  UserChartOptions: ChartOptions = {
    series: [], // For adoption, rescue, owned

    chart: {
      type: 'pie',
    },
    labels: ['Dcotors', 'Cusotmers'],
    colors: ['#5D57F4', '#212529'], // ✅ Custom colors here
    dataLabels: {
      enabled: true,
      style: {
        colors: ['#ffffff'], // ✅ Text color for percentages on each slice
        fontSize: '12px',
        fontWeight: 'bold',
      },
      dropShadow: {
        enabled: false,
      },
    },
    legend: {
      position: 'bottom',
    },
    plotOptions: {
      pie: {
        donut: {
          size: '70%', // smaller = thinner stroke; try '50%', '40%', etc.
          labels: {
            show: false,
            name: {
              show: true,
              fontSize: '12px',
              fontWeight: 'bold',
              color: '#343a40', // Label name color
            },
            value: {
              show: true,
              fontSize: '16px',
              color: '#5D57F4', // ✅ Percentage number color
            },
            total: {
              show: true,
              label: 'Total',
              color: '#212529', // Total label color
              fontSize: '12px',
            },
          },
        },
      },
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 320,
          },
          legend: {
            position: 'bottom',
          },
        },
      },
    ],
  };
  constructor(
    private adminService: AdminService,
    private modalService: NgbModal,
    private sanitizer: DomSanitizer,
    private router: Router
  ) {
    this.router.events.subscribe(() => {
      this.sidebarVisible = false;
    });
  }

  ngOnInit(): void {
    this.loadData();
    this.loadStatistics();
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

  loadStatistics(): void {
    this.adminService.getStatistics().subscribe({
      next: (stats) => {
        console.log(stats);
        this.statistics = stats;
        this.chartReady = true;

        this.PetChartOptions.series = [
          stats.totalPets,
          stats.petsForAdoption,
          stats.petsForRescue,
        ];
        this.DocChartOptions.series = [
          stats.totalDoctors - this.doctors.length,
          this.doctors.length,
        ];
        this.UserChartOptions.series = [
          stats.totalCustomers,
          stats.totalDoctors,
        ];
      },
      error: (err) => {
        console.error('Failed to load statistics:', err);
      },
    });
  }

  approveDoctor(id: string): void {
    this.adminService.approveDoctor(id).subscribe({
      next: () => (this.doctors = this.doctors.filter((d) => d.id !== id)),
      error: (err) => {
        console.error('Failed to approve doctor:', err);
        alert('Failed to approve doctor.');
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

  getDoctorFullName(doctor: Doctor): string {
    return `${doctor.fName} ${doctor.lName}`;
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

  toggleSidebar() {
    this.sidebarVisible = !this.sidebarVisible;
  }
}
