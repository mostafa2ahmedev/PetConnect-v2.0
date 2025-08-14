import { Routes } from '@angular/router';
import { Contact } from './Feature/contact/contact';
import { Login } from './Feature/login/login';
import { Register } from './Feature/register/register';
import { Home } from './Feature/home/home';
import { AddPets } from './Feature/pets/add-pets/add-pets';
import { Categories } from './Feature/categories/All-categories/categories';
import { Doctors } from './Feature/doctors/doctors';
import { DoctorRegisterForm } from './Feature/Doctor/doctor-register-form/doctor-register-form';
import { CustomerRegisterForm } from './Feature/customer-register-form/customer-register-form';
import { PetDetails } from './Feature/pets/pet-details/pet-details';
import { Pets } from './Feature/pets/All-Pets/pets';
import { UpdatePet } from './Feature/pets/update-pet/update-pet';
import { CategoryDetails } from './Feature/categories/category-details/category-details';
import { AddCategory } from './Feature/categories/add-category/add-category';
import { AllBreeds } from './Feature/breeds/all-breeds/all-breeds';
import { AddBreed } from './Feature/breeds/add-breed/add-breed';
import { BreedDetails } from './Feature/breeds/breed-details/breed-details';
import { DoctorProfile } from './Feature/Doctor/doctor-profile/doctor-profile';
import { NotFoundDoctor } from './Feature/not-found-doctor/not-found-doctor';
import { OrdersComponent } from './Feature/orders/orders';
import { ProductsComponent } from './Feature/Products/all-products/all-products';
import { ProductDetailsComponent } from './Feature/Products/product-details/product-details';
import { CartComponent } from './Feature/cart/cart';
import { SellerDashboardComponent } from './Feature/seller-dashboard/seller-dashboard';

import { DoctorCustomerAppointment } from './Feature/Doctor/doctor-customer-appointment/doctor-customer-appointment';
import { Doctor } from './Feature/profile/doctor/doctor';
import { ShowAllDoctorTimeslots } from './Feature/profile/doctor/All Time Slots/show-all-doctor-timeslots/show-all-doctor-timeslots';

import { AdminDashboardComponent } from './Feature/admin-dashboard/admin-dashboard/admin-dashboard';
import { authGuard } from './core/guards/auth-guard';
import { CustomerProfile } from './Feature/customer-profile/Profile/customer-profile';
import { UpdateProfile } from './Feature/customer-profile/update-profile/update-profile';
import { AdminGuard } from './core/guards/admin-guard';
import { UnauthComponent } from './Feature/unauthorized/unauth-component/unauth-component';
import { AdminDoctors } from './Feature/admin-dashboard/admin-dashboard/admin-doctors/admin-doctors';
import { AdminPets } from './Feature/admin-dashboard/admin-dashboard/admin-pets/admin-pets';
import { AdminInsights } from './Feature/admin-dashboard/admin-dashboard/admin-insights/admin-insights';
import { ChatComponent } from './Feature/chat/chat/chat';
import { doctorGuardGuard } from './core/guards/doctor-guard-guard';
import { CustomerGuard } from './core/guards/customer-guard';
import { SellerRegisterForm } from './Feature/seller-register-form/seller-register-form';
import { Dashboard } from './Feature/Seller/dashboard/dashboard';
import { EditProduct } from './Feature/edit-product/edit-product';
import { DoctorAddTimeslot } from './Feature/profile/doctor/doctor-add-timeslot/doctor-add-timeslot';
import { DoctorLearnMore } from './Feature/Doctor/doctor-learn-more/doctor-learn-more';
import { DoctorEditProfile } from './Feature/Doctor/doctor-edit-profile/doctor-edit-profile';
import { DocAppointmentets } from './Feature/profile/doctor/doc-appointmentets/doc-appointmentets';
import { AllBlogs } from './Feature/Blog/all-blogs/all-blogs';
import { SinglePost } from './Feature/Blog/single-post/single-post';
import { AddBlog } from './Feature/Blog/add-blog/add-blog';
import { Blogs } from './Feature/profile/doctor/blogs/blogs';
import { UpdateBlog } from './Feature/Blog/update-blog/update-blog';
import { MainPage } from './Feature/Review/review-main-page/review-main-page';

import { NotFound } from './Feature/not-found/not-found';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'home', component: Home },
  { path: 'doctors', component: Doctors },
  {
    path: 'doctors/appointment',
    component: DoctorCustomerAppointment,
    canActivate: [authGuard, CustomerGuard],
  },
  {path:'doctors/review',component:ReviewMainPage}  ,
  {
    path: 'doctors/timeslots',
    component: ShowAllDoctorTimeslots,
    canActivate: [doctorGuardGuard],
  },
  {
    path: 'doctors/timeslot/:id',
    component: DoctorAddTimeslot,
    canActivate: [doctorGuardGuard],
  },
  { path: 'doctors/:id', component: DoctorProfile, canActivate: [authGuard] },
  { path: 'doctor-details/:id', component: DoctorLearnMore },
  {
    path: 'doctors/update/:id',
    component: DoctorEditProfile,
  },
  { path: 'pets', component: Pets },
  { path: 'pets/:mode', component: Pets },
  { path: 'pet-details/:id', component: PetDetails },
  { path: 'add-pet', component: AddPets, canActivate: [authGuard] },
  { path: 'pets/update/:id', component: UpdatePet, canActivate: [authGuard] },

  { path: 'contact', component: Contact },
  { path: 'login', component: Login },
  {
    path: 'profile/update',
    component: UpdateProfile,
    canActivate: [authGuard],
  },

  {
    path: 'profile',
    component: CustomerProfile,
    canActivate: [authGuard, CustomerGuard],
  },
  {
    path: 'doc-profile',
    component: Doctor,
    canActivate: [doctorGuardGuard],
    children: [
      {
        path: '',
        redirectTo: 'appointments', // default child route
        pathMatch: 'full',
      },
      {
        path: 'appointments',
        component: DocAppointmentets,
      },
      {
        path: 'add-time-slots/:id',
        component: DoctorAddTimeslot,
      },
      {
        path: 'timeslots/:id',
        component: ShowAllDoctorTimeslots,
      },
      {
        path: 'blog/add',
        component: AddBlog,
      },
      {
        path: 'blogs',
        component: Blogs,
      },
      {
        path: 'blog/edit/:id',
        component: UpdateBlog,
      },
    ],
  },
  { path: 'blog', component: AllBlogs, children: [] },
  { path: 'blog/post/:id', component: SinglePost, children: [] },
  {
    path: 'blog/add',
    component: AddBlog,
    children: [],
    canActivate: [doctorGuardGuard],
  },

  {
    path: 'blog/category/:categorySlug',
    component: AllBlogs,
  },
  {
    path: 'blog/topic/:topicSlug',
    component: AllBlogs,
  },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'register/seller', component: SellerRegisterForm },

  { path: 'notfound/doctor', component: NotFoundDoctor },
  { path: 'orders', component: OrdersComponent },
  { path: 'all-products', component: ProductsComponent },
  { path: 'products/edit', component: EditProduct },
  { path: 'products/:id', component: ProductDetailsComponent },
  { path: 'cart', component: CartComponent },
  // {path: 'seller', component: SellerDashboardComponent },
  { path: 'seller', component: Dashboard },

  { path: 'register', component: Register, children: [] },

  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'notfound/doctor', component: NotFoundDoctor },
  { path: 'notfound/doctor', component: NotFoundDoctor },
  {
    path: 'admin',
    component: AdminDashboardComponent,
    canActivate: [authGuard, AdminGuard],
    children: [
      { path: '', component: AdminInsights },

      { path: 'categories', component: Categories },
      {
        path: 'category/:id',
        component: CategoryDetails,
        canActivate: [authGuard, AdminGuard],
      },
      {
        path: 'add-category',
        component: AddCategory,
        canActivate: [authGuard, AdminGuard],
      },
      { path: 'breeds', component: AllBreeds },
      { path: 'doctors', component: AdminDoctors },
      { path: 'pets', component: AdminPets },
      {
        path: 'breed/:id',
        component: BreedDetails,
        canActivate: [authGuard, AdminGuard],
      },

      {
        path: 'add-breed',
        component: AddBreed,
        canActivate: [authGuard, AdminGuard],
      },

      { path: '', redirectTo: 'insights', pathMatch: 'full' }, // default child
    ],
  },
  { path: 'unauthorized', component: UnauthComponent },
  { path: 'register', component: Register, children: [] },

  { path: 'notfound/doctor', component: NotFoundDoctor },
  {
    path: 'chat',
    component: ChatComponent,
    canActivate: [authGuard, CustomerGuard],
  },
  {
    path: 'chat/:id',
    component: ChatComponent,
    canActivate: [authGuard, CustomerGuard],
  },
  { path: '**', component: NotFound },
];
