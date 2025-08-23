using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Identity;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.UnitTests
{

    public class AdminServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _notificationServiceMock = new Mock<INotificationService>();
            _adminService = new AdminService(_unitOfWorkMock.Object, _notificationServiceMock.Object);
        }

        [Fact]
        public void GetPendingDoctorsAndPets_ShouldReturnCorrectData()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = "doc1",
                FName = "John",
                LName = "Doe",
                IsApproved = false,
                IsDeleted = false,
                PetSpecialty = PetSpecialty.Dog,
                Gender = Gender.Male,
                PricePerHour = 50,
                Address = new Address { Street = "Street1", City = "City1" }
            };

            var pet = new Pet
            {
                Id = 1,
                Name = "Buddy",
                Status = PetStatus.ForAdoption,
                IsApproved = false,
                IsDeleted = false,
                ImgUrl = "petimg.jpg",
                Breed = new PetBreed { Name = "Labrador", Category = new PetCategory { Name = "Dog" } }
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAll(false)).Returns(new List<Doctor> { doctor });
            _unitOfWorkMock.Setup(u => u.PetRepository.GetPendingPetsWithBreedAndCategory()).Returns(new List<Pet> { pet }.AsQueryable());

            // Act
            var result = _adminService.GetPendingDoctorsAndPets();

            // Assert
            result.PendingDoctors.Should().HaveCount(1);
            result.PendingPets.Should().HaveCount(1);
            result.PendingDoctors.First().FName.Should().Be("John");
            result.PendingPets.First().Name.Should().Be("Buddy");
        }

        [Fact]
        public void ApproveDoctor_ShouldUpdateAndReturnDoctorDetails()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = "doc1",
                FName = "John",
                LName = "Doe",
                IsApproved = false,
                IsDeleted = false,
                PetSpecialty = PetSpecialty.Cat,
                Address = new Address { Street = "S1", City = "C1" }
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("doc1")).Returns(doctor);

            // Act
            var result = _adminService.ApproveDoctor("doc1");

            // Assert
            result.Should().NotBeNull();
            result.IsApproved.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
            _notificationServiceMock.Verify(n => n.CreateAndSendNotification("doc1", It.IsAny<NotificationDTO>()), Times.Once);
        }

        [Fact]
        public async Task ApprovePet_ShouldUpdateAndReturnPetDetails()
        {
            // Arrange
            var pet = new Pet { Id = 1, Name = "Buddy", Status = PetStatus.ForAdoption,IsApproved = false };
            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(1)).Returns(pet);
            _unitOfWorkMock.Setup(u => u.CustomerAddedPetsRepository.GetAllQueryable(false))
                .Returns(new List<CustomerAddedPets> { new CustomerAddedPets { PetId = 1, CustomerId = "cust1" } }.AsQueryable());

            // Act
            var result = await _adminService.ApprovePet(1);

            // Assert
            result.Should().NotBeNull();
            result.IsApproved.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.PetRepository.Update(It.IsAny<Pet>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
            _notificationServiceMock.Verify(n => n.CreateAndSendNotification("cust1", It.IsAny<NotificationDTO>()), Times.Once);
        }

        [Fact]
        public async Task RejectDoctor_ShouldMarkAsDeletedAndSendNotification()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = "doc1",
                FName = "John",
                LName = "Doe",
                PetSpecialty = PetSpecialty.Bird,
                IsDeleted = false,
                Address = new Address { Street = "S1", City = "C1" }
            };

            AdminDoctorMessage adminDoctorMessage = new AdminDoctorMessage()
            {
                MessageType = AdminMessageType.Rejection,
                Message = "message",
                DoctorId = "doc1"
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("doc1")).Returns(doctor);
            _unitOfWorkMock.Setup(u => u.AdminDoctorMessageRepository.Add(adminDoctorMessage));

            // Act
            var result = await _adminService.RejectDoctor("doc1", "Reason");

            // Assert
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.AdminDoctorMessageRepository.Add(It.IsAny<AdminDoctorMessage>()), Times.Once);
            _notificationServiceMock.Verify(n => n.CreateAndSendNotification("doc1", It.IsAny<NotificationDTO>()), Times.Once);
        }

        [Fact]
        public void RejectPet_ShouldMarkAsDeletedAndSendNotification()
        {
            // Arrange
            var pet = new Pet { Id = 1, Name = "Buddy", Status = PetStatus.ForAdoption,IsDeleted=false };

            AdminPetMessage adminPetMessage = new AdminPetMessage()
            {
                MessageType = AdminMessageType.Rejection,
                PetId = 1,
                Message = "message"
            };

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(1)).Returns(pet);
            _unitOfWorkMock.Setup(u => u.AdminPetMessageRepository.Add(adminPetMessage));
            _unitOfWorkMock.Setup(u => u.CustomerAddedPetsRepository.GetAllQueryable(false))
                .Returns(new List<CustomerAddedPets> { new CustomerAddedPets { PetId = 1, CustomerId = "cust1" } }.AsQueryable());

            // Act
            var result = _adminService.RejectPet(1, "Reason");

            // Assert
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeTrue();
            _unitOfWorkMock.Verify(u => u.AdminPetMessageRepository.Add(It.IsAny<AdminPetMessage>()), Times.Once);
            _notificationServiceMock.Verify(n => n.CreateAndSendNotification("cust1", It.IsAny<NotificationDTO>()), Times.Once);
        }

        [Fact]
        public void  GetProfile_ShouldReturnProfile_WhenExists()
        {
            // Arrange
            var admin = new Admin
            {
                Id = "admin1",
                UserName = "AdminUser",
                FName = "First",
                LName = "Last",
                ImgUrl = "img.png",
                Gender = Gender.Female,
                Address = new Address { Street = "S1", City = "C1", Country = "Country1" },
                Email = "admin@test.com",
                IsApproved = true,
                PhoneNumber = "123"
            };
            _unitOfWorkMock.Setup(u => u.AdminRepository.GetByID("admin1")).Returns(admin);

            // Act
            var result = _adminService.GetProfile("admin1");

            // Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be("AdminUser");
        }
    }
}


