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
import { DoctorAddTimeslot } from './Feature/doctor-add-timeslot/doctor-add-timeslot';
import { DoctorCustomerAppointment } from './Feature/doctor-customer-appointment/doctor-customer-appointment';
import { Profile } from './Feature/profile/profile';
import { Customer } from './Feature/profile/customer/customer';
import { Doctor } from './Feature/profile/doctor/doctor';
import { ShowAllDoctorTimeslots } from './Feature/Doctor/show-all-doctor-timeslots/show-all-doctor-timeslots';
export const routes: Routes = [
  { path: '', component: Home },
  { path: 'doctors', component: Doctors },
  {path:'doctors/appointment',component:DoctorCustomerAppointment},
  {path:'doctors/timeslots', component:ShowAllDoctorTimeslots},
  {path:'doctors/timeslot/:id', component:DoctorAddTimeslot},
  { path: 'doctors/:id', component: DoctorProfile },
  { path: 'doctors/update/:id', component: DoctorEditProfile },
  { path: 'pets', component: Pets },
  { path: 'pet-details/:id', component: PetDetails },
  { path: 'add-pet', component: AddPets },
  { path: 'pets/update/:id', component: UpdatePet },
  { path: 'categories', component: Categories },
  { path: 'category/:id', component: CategoryDetails },
  { path: 'add-category', component: AddCategory },

  { path: 'breeds', component: AllBreeds },
  { path: 'breed/:id', component: BreedDetails },

  { path: 'add-breed', component: AddBreed },
  { path: 'contact', component: Contact },
  { path: 'login', component: Login },
  { path: 'register', component: Register, children: [] },
  { path: 'register/doctor', component: DoctorRegisterForm },
  { path: 'register/customer', component: CustomerRegisterForm },
  { path: 'register', component: Register ,children: []},
  { path:'profile/:id',component:Profile , children:[
    // {path:'doctor',component: Doctor},
    // {path:'customer', component:Customer}
    ]},
  {path:"register/doctor", component:DoctorRegisterForm},
  {path:"register/customer", component:CustomerRegisterForm},
  {path:"notfound/doctor",component:NotFoundDoctor}
];
