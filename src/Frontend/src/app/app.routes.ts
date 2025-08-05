import { Routes } from '@angular/router';
import { Contact } from './Feature/contact/contact';
import { Login } from './Feature/login/login';
import { Register } from './Feature/register/register';
import { Home } from './Feature/home/home';
import { AddPets } from './Feature/pets/add-pets/add-pets';
import { Categories } from './Feature/categories/All-categories/categories';
import { Doctors } from './Feature/doctors/doctors';
import { DoctorRegisterForm } from './Feature/doctor-register-form/doctor-register-form';
import { CustomerRegisterForm } from './Feature/customer-register-form/customer-register-form';
import { PetDetails } from './Feature/pets/pet-details/pet-details';
import { Pets } from './Feature/pets/All-Pets/pets';
import { UpdatePet } from './Feature/pets/update-pet/update-pet';
import { CategoryDetails } from './Feature/categories/category-details/category-details';
import { AddCategory } from './Feature/categories/add-category/add-category';
import { AllBreeds } from './Feature/breeds/all-breeds/all-breeds';
import { AddBreed } from './Feature/breeds/add-breed/add-breed';
import { BreedDetails } from './Feature/breeds/breed-details/breed-details';
import { DoctorProfile } from './Feature/doctor-profile/doctor-profile';
import { DoctorEditProfile } from './Feature/doctor-edit-profile/doctor-edit-profile';
import { NotFoundDoctor } from './Feature/not-found-doctor/not-found-doctor';
import { OrdersComponent } from './Feature/orders/orders';
import { ProductsComponent } from './Feature/Products/all-products/all-products';
import { ProductDetailsComponent } from './Feature/Products/product-details/product-details';
import { CartComponent } from './Feature/cart/cart';

import { DoctorAddTimeslot } from './Feature/doctor-add-timeslot/doctor-add-timeslot';
import { DoctorCustomerAppointment } from './Feature/doctor-customer-appointment/doctor-customer-appointment';
import { Profile } from './Feature/profile/profile';
import { Customer } from './Feature/profile/customer/customer';
import { Doctor } from './Feature/profile/doctor/doctor';
import { ShowAllDoctorTimeslots } from './Feature/Doctor/show-all-doctor-timeslots/show-all-doctor-timeslots';

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
import { DoctorLearnMore } from './Feature/doctor-learn-more/doctor-learn-more';
import { doctorGuardGuard } from './core/guards/doctor-guard-guard';
import { CustomerGuard } from './core/guards/customer-guard';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'home', component: Home },
  { path: 'doctors', component: Doctors },
  {
    path: 'doctors/appointment',
    component: DoctorCustomerAppointment,
    canActivate: [authGuard],
  },
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
    canActivate: [doctorGuardGuard],
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
  { path: 'doc-profile', component: Doctor, canActivate: [doctorGuardGuard] },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'notfound/doctor', component: NotFoundDoctor },
  { path: 'orders', component: OrdersComponent },
  { path: 'all-products', component: ProductsComponent },
  { path: 'products/:id', component: ProductDetailsComponent },
  { path: 'cart', component: CartComponent },

  { path: 'register', component: Register, children: [] },
  {
    path: 'profile/:id',
    component: Profile,
    children: [
      // {path:'doctor',component: Doctor},
      // {path:'customer', component:Customer}
    ],
  },
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
];
