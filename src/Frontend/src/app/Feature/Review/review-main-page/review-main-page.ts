import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { DoctorsService } from '../../doctors/doctors-service';
import Swiper from 'swiper';
import 'swiper/css';
import 'swiper/css/navigation'; // If you're using navigation
import 'swiper/css/pagination'; // If you're using pagination

// You also need to import the modules you will use
import { Navigation, Pagination } from 'swiper/modules';
import { Router } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { DoctorReviewModel } from '../models/doctor-review-model';
import { ReviewsService } from '../reviews-service';
@Component({
  selector: 'app-review-main-page',
  imports: [DatePipe],
  templateUrl: './review-main-page.html',
  styleUrl: './review-main-page.css'
})
export class ReviewMainPage implements OnInit , AfterViewInit{
  ngAfterViewInit(): void {
    this.initSwiper();
  }
  ngOnInit(): void {
    if(!history.state.doctorId)
      this.router.navigate(['/doctors'])

    this.doctorId=history.state.doctorId;
    this.doctorService.getById(history.state.doctorId).subscribe({
      next:resp=>{
        if(typeof resp !='string'){
          this.doc=resp;
        this.doctorName=`${resp.fName} ${resp.lName}`;

        }
      }
    })

    this.reviewService.getDoctorReviewsById(this.doctorId).subscribe({
      next:resp=>{
        this.testimonials= resp.data
        console.log(resp.data);
      },
      error: err=>{
        console.log(err);
      }
    })
  }

  initSwiper() {
    // You should load the Swiper library from an external file or a module
    // and then call this function once the component is initialized
    new Swiper('.testimonials-slider', {
      modules: [Navigation, Pagination],
      loop: true,
      speed: 800,
      autoplay: {
        delay: 5000,
      },
      slidesPerView: 1,
      spaceBetween: 30,
      pagination: {
        el: '.swiper-pagination',
        type: 'bullets',
        clickable: true,
      },
      breakpoints: {
        768: {
          slidesPerView: 1,
        },
        1200: {
          slidesPerView: 1,
        },
      },
    });
  }
  bookAppointment(){
    this.router.navigate(['/doctors',this.doctorId])
  }
  goBack(){
    this.location.back();
  }
  makeReview(){
    this.router.navigate(['doctors','add-review'],{state:{doctorObj:this.doc}})
  }
  arrayOfRating(rating:number){
    return Array.from({length :rating},(_,i)=>i);
  }
  getFullImageUrl(relativePath: string): string {
    return `https://localhost:7102${relativePath}`;
  }
  doctorService = inject(DoctorsService)
  router=inject(Router)
  location = inject(Location)
doctorName:string="";
doctorId:string="" ;
doc:any;
  // testimonials:{id:string,content:string, stars:number[],halfStar:boolean,name:string,role:string,imgSrc:string}[]=[];
  testimonials:DoctorReviewModel[] = []; 
  reviewService= inject(ReviewsService);
}
