import { Routes } from '@angular/router';

// --- استيراد كل المكونات ---
import { Home } from './Feature/home/home';
import { Doctors } from './Feature/doctors/doctors';
import { DoctorProfile } from './Feature/doctor-profile/doctor-profile';
import { DoctorEditProfile } from './Feature/doctor-edit-profile/doctor-edit-profile';
import { DoctorRegisterForm } from './Feature/doctor-register-form/doctor-register-form';
import { CustomerRegisterForm } from './Feature/customer-register-form/customer-register-form';
import { Register } from './Feature/register/register';
import { Login } from './Feature/login/login';
import { Contact } from './Feature/contact/contact';
import { Pets } from './Feature/pets/All-Pets/pets';
import { PetDetails } from './Feature/pets/pet-details/pet-details';
import { AddPets } from './Feature/pets/add-pets/add-pets';
import { UpdatePet } from './Feature/pets/update-pet/update-pet';
import { Categories } from './Feature/categories/All-categories/categories';
import { CategoryDetails } from './Feature/categories/category-details/category-details';
import { AddCategory } from './Feature/categories/add-category/add-category';
import { AllBreeds } from './Feature/breeds/all-breeds/all-breeds';
import { BreedDetails } from './Feature/breeds/breed-details/breed-details';
import { AddBreed } from './Feature/breeds/add-breed/add-breed';
import { NotFoundDoctor } from './Feature/not-found-doctor/not-found-doctor';
import { OrdersComponent } from './Feature/orders/orders';
import { ProductsComponent } from './Feature/Products/all-products/all-products';
import { ProductDetailsComponent } from './Feature/Products/product-details/product-details';
import { CartComponent } from './Feature/cart/cart';


import { FaceComparisonComponent } from './Feature/FaceRecognition/face-comparison/face-comparison';
import { RegistrationGuard } from '././core/guards/registration-guard'; 

export const routes: Routes = [
  // --- المسارات الأساسية ---
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'contact', component: Contact },

  // --- مسارات الأطباء ---
  { path: 'doctors', component: Doctors },
  { path: 'doctors/:id', component: DoctorProfile },
  { path: 'doctors/update/:id', component: DoctorEditProfile },
  { path: 'notfound/doctor', component: NotFoundDoctor },

  
  { path: 'register', component: Register }, 
{ path: 'register/customer', component: CustomerRegisterForm },


{
  path: 'auth/face-compare', 
  component: FaceComparisonComponent 
},


{
  path: 'auth/doctor-register', 
  component: DoctorRegisterForm,
  canActivate: [RegistrationGuard] 
},

  // --- مسارات الحيوانات الأليفة ---
  { path: 'pets', component: Pets },
  { path: 'pet-details/:id', component: PetDetails },
  { path: 'add-pet', component: AddPets },
  { path: 'pets/update/:id', component: UpdatePet },

  // --- مسارات الفئات ---
  { path: 'categories', component: Categories },
  { path: 'category/:id', component: CategoryDetails },
  { path: 'add-category', component: AddCategory },

  // --- مسارات السلالات ---
  { path: 'breeds', component: AllBreeds },
  { path: 'breed/:id', component: BreedDetails },
  { path: 'add-breed', component: AddBreed },

  // --- مسارات المنتجات والطلبات ---
  { path: 'all-products', component: ProductsComponent },
  { path: 'products/:id', component: ProductDetailsComponent },
  { path: 'cart', component: CartComponent },
  { path: 'orders', component: OrdersComponent },

];
