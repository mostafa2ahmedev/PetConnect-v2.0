import { Routes } from '@angular/router';
import { Breeds } from './Feature/breeds/breeds';
import { Contact } from './Feature/contact/contact';
import { Login } from './Feature/login/login';
import { Register } from './Feature/register/register';
import { Home } from './Feature/home/home';
import { AddPets } from './Feature/add-pets/add-pets';
import { Pets } from './Feature/pets/pets';
import { Categories } from './Feature/categories/categories';
import { Doctors } from './Feature/doctors/doctors';
import { DoctorRegisterForm } from './Feature/doctor-register-form/doctor-register-form';
import { CustomerRegisterForm } from './Feature/customer-register-form/customer-register-form';
import { DoctorProfile } from './Feature/doctor-profile/doctor-profile';
import { DoctorEditProfile } from './Feature/doctor-edit-profile/doctor-edit-profile';
import { NotFoundDoctor } from './Feature/not-found-doctor/not-found-doctor';
export const routes: Routes = [
      { path: '', component: Home },
  { path: 'doctors', component: Doctors },
  { path: 'doctors/:id', component: DoctorProfile },
  { path: 'doctors/update/:id', component: DoctorEditProfile },
  { path: 'pets', component: Pets },
  { path: 'add-pet', component: AddPets },
  { path: 'categories', component: Categories },
  { path: 'breeds', component: Breeds },
  { path: 'contact', component: Contact },
  { path: 'login', component: Login },
  { path: 'register', component: Register ,children: []},
  {path:"register/doctor", component:DoctorRegisterForm},
  {path:"register/customer", component:CustomerRegisterForm},
  {path:"notfound/doctor",component:NotFoundDoctor}
];
