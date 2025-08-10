using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.Customer;
using PetConnect.BLL.Services.DTOs.Notification;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Migrations;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.UnitTests
{
    public class CustomerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAttachmentService> _attachmentServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ICustomerAddedPetsService> _customerPetsServiceMock;
        private readonly Mock<IPetService> _PetServiceMock;
        private readonly CustomerService _customerService;
        private readonly IFixture _fixture;

        public CustomerServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _PetServiceMock = new Mock<IPetService>();
            _attachmentServiceMock = new Mock<IAttachmentService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _customerPetsServiceMock = new Mock<ICustomerAddedPetsService>();
            _customerService = new CustomerService(_unitOfWorkMock.Object,_PetServiceMock.Object,_attachmentServiceMock.Object,_customerPetsServiceMock.Object, _notificationServiceMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task RequestAdoption_ShouldAddCustomerPetAdoption_WhenCalled()
        {
            // Arrange
            int petId = 4;
            var reqCustomerId = "Requester123";
            var recCustomerId = "Receiver456";

            var pet = new Pet { Id = petId, Name = "Buddy" };

            var adoptionDto = new CusRequestAdoptionDto
            {
                PetId = petId,
                RecCustomerId = recCustomerId
            };

            var CusReqAdoption = new CustomerPetAdoptions()
            {
                RequesterCustomerId = reqCustomerId,
                ReceiverCustomerId = adoptionDto.RecCustomerId,
                PetId = adoptionDto.PetId,
                Status = AdoptionStatus.Pending,
                AdoptionDate = DateTime.Now,
            };

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns(pet);
            _unitOfWorkMock.Setup(u => u.CustomerPetAdpotionsRepository.Add(CusReqAdoption));

            // Act
            await _customerService.RequestAdoption(adoptionDto, reqCustomerId);

            // Assert
            _unitOfWorkMock.Verify(u =>
                u.CustomerPetAdpotionsRepository.Add(It.Is<CustomerPetAdoptions>(a =>
                    a.RequesterCustomerId == reqCustomerId &&
                    a.ReceiverCustomerId == recCustomerId &&
                    a.PetId == petId &&
                    a.Status == AdoptionStatus.Pending &&
                    a.AdoptionDate.Date == DateTime.Now.Date
                )),
                Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task DeleteRequestAdoption_ShouldDeleteRecord_WhenRecordExists()
        {
            // Arrange
            var reqCustomerId = "Requester123";
            var recCustomerId = "Receiver456";
            var petId = 4;
            var adoptionDate = DateTime.Now.ToString("yyyy-MM-dd");

            var adoptionRecord = new CustomerPetAdoptions
            {
                RequesterCustomerId = reqCustomerId,
                ReceiverCustomerId = recCustomerId,
                PetId = petId,
                AdoptionDate = DateTime.Parse(adoptionDate)
            };

            var pet = new Pet { Id = petId, Name = "Buddy" };

            var dto = new DelCusRequestAdoptionDto
            {
                PetId = petId,
                RecCustomerId = recCustomerId,
                AdoptionDate = adoptionDate
            };

            _unitOfWorkMock.Setup(u => u.CustomerPetAdpotionsRepository
                .GetCustomerAdoptionRecord(recCustomerId, reqCustomerId, petId, adoptionDate))
                .Returns(adoptionRecord);

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns(pet);

            _unitOfWorkMock.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = await _customerService.DeleteRequestAdoption(dto, reqCustomerId);

            // Assert
            result.Should().Be(1);
            _unitOfWorkMock.Verify(u => u.CustomerPetAdpotionsRepository.Delete(adoptionRecord), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task DeleteRequestAdoption_ShouldReturnZero_WhenRecordDoesNotExist()
        {
            // Arrange
            var reqCustomerId = "Requester123";
            var dto = new DelCusRequestAdoptionDto
            {
                PetId = 4,
                RecCustomerId = "Receiver456",
                AdoptionDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            _unitOfWorkMock.Setup(u => u.CustomerPetAdpotionsRepository
                .GetCustomerAdoptionRecord(dto.RecCustomerId, reqCustomerId, dto.PetId, dto.AdoptionDate))
                .Returns((CustomerPetAdoptions)null);

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(4)).Returns(new Pet()); // used for notification service , so any pet will do it 

            // Act
            var result = await _customerService.DeleteRequestAdoption(dto, reqCustomerId);

            // Assert
            result.Should().Be(0);
            _unitOfWorkMock.Verify(u => u.CustomerPetAdpotionsRepository.Delete(It.IsAny<CustomerPetAdoptions>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Fact]
        public void GetCustomerReqAdoptionsPendingData_ShouldReturnPendingAdoptionsDetails_WhenDataExists()
        {
            // Arrange
            var userId = "Requester123";
            var petId = 4;
            var breedId = 2;
            var categoryId = 10;

            var pendingAdoptions = new List<CustomerPetAdoptions>
            {
                new CustomerPetAdoptions
                {
                    RequesterCustomerId = userId,
                    ReceiverCustomerId = "Receiver456",
                    PetId = petId,
                    Status = AdoptionStatus.Pending,
                   AdoptionDate = DateTime.Parse("2025-08-01")
                }
            };

            var pet = new Pet
            {
                Id = petId,
                Name = "Buddy",
                BreedId = breedId
            };

            var breed = new PetBreed
            {
                Id = breedId,
                Name = "Golden Retriever",
                CategoryId = categoryId
            };

            var category = new PetCategory
            {
                Id = categoryId,
                Name = "Dog"
            };

            _unitOfWorkMock.Setup(u => u.CustomerPetAdpotionsRepository.GetAllQueryable(false))
                .Returns(pendingAdoptions.AsQueryable());

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns(pet);

            _unitOfWorkMock.Setup(u => u.PetBreedRepository.GetByID(breedId))
                .Returns(breed);

            _unitOfWorkMock.Setup(u => u.PetCategoryRepository.GetByID(categoryId))
                .Returns(category);

            // Act
            var result = _customerService.GetCustomerReqAdoptionsPendingData(userId);

            // Assert
            result.Should().HaveCount(1);

            var adoptionDetails = result.First();
            adoptionDetails.AdoptionDate.Should().Be(DateTime.Parse("2025-08-01"));
            adoptionDetails.AdoptionStatus.Should().Be(AdoptionStatus.Pending.ToString());
            adoptionDetails.PetName.Should().Be("Buddy");
            adoptionDetails.PetBreadName.Should().Be("Golden Retriever");
            adoptionDetails.PetCategoryName.Should().Be("Dog");
            adoptionDetails.RecCustomerId.Should().Be("Receiver456");
            adoptionDetails.PetId.Should().Be(petId);
        }

        [Fact]
        public void GetCustomerRecAdoptionsPendingData_ShouldReturnPendingReceivedAdoptions_WhenDataExists()
        {
            // Arrange
            var userId = "Receiver123";
            var petId = 4;
            var breedId = 2;
            var categoryId = 10;

            var pendingAdoptions = new List<CustomerPetAdoptions>
            {
                new CustomerPetAdoptions
                {
                    RequesterCustomerId = "Requester456",
                    RequesterCustomer = new Customer
                    {
                        FName = "John",
                        LName = "Doe",
                        PhoneNumber = "1234567890"
                    },
                    ReceiverCustomerId = userId,
                    ReceiverCustomer = new Customer { Id = userId },
                    PetId = petId,
                    Pet = new Pet
                    {
                        Id = petId,
                        Name = "Buddy",
                        BreedId = breedId,
                        Breed = new PetBreed
                        {
                            Id = breedId,
                            Name = "Golden Retriever",
                            CategoryId = categoryId,
                            Category = new PetCategory
                            {
                                Id = categoryId,
                                Name = "Dog"
                            }
                        }
                    },
                    Status = AdoptionStatus.Pending,
                    AdoptionDate = DateTime.Parse("2025-08-01")
                }
            };

            _unitOfWorkMock
                .Setup(u => u.CustomerPetAdpotionsRepository.GetAllQueryable(false))
                .Returns(pendingAdoptions.AsQueryable());

            // Act
            var result = _customerService.GetCustomerRecAdoptionsPendingData(userId).ToList();

            // Assert
            result.Should().HaveCount(1);

            var adoption = result.First();
            adoption.AdoptionDate.Should().Be(DateTime.Parse("2025-08-01"));
            adoption.PetId.Should().Be(petId);
            adoption.PetName.Should().Be("Buddy");
            adoption.PetBreadName.Should().Be("Golden Retriever");
            adoption.PetCategoryName.Should().Be("Dog");
            adoption.ReqCustomerId.Should().Be("Requester456");
            adoption.RequesterFullName.Should().Be("John Doe");
            adoption.ReqPhoneNumber.Should().Be("1234567890");
            adoption.AdoptionStatus.Should().Be(AdoptionStatus.Pending);
        }

        [Fact]
        public async Task ApproveOrCancelCustomerAdoptionRequest_WhenApproved_ShouldApproveAndRemoveOtherRequests()
        {
            // Arrange
            var petId = 4;
            var recUserId = "Receiver123";
            var reqUserId = "Requester456";
            var adoptionDate = DateTime.Now.ToString("yyyy-MM-dd");

            var pet = new PetDetailsDto { Id = petId, Name = "Buddy" };

            var adoptionRecord = new CustomerPetAdoptions
            {
                RequesterCustomerId = reqUserId,
                ReceiverCustomerId = recUserId,
                PetId = petId,
                AdoptionDate = DateTime.Parse(adoptionDate),
                Status = AdoptionStatus.Pending
            };

            var dto = new ApproveORCancelReceivedCustomerRequest
            {
                PetId = petId,
                ReqCustomerId = reqUserId,
                AdoptionDate = adoptionDate,
                AdoptionStatus = AdoptionStatus.Approved
            };

            var otherRequesterIds = new List<string> { "other1", "other2" };

            // use unit of work + pet service mocks only
            _PetServiceMock.Setup(s => s.GetPet(petId)).Returns(pet);

            _unitOfWorkMock
                .Setup(u => u.CustomerPetAdpotionsRepository
                    .GetCustomerAdoptionRecord(recUserId, reqUserId, petId, adoptionDate))
                .Returns(adoptionRecord);

            _unitOfWorkMock
                .Setup(u => u.CustomerAddedPetsRepository.DeleteCustomerAddedPetRecord(petId, recUserId))
                .Returns(1);

            _unitOfWorkMock
                .Setup(u => u.CustomerPetAdpotionsRepository.RemoveOtherRequestsForPet(petId, reqUserId))
                .Returns(otherRequesterIds);

            _unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(1);

            // Act
            var result = await _customerService.ApproveOrCancelCustomerAdoptionRequest(dto, recUserId);

            // Assert
            result.Should().Be(AdoptionStatus.Approved.ToString());
            adoptionRecord.Status.Should().Be(AdoptionStatus.Approved);

            _unitOfWorkMock.Verify(u =>
                u.CustomerAddedPetsRepository.DeleteCustomerAddedPetRecord(petId, recUserId),
                Times.Once);

            // Ensure requester was registered (service is used in your class)
            _customerPetsServiceMock.Verify(s => s.RegisterCustomerPetAddition(reqUserId, petId), Times.Once);

            _unitOfWorkMock.Verify(u =>
                u.CustomerPetAdpotionsRepository.RemoveSingleReq(recUserId, reqUserId, petId),
                Times.Once);

            _unitOfWorkMock.Verify(u =>
                u.CustomerPetAdpotionsRepository.RemoveOtherRequestsForPet(petId, reqUserId),
                Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task ApproveOrCancelCustomerAdoptionRequest_WhenCancelled_ShouldRemoveSingleRequestAndReturnCancelled()
        {
            // Arrange
            var petId = 5;
            var recUserId = "ReceiverABC";
            var reqUserId = "RequesterXYZ";
            var adoptionDate = DateTime.Now.ToString("yyyy-MM-dd");

            var pet = new PetDetailsDto { Id = petId, Name = "Milo" };

            var adoptionRecord = new CustomerPetAdoptions
            {
                RequesterCustomerId = reqUserId,
                ReceiverCustomerId = recUserId,
                PetId = petId,
                AdoptionDate = DateTime.Parse(adoptionDate),
                Status = AdoptionStatus.Pending
            };

            var dto = new ApproveORCancelReceivedCustomerRequest
            {
                PetId = petId,
                ReqCustomerId = reqUserId,
                AdoptionDate = adoptionDate,
                AdoptionStatus = AdoptionStatus.Cancelled
            };

            _PetServiceMock.Setup(s => s.GetPet(petId)).Returns(pet);

            _unitOfWorkMock
                .Setup(u => u.CustomerPetAdpotionsRepository
                    .GetCustomerAdoptionRecord(recUserId, reqUserId, petId, adoptionDate))
                .Returns(adoptionRecord);

            _unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(1);

            // Act
            var result = await _customerService.ApproveOrCancelCustomerAdoptionRequest(dto, recUserId);

            // Assert
            result.Should().Be(AdoptionStatus.Cancelled.ToString());

            _unitOfWorkMock.Verify(u =>
                u.CustomerPetAdpotionsRepository.RemoveSingleReq(recUserId, reqUserId, petId),
                Times.Once);

            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task ApproveOrCancelCustomerAdoptionRequest_WhenRecordNotFound_ShouldReturnNullAndDoNothing()
        {
            // Arrange
            var petId = 6;
            var recUserId = "ReceiverNO";
            var reqUserId = "RequesterNO";
            var adoptionDate = DateTime.Now.ToString("yyyy-MM-dd");

            var dto = new ApproveORCancelReceivedCustomerRequest
            {
                PetId = petId,
                ReqCustomerId = reqUserId,
                AdoptionDate = adoptionDate,
                AdoptionStatus = AdoptionStatus.Approved
            };

            _unitOfWorkMock
                .Setup(u => u.CustomerPetAdpotionsRepository
                    .GetCustomerAdoptionRecord(recUserId, reqUserId, petId, adoptionDate))
                .Returns((CustomerPetAdoptions?)null);

            // Act
            var result = await _customerService.ApproveOrCancelCustomerAdoptionRequest(dto, recUserId);

            // Assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Never);
            _unitOfWorkMock.Verify(u =>
                u.CustomerPetAdpotionsRepository.RemoveSingleReq(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
                Times.Never);
            _customerPetsServiceMock.Verify(s => s.RegisterCustomerPetAddition(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetCustomerOwnedPets_ShouldReturnOwnedPetsForGivenUserId() //Owned mean any pet (forAdption , rescue , owned)
        {
            // Arrange
            var userId = "customer123";

            var category = new PetCategory { Name = "Dog" };
            var breed = new PetBreed { Category = category };
            var customer = new Customer { Id = userId };

            var pets = new List<Pet>
            {
                new Pet
                {
                    Id = 1,
                    Name = "Buddy",
                    ImgUrl = "buddy.jpg",
                    Status = PetStatus.Owned,
                    Age = 3,
                    Breed = breed,
                    CustomerAddedPets = new CustomerAddedPets { CustomerId = userId, Customer = customer }
                },
                new Pet
                {
                    Id = 2,
                    Name = "Max",
                    ImgUrl = "max.jpg",
                    Status = PetStatus.ForAdoption,
                    Age = 5,
                    Breed = breed,
                    CustomerAddedPets = new CustomerAddedPets { CustomerId = userId, Customer = customer }
                }
            }.AsQueryable();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetAllQueryable(false))
                           .Returns(pets);

            // Act
            var result = _customerService.GetCustomerOwnedPets(userId);

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(r => r.Name == "Buddy" && r.ImgUrl == "/assets/PetImages/buddy.jpg");
        }

        [Fact]
        public void GetProfile_ShouldReturnCustomerDetails_WhenCustomerExists()
        {
            // Arrange
            var customerId = "123";
            var customer = new Customer
            {
                Id = customerId,
                UserName = "john_doe",
                FName = "John",
                LName = "Doe",
                ImgUrl = "john.jpg",
                Gender = Gender.Male,
                Address = new Address { Street = "Main St", City = "Cairo", Country = "Egypt" },
                Email = "john@example.com",
                IsApproved = true,
                PhoneNumber = "01000000000"
            };

            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                           .Returns(customer);

            // Act
            var result = _customerService.GetProfile(customerId);

            // Assert
            result.Should().NotBeNull(); 
            result.UserName.Should().Be("john_doe");
            result.City.Should().Be("Cairo");
        }

        [Fact]
        public void GetProfile_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = "999";
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                           .Returns((Customer)null);

            // Act
            var result = _customerService.GetProfile(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllCustomers_ShouldReturnCustomerList()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = "1",
                    FName = "John",
                    LName = "Doe",
                    ImgUrl = "john.jpg",
                    Address = new Address { City = "Cairo" }
                },
                new Customer
                {
                    Id = "2",
                    FName = "Jane",
                    LName = "Smith",
                    ImgUrl = "jane.jpg",
                    Address = new Address { City = "Alex" }
                }
            };

            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetAll(false))
                           .Returns(customers);

            // Act
            var result = _customerService.GetAllCustomers().ToList();

            // Assert
            result.Count.Should().Be(2);
            result[0].City.Should().Be("Cairo");
            result[1].City.Should().Be("Alex");
        }

        [Fact]
        public void Delete_ShouldRemoveCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = "123";
            var customer = new Customer { Id = customerId };

            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                           .Returns(customer);

            _unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(1);

            // Act
            var result = _customerService.Delete(customerId);

            // Assert
            Assert.Equal(1, result);
            _unitOfWorkMock.Verify(u => u.CustomerRepository.Delete(customer), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnZero_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = "999";
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                           .Returns((Customer)null);

            // Act
            var result = _customerService.Delete(customerId);

            // Assert
            result.Should().Be(0);
            _unitOfWorkMock.Verify(u => u.CustomerRepository.Delete(It.IsAny<Customer>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateProfile_ShouldUpdateCustomerAndReturn1_WhenSuccessful()
        {
            // Arrange
            var customerId = "customer1";

            var existingCustomer = new Customer() { Id = customerId, ImgUrl = "old-image.jpg" };

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.FileName).Returns("new-image.jpg");

            var dto = new UpdateCustomerProfileDTO() { ImageFile = mockFile.Object};

            // Setup mocks
            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                .Returns(existingCustomer);
            _unitOfWorkMock.Setup(u => u.SaveChanges())
                .Returns(1);
            _attachmentServiceMock.Setup(a => a.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("new-image.jpg");

            // Act
            var result = await _customerService.UpdateProfile(dto, customerId);

            // Assert
            result.Should().Be(1);
            existingCustomer.FName.Should().Be(dto.FName);
            existingCustomer.LName.Should().Be(dto.LName);
            existingCustomer.ImgUrl.Should().Be($"/assets/img/person/new-image.jpg");
            existingCustomer.Address.City.Should().Be(dto.City);

            _unitOfWorkMock.Verify(u => u.CustomerRepository.Update(existingCustomer), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task UpdateProfile_ShouldReturn0_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = "nonexistent";
            var dto = new UpdateCustomerProfileDTO() { };

            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                .Returns((Customer)null);

            // Act
            var result = await _customerService.UpdateProfile(dto, customerId);

            // Assert
            result.Should().Be(0);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task UpdateProfile_ShouldNotUpdateImage_WhenNoFileProvided()
        {
            // Arrange
            var customerId = "customer1";
            var existingCustomer = new Customer() { Id = customerId, ImgUrl = "old-image.jpg" };

            var dto = _fixture.Build<UpdateCustomerProfileDTO>()
                .Without(x => x.ImageFile)
                .Create();

            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetByID(customerId))
                .Returns(existingCustomer);
            _unitOfWorkMock.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = await _customerService.UpdateProfile(dto, customerId);

            // Assert
            result.Should().Be(1);
            existingCustomer.ImgUrl.Should().Be("old-image.jpg");
            _attachmentServiceMock.Verify(a => a.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void GetCustomerById_ShouldReturnCustomerData_WhenCustomerExists()
        {
            // Arrange
            var customerId = "customer1";
            var mockCustomer = new Customer()
            {
                Id = customerId,
                FName = "John",
                LName = "Doe",
                ImgUrl = "profile.jpg",

                Address = new Address { City = "New York" }
            };

            _unitOfWorkMock.Setup(u => u.CustomerRepository.GetAll(false))
                .Returns(new List<Customer> { mockCustomer }.AsQueryable());

            // Act
            var result = _customerService.GetCustomerById(customerId);

            // Assert
            result.Should().NotBeNull();
            result.CustomerId.Should().Be(customerId);
            result.FName.Should().Be("John");
            result.LName.Should().Be("Doe");
            result.ImgUrl.Should().Be("profile.jpg");
            result.City.Should().Be("New York");
        }

    }
}
