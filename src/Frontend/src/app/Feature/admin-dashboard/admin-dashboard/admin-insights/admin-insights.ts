import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexNonAxisChartSeries,
  ApexChart,
  ApexResponsive,
  ApexLegend,
} from 'ng-apexcharts';
import { RouterModule } from '@angular/router';

import { CustomerPofileDetails } from '../../../../models/customer-pofile-details';
import { AdminService } from '../admin-service';
import { Doctor } from '../models/doctor';
import { Pet } from '../models/pet';

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
  selector: 'app-admin-insights',
  imports: [CommonModule, FormsModule, NgApexchartsModule, RouterModule],
  templateUrl: './admin-insights.html',
  styleUrl: './admin-insights.css',
})
export class AdminInsights {
  loading = true;

  profileData: CustomerPofileDetails | null = null; // Instead of a large ViewModel type
  loadingProfile = true;
  selectedRejectionId: string | number | null = null;
  rejectionTarget: 'doctor' | 'pet' | null = null;
  statistics: AdminStatistics | null = null;
  rejectionMessage: string = '';
  chartReady = false;
  doctors: Doctor[] = [];
  pets: Pet[] = [];
  PetChartOptions: ChartOptions = {
    series: [], // For adoption, rescue, owned

    chart: {
      type: 'pie',
    },
    labels: ['For Adoption', 'Rescue', 'Owned'],
    colors: ['#5D57F4', '#9915a2ff', '#949494ff'], // ✅ Custom colors here
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
  PetApprovalChat: ChartOptions = {
    series: [], // For adoption, rescue, owned

    chart: {
      type: 'pie',
    },
    labels: ['Approved', 'Pending', 'Rejected'],
    colors: ['#5D57F4', '#9915a2ff', '#cd1313ff'], // ✅ Custom colors here
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
              fontSize: '12px',
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
    labels: ['Approved', 'Pending', 'Rejected'],
    colors: ['#5D57F4', '#9915a2ff', '#cd1313ff'], // ✅ Custom colors here
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
  UserChartOptions: ChartOptions = {
    series: [], // For adoption, rescue, owned

    chart: {
      type: 'pie',
    },
    labels: ['Dcotors', 'Cusotmers'],
    colors: ['#5D57F4', '#9915a2ff'], // ✅ Custom colors here
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
    private sanitizer: DomSanitizer
  ) {}

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
          stats.petsForAdoption,
          stats.petsForRescue,
          stats.approvedPets - stats.petsForAdoption - stats.petsForRescue,
        ];
        this.PetApprovalChat.series = [
          stats.approvedPets,
          stats.pendingPets,
          stats.rejectedPets,
        ];
        this.DocChartOptions.series = [
          stats.approvedDoctors,
          stats.pendingDoctors,
          stats.rejectedDoctors,
        ];
        this.UserChartOptions.series = [
          stats.totalUsers - stats.totalCustomers,
          stats.totalCustomers,
        ];
      },
      error: (err) => {
        console.error('Failed to load statistics:', err);
      },
    });
  }

  getSafeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
}
