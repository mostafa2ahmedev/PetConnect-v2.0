import { CommonModule } from '@angular/common';
import { Component, HostListener, ViewChild } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgbCarousel, NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home',
  imports: [NgbCarouselModule, CommonModule, RouterModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  doctors = [{ fName: 'h', id: 1, lName: 'e', petSpecialty: 'cat' }];
  @ViewChild(NgbCarousel) carousel!: NgbCarousel;

  testimonials = [
    {
      text: `Adopting Bella through this site was a seamless experience. The
             vet follow-up service was prompt and professional. I’m beyond grateful!`,
      rating: 5,
      img: 'assets/img/person/person-f-3.webp',
      name: 'Layla Ahmed',
      role: 'Pet Adopter',
    },
    {
      text: `I found Max here, and the adoption process was fast and caring.
             Their vet even helped me with his vaccination schedule. Highly recommend!`,
      rating: 5,
      img: 'assets/img/person/person-m-5.webp',
      name: 'Youssef El Sayed',
      role: 'Software Engineer',
    },
    {
      text: `My rescued kitten was treated for free by a certified vet I
             found here. This platform truly supports animal welfare in every way.`,
      rating: 5,
      img: 'assets/img/person/person-f-7.webp',
      name: 'Mariam Nabil',
      role: 'Animal Rescuer',
    },
    {
      text: `We adopted a dog and got access to quality pet food and
             emergency care through this website. It’s a full solution for pet lovers.`,
      rating: 5,
      img: 'assets/img/person/person-m-8.webp',
      name: 'Omar Khaled',
      role: 'New Pet Owner',
    },
    {
      text: `Adopting Bella through this site was a seamless experience. The
             vet follow-up service was prompt and professional. I’m beyond grateful!`,
      rating: 5,
      img: 'assets/img/person/person-f-3.webp',
      name: 'Layla Ahmed',
      role: 'Pet Adopter',
    },
    {
      text: `Their online vet consultation saved my cat during a critical
             illness. Thank you for providing such a valuable service!`,
      rating: 5,
      img: 'assets/img/person/person-f-10.webp',
      name: 'Salma Fathy',
      role: 'Pet Owner',
    },
  ];

  groupedTestimonials: any[][] = [];
  groupSize = 1;

  ngOnInit() {
    this.updateGroupSize(window.innerWidth);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateGroupSize(event.target.innerWidth);
  }

  updateGroupSize(width: number) {
    const newSize = width <= 1000 ? 1 : 2;
    if (newSize !== this.groupSize) {
      this.groupSize = newSize;
      this.groupedTestimonials = this.chunk(this.testimonials, this.groupSize);
    } else if (this.groupedTestimonials.length === 0) {
      this.groupedTestimonials = this.chunk(this.testimonials, this.groupSize);
    }
  }

  chunk(arr: any[], size: number): any[][] {
    const chunks = [];
    for (let i = 0; i < arr.length; i += size) {
      chunks.push(arr.slice(i, i + size));
    }
    return chunks;
  }

  getStars(n: number) {
    return Array(n).fill(0);
  }
  prev() {
    this.carousel.prev();
  }

  next() {
    this.carousel.next();
  }
}
