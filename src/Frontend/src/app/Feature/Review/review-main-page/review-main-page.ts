import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { DoctorsService } from '../../doctors/doctors-service';
import Swiper from 'swiper';
import 'swiper/css';
import 'swiper/css/navigation'; // If you're using navigation
import 'swiper/css/pagination'; // If you're using pagination

// You also need to import the modules you will use
import { Navigation, Pagination } from 'swiper/modules';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
@Component({
  selector: 'app-review-main-page',
  imports: [],
  templateUrl: './review-main-page.html',
  styleUrl: './review-main-page.css'
})
export class ReviewMainPage implements OnInit , AfterViewInit{
  ngAfterViewInit(): void {
    this.initSwiper();
  }
  ngOnInit(): void {
    this.testimonials.push(  {
    id: "t1",
    content: "This product completely exceeded my expectations. The quality is top-notch and the customer service was outstanding.",
    stars: [1, 1, 1, 1],
    halfStar: true,
    name: "Sarah Mitchell",
    role: "Marketing Director",
    imgSrc: "https://randomuser.me/api/portraits/women/44.jpg"
  },
  {
    id: "t2",
    content: "Really good experience overall. There were a few minor issues, but they were resolved quickly.",
    stars: [1, 1, 1],
    halfStar: true,
    name: "James Carter",
    role: "Software Engineer",
    imgSrc: "https://randomuser.me/api/portraits/men/32.jpg"
  },{    id: "t1",
    content: "This product completely exceeded my expectations. The quality is top-notch and the customer service was outstanding.",
    stars: [1, 1, 1, 1],
    halfStar: true,
    name: "Sarah Mitchell",
    role: "Marketing Director",
    imgSrc: "https://randomuser.me/api/portraits/women/44.jpg"
  },
  {
    id: "t2",
    content: "Really good experience overall. There were a few minor issues, but they were resolved quickly.",
    stars: [1, 1, 1],
    halfStar: true,
    name: "James Carter",
    role: "Software Engineer",
    imgSrc: "https://randomuser.me/api/portraits/men/32.jpg"
  },{    id: "t1",
    content: "This product completely exceeded my expectations. The quality is top-notch and the customer service was outstanding.",
    stars: [1, 1, 1, 1],
    halfStar: true,
    name: "Sarah Mitchell",
    role: "Marketing Director",
    imgSrc: "https://randomuser.me/api/portraits/women/44.jpg"
  },
  {
    id: "t2",
    content: "Really good experience overall. There were a few minor issues, but they were resolved quickly.",
    stars: [1, 1, 1],
    halfStar: true,
    name: "James Carter",
    role: "Software Engineer",
    imgSrc: "https://randomuser.me/api/portraits/men/32.jpg"
  })

    this.doctorId=history.state.doctorId;
    this.doctorService.getById(history.state.doctorId).subscribe({
      next:resp=>{
        if(typeof resp !='string'){
          this.doc=resp;
        this.doctorName=`${resp.fName} ${resp.lName}`;

        }
        console.log(resp);
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
  doctorService = inject(DoctorsService)
  router=inject(Router)
  location = inject(Location)
doctorName:string="";
doctorId:string="" ;
doc:any;
  testimonials:{id:string,content:string, stars:number[],halfStar:boolean,name:string,role:string,imgSrc:string}[]=[];

}
