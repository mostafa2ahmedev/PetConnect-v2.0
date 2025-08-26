# 🐾 PetConnect

PetConnect is a **full-stack platform** that connects **Customers, Doctors, Sellers, and Admins** in a single ecosystem.  
It enables **Pet Adoption & Rescue, Veterinary Appointments, E-Commerce, Blogging and Real-time Communication**, making it a **one-stop solution** for pet care and community.

---

## 🌐 Live Demo

- **API Documentation (Swagger, Interactive):** [https://petconnect.com/swagger](http://petconnect.runasp.net/swagger/index.html)  

> ⚠️ Note: Most endpoints require a **JWT token** for authentication and authorization.  
> To fully test all endpoints, follow these steps:  
> 1. **Register** a new user via the `/api/customers` or `/api/doctors` endpoint.  
> 2. **Login** using `/api/auth/login` with your credentials.  
> 3. The response will include a **JWT token**.  
> 4. Include this token in the `Authorization` header (`Bearer <token>`) when calling protected endpoints.  
>
> 🔹 You can test endpoints directly in **Swagger**, but for endpoints that require a JWT, it’s recommended to use **Postman** or another API client to include the token in requests.  

---

## 🌟 Core Features

- 🐕 **Pet Adoption & Rescue**: Browse, request, and chat with owners before ownership transfer (admin approval required).  
- 👨‍⚕️ **Doctor Services**: Verified doctors manage appointments, share blogs, and interact with customers.  
- 🏪 **E-Commerce**: Redis-backed shopping cart, secure payments with Stripe, and seller product management.  
- 💬 **Real-time Communication**: SignalR-powered chat & notifications (adoptions, appointments, orders, tickets).  
- 🛠️ **Admin Management**: Verify doctors, approve pets, resolve tickets, and monitor platform insights.  
- 📖 **Blog System**: Doctors post articles, customers interact via comments in real time.  
- 🎫 **Help Center**: Customers open support tickets; admins resolve them with real-time updates.  

---

## 👥 Roles & Dashboards

### 🐶 Customer
- Manage profile, pets, adoption requests.  
- Book doctor appointments & submit reviews.  
- Browse products, manage cart (Redis), pay with Stripe.  
- Open support tickets & get real-time notifications.  

### 👨‍⚕️ Doctor
- Register with verification (**Face++ + certificate**).  
- Manage appointment slots, confirm/reject bookings.  
- Write blogs, engage with customers.  
- Track reviews & ratings.  

### 🏪 Seller
- Manage products (add/update/remove).  
- Track orders & mark as delivered.  
- Receive live notifications on new orders.  

### 🛠️ Admin
- Verify doctors.  
- Approve/reject pets for adoption.  
- Manage tickets & monitor platform activity.  
- Access insights & system monitoring.  

---

## 🏗️ Architecture  

PetConnect is designed as a **Layered Monolith** with a clear separation of concerns, complemented by a modern Angular frontend and a dedicated testing suite.  

### Backend  
- **Presentation Layer (PetConnect.API)**  
  - Exposes the application via a RESTful API.  
  - Handles authentication, authorization, request validation.  
  - Delegates business operations to the BLL.  

- **Business Logic Layer (PetConnect.BLL)**  
  - Implements core business workflows and rules.  
  - Orchestrates operations across repositories, external services, and ensures domain consistency.  
  - Integrates with:  
    - **Stripe** → secure payments  
    - **Face++** → doctor verification  

- **Data Access Layer (PetConnect.DAL)**  
  - Uses **Entity Framework Core** with a **Code-First** approach.  
  - Manages repositories, queries, and migrations.  

### Frontend  
- Built with **Angular 20**.  
- Uses **RxJS** for reactive programming and **Bootstrap** for styling.  
- Communicates with the API through HTTP services and **SignalR hubs** for real-time updates.  

### Cross-Cutting Concerns  
- **SignalR** → Real-time chat and notifications.  
- **Redis** → Caching, session management, shopping cart.  

### Testing  
- **xUnit** → Unit & integration testing framework.  
- **Moq** → Mocking external dependencies for isolated testing.  
- **FluentAssertions** → Improves readability and maintainability of test assertions.  

---

## 🛠️ Tech Stack

### Backend
- ![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=white)
- ![EF Core](https://img.shields.io/badge/Entity_Framework_Core-68217A?logo=ef&logoColor=white)
- ![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white)
- ![SignalR](https://img.shields.io/badge/SignalR-5C2D91?logo=dotnet&logoColor=white)
- ![Stripe](https://img.shields.io/badge/Stripe-008CDD?logo=stripe&logoColor=white)
- ![Face++](https://img.shields.io/badge/Face++-0A9CF3?logo=azure-face-api&logoColor=white)

### Frontend
- ![Angular](https://img.shields.io/badge/Angular-DD0031?logo=angular&logoColor=white)
- ![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?logo=typescript&logoColor=white)
- ![RxJS](https://img.shields.io/badge/RxJS-B7178C?logo=reactivex&logoColor=white)
- ![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=white)

#### Testing
- ![xUnit](https://img.shields.io/badge/xUnit-5B2C6F?logo=xunit&logoColor=white)
- ![Moq](https://img.shields.io/badge/Moq-512BD4?logo=nuget&logoColor=white)
- ![FluentAssertions](https://img.shields.io/badge/FluentAssertions-2E86C1?logo=nuget&logoColor=white)

---

## 📂 Project Structure

```text
PetConnect/                # Root Monorepo
│── Backend/
│   ├── PetConnect.API/    # ASP.NET Core Web API (Controllers, Startup, Configurations)
│   ├── PetConnect.BLL/    # Business Logic Layer (Services, Managers, DTOs, Validation)
│   ├── PetConnect.DAL/    # Data Access Layer (Repositories, EF Core, DbContext, Migrations)
│
│── Frontend/
│   ├── petconnect-angular/ # Angular 20 App (Components, Services, Reactive Forms, Bootstrap)
│
│── Tests/
│   ├── PetConnect.Tests/   # Unit & Integration Tests (xUnit, Moq, FluentAssertions)
│
│── README.md              # Documentation

---


