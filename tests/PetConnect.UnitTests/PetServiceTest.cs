
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PetConnect.BLL.Common.AttachmentServices;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.UnitTests
{
    public class PetServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAttachmentService> _attachmentServiceMock;
        private readonly Mock<ICustomerAddedPetsService> _customerPetsServiceMock;
        private readonly PetService _petService;
        private readonly IFixture _fixture;

        public PetServiceTest()
        {
            //making the mocking object for the the other dependencies
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _attachmentServiceMock = new Mock<IAttachmentService>();
            _customerPetsServiceMock = new Mock<ICustomerAddedPetsService>();
            _fixture = new Fixture();

            //assign the mocking objects to service so it can work dependently.
            //so the only service that is not mocked is the pet service that is the service i test here.
            _petService = new PetService(
                _unitOfWorkMock.Object,
                _attachmentServiceMock.Object,
                _customerPetsServiceMock.Object
            );

            //configure auto fixture to make a mock object of interface IFormFile
            _fixture.Register<IFormFile>(() =>
            {
                var fileMock = new Mock<IFormFile>();
                fileMock.Setup(f => f.FileName).Returns("test.jpg");
                fileMock.Setup(f => f.Length).Returns(100);
                fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
                return fileMock.Object;
            });

            // Handle circular references
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        //Add Pet Test the happy path, the only pass in the method in the service no other passes.
        [Fact]
        public async Task AddPet_PassCorrectData_PetShouldBeAdded()
        {
            //Arrange
            //auto fixture will create all these variable with default values to be used in the test
            //but we configured the IformFile in the contructor beacuse the package doesn't know how to create a varaible of it with the default value 
            var addedPet = _fixture.Create<AddedPetDto>();
            var customerId = _fixture.Create<string>();
            var fakeImageUrl = _fixture.Create<string>();

            _attachmentServiceMock.Setup(s => s.UploadAsync(addedPet.ImgURL, "PetImages"))
                .ReturnsAsync(fakeImageUrl);

            //this will have the fake pet that added in the unitOfWork so we can assert that the data that saved in the database is the same 
            //optional check we can do the test with out , but then we need to setup the add in the petRepository and don't use the callback 
            Pet? capturedPet = null;

            //this will be called in the act after we add with the dto and the customer id and in the service the dto will be mapped to a Pet then call add from the repository 
            _unitOfWorkMock.Setup(u => u.PetRepository.Add(It.IsAny<Pet>()))
                .Callback<Pet>(p => capturedPet = p);// put the added data in the capturedPet above.

            _unitOfWorkMock
                .SetupSequence(u => u.SaveChanges())
                .Returns(1).Returns(1);

            _customerPetsServiceMock
            .Setup(s => s.RegisterCustomerPetAddition(customerId, It.IsAny<int>()))
            .Verifiable();

            //Act 
            var res = await _petService.AddPet(addedPet, customerId);

            //Assert 
            res.Should().Be(1);
            capturedPet.Should().NotBeNull();
            capturedPet.Name.Should().Be(addedPet.Name);
            capturedPet.BreedId.Should().Be(addedPet.BreedId);
            capturedPet.BreedId.Should().Be(addedPet.BreedId);
            capturedPet.Age.Should().Be(addedPet.Age);
            capturedPet.ImgUrl.Should().Be(fakeImageUrl);
            capturedPet.Status.Should().Be(addedPet.Status);
        }

        [Fact]
        public void GetAllPetsAddedByCustomers_ShouldReturnAllPetsAddedByAnyCustomer()
        {
            // Arrange
            //we make a 3 pets with these data and the without because the autofixture can have cycle and will create all these object which will destroy the meomry and the test will take tooo long in my case it takes about 23 sec 
            var pets = _fixture.Build<Pet>()
             .With(p => p.Breed, new PetBreed
             {
                 Category = new PetCategory { Name = "Dogs" }
             })
             .With(p => p.CustomerAddedPets, new CustomerAddedPets
             {
                 CustomerId = "cust-123",
                 Customer = new Customer
                 {
                     FName = "John",
                     LName = "Doe",
                     Address = new Address
                     {
                         City = "Cairo",
                         Country = "Egypt",
                         Street = "Main St"
                     }
                 }
             })
             .Without(p => p.AdminPetMessages)
             .Without(p => p.CustomerPetAdoptions)
             .Without(p => p.ShelterAddedPets)
             .Without(p => p.ShelterPetAdoptions)
             .CreateMany(3)
             .ToList();

            //the mocked pets coming from database 
            _unitOfWorkMock.Setup(u => u.PetRepository.GetPetBreadCategoryDataWithCustomer())
                .Returns(pets.AsQueryable());

            // Act
            var result = _petService.GetAllPetsWithBelongsToCustomer().ToList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetAllApprovedPetsThatIsAddedByCustomer_ShouldReturnAllApprovedPetsThatisAddedByAnyCustomer()
        {
            var pets = _fixture.Build<Pet>()
             .With(p => p.Breed, new PetBreed
             {
                 Category = new PetCategory { Name = "Dogs" }
             })
             .With(p => p.CustomerAddedPets, new CustomerAddedPets
             {
                 CustomerId = "cust-123",
                 Customer = new Customer
                 {
                     FName = "John",
                     LName = "Doe",
                     Address = new Address
                     {
                         City = "Cairo",
                         Country = "Egypt",
                         Street = "Main St"
                     }
                 }
             })
             .Without(p => p.AdminPetMessages)
             .Without(p => p.CustomerPetAdoptions)
             .Without(p => p.ShelterAddedPets)
             .Without(p => p.ShelterPetAdoptions)
             .CreateMany(3)
             .ToList();

            //the mocked pets coming from database 
            _unitOfWorkMock.Setup(u => u.PetRepository.GetApprovedPetBreadCategoryDataWithCustomer())
                .Returns(pets.AsQueryable());

            // Act
            var result = _petService.GetAllApprovedPetsWithBelongsToCustomer().ToList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public void GetPetById_PetNotFoundWhenPassingFalseId_ShouldReturnNull()
        {
            //Arrange false if to return Null
            int wrongId = -1;
            _unitOfWorkMock.Setup(u => u.PetRepository.GetPetDetails(wrongId))
                .Returns((Pet)null);
            //Act
            var pet = _petService.GetPet(wrongId);
            //Assert
            pet.Should().BeNull();
        }

        [Fact]
        public void GetPetById_PetIsFoundAndBreedIsNotFound_ShouldReturnUnknownBreedAndCategory()
        {
            //Arrange pet is found and breed will be null
            int petIdWithNotFoundBreed = 2;
            var Pet = _fixture.Build<Pet>()
                .With(P => P.BreedId , 100). //100 represent false breedId which will get breed is null from repository 
                Without(P => P.AdminPetMessages).
                Without(P => P.CustomerAddedPets).
                Without(P => P.CustomerPetAdoptions).
                Without(P => P.ShelterAddedPets).
                Without(P => P.ShelterPetAdoptions).
                Without(P => P.Breed).Create();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetPetDetails(petIdWithNotFoundBreed))
                .Returns((Pet)Pet);

             _unitOfWorkMock.Setup(u => u.PetBreedRepository.GetByID(Pet.BreedId))
                .Returns((PetBreed)null);
            //Act 
            var res = _petService.GetPet(petIdWithNotFoundBreed);
            //Assert
            res.Should().NotBeNull();
            res.BreadName.Should().Be("Unknown");
            res.CategoryName.Should().Be("Unknown");
        }
        [Fact]
        public void GetPetById_PetIsFoundAndCategoryIsNotFound_ShouldReturnUnknownCategory()
        {
            // Arrange
            int petIdWithWithNotFoundCategory = 3;

            // Create a mock pet with a valid BreedId
            var pet = _fixture.Build<Pet>()
                .With(p => p.BreedId, 101)
                .Without(p => p.AdminPetMessages)
                .Without(p => p.CustomerAddedPets)
                .Without(p => p.CustomerPetAdoptions)
                .Without(p => p.ShelterAddedPets)
                .Without(p => p.ShelterPetAdoptions)
                .Without(p => p.Breed)
                .Create();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetPetDetails(petIdWithWithNotFoundCategory))
                .Returns(pet);

            // Create a mock breed with a CategoryId that is not found 
            var breed = _fixture.Build<PetBreed>()
                .With(b => b.Id, pet.BreedId)
                .With(b => b.CategoryId, 202) // non-existing category id
                .Without(b => b.Category)
                .Without(b => b.Pets)
                .Without(b => b.PetPreedProducts)
                .Create();

            // Mock BreedRepository to return the breed
            _unitOfWorkMock.Setup(u => u.PetBreedRepository.GetByID(pet.BreedId))
                .Returns(breed);

            // Mock CategoryRepository to return null
            _unitOfWorkMock.Setup(u => u.PetCategoryRepository.GetByID(breed.CategoryId))
                .Returns((PetCategory)null);

            // Act
            var res = _petService.GetPet(petIdWithWithNotFoundCategory);

            // Assert
            res.Should().NotBeNull();
            res.BreadName.Should().Be(breed.Name); // breed is found, so name should match
            res.CategoryName.Should().Be("Unknown"); // category is null
        }
        [Fact]
        public void GetPetById_PetIsFoundAndCategoryAndBreedIsFound_ShouldReturnPetWithCategoryAndBreed()
        {
            // Arrange
            int petId = 3;

            // Create a mock pet with a valid BreedId
            var pet = _fixture.Build<Pet>()
                .With(p => p.BreedId, 10)
                .Without(p => p.AdminPetMessages)
                .Without(p => p.CustomerAddedPets)
                .Without(p => p.CustomerPetAdoptions)
                .Without(p => p.ShelterAddedPets)
                .Without(p => p.ShelterPetAdoptions)
                .Without(p => p.Breed)
                .Create();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetPetDetails(petId))
                .Returns(pet);

            // Create a mock breed with a CategoryId that is not found 
            var breed = _fixture.Build<PetBreed>()
                .With(b => b.Id, pet.BreedId)
                .With(b => b.CategoryId, 30) //existing category id
                .Without(b => b.Category)
                .Without(b => b.Pets)
                .Without(b => b.PetPreedProducts)
                .Create();

            // Mock BreedRepository to return the breed
            _unitOfWorkMock.Setup(u => u.PetBreedRepository.GetByID(pet.BreedId))
                .Returns(breed);

            var cat = _fixture.Build<PetCategory>()
                .With(c => c.Id, breed.CategoryId)
                .Without(c => c.Breeds).Create();

            // Mock CategoryRepository to category 
            _unitOfWorkMock.Setup(u => u.PetCategoryRepository.GetByID(breed.CategoryId))
                .Returns(cat);

            // Act
            var res = _petService.GetPet(petId);

            // Assert
            res.Should().NotBeNull();
            res.BreadName.Should().Be(breed.Name); // breed is found, so name should match
            res.CategoryName.Should().Be(cat.Name); // category is found, so name should match
        }

        [Fact]
        public async Task UpdatePet_PetNotFound_ShouldReturnZero()
        {
            // Arrange
     
            var dto = _fixture.Create<UpdatedPetDto>();
            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(dto.Id)).Returns((Pet)null);

            // Act
            var result = await _petService.UpdatePet(dto);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public async Task UpdatePet_PetFoundWithoutImage_ShouldUpdateAndReturnSaveChanges()
        {
            // Arrange
            var petId = 1;

            var updatedPetDto = _fixture.Build<UpdatedPetDto>()
                .With(p => p.Id, petId)
                .With(p => p.ImgURL, (IFormFile)null)
                .Create();

            var existingPet = _fixture.Build<Pet>()
                .With(p => p.Id, petId)
                .Without(p => p.AdminPetMessages)
                .Without(p => p.CustomerAddedPets)
                .Without(p => p.CustomerPetAdoptions)
                .Without(p => p.ShelterAddedPets)
                .Without(p => p.ShelterPetAdoptions)
                .Without(p => p.Breed)
                .Create();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns(existingPet);

            _unitOfWorkMock.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = await _petService.UpdatePet(updatedPetDto);

            // Assert
            result.Should().Be(1);

            existingPet.Name.Should().Be(updatedPetDto.Name);
            existingPet.Ownership.Should().Be(updatedPetDto.Ownership);
            existingPet.Status.Should().Be(updatedPetDto.Status);
            existingPet.BreedId.Should().Be(updatedPetDto.BreedId);
            existingPet.Age.Should().Be(updatedPetDto.Age);
            existingPet.Notes.Should().Be(updatedPetDto.Notes);
            existingPet.IsApproved.Should().BeFalse();

            _unitOfWorkMock.Verify(u => u.PetRepository.Update(existingPet), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
            _attachmentServiceMock.Verify(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePet_PetFoundWithImage_ShouldUploadImageAndUpdateAndReturnSaveChanges()
        {
            // Arrange
            var petId = 1;
            var fakeFileName = "uploaded-image.jpg";

            var fakeFile = new Mock<IFormFile>();
            fakeFile.Setup(f => f.FileName).Returns("pet.jpg");

            var updatedPetDto = _fixture.Build<UpdatedPetDto>()
                .With(p => p.Id, petId)
                .With(p => p.ImgURL, fakeFile.Object)
                .Create();

            var existingPet = _fixture.Build<Pet>()
                .With(p => p.Id, petId)
                .Without(p => p.AdminPetMessages)
                .Without(p => p.CustomerAddedPets)
                .Without(p => p.CustomerPetAdoptions)
                .Without(p => p.ShelterAddedPets)
                .Without(p => p.ShelterPetAdoptions)
                .Without(p => p.Breed)
                .Create();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns(existingPet);

            _attachmentServiceMock.Setup(s => s.UploadAsync(fakeFile.Object, "PetImages"))
                .ReturnsAsync(fakeFileName);

            _unitOfWorkMock.Setup(u => u.SaveChanges())
                .Returns(1);

            // Act
            var result = await _petService.UpdatePet(updatedPetDto);

            // Assert
            result.Should().Be(1);
            existingPet.ImgUrl.Should().Be(fakeFileName);
            existingPet.Name.Should().Be(updatedPetDto.Name);
            existingPet.Ownership.Should().Be(updatedPetDto.Ownership);
            existingPet.Status.Should().Be(updatedPetDto.Status);
            existingPet.BreedId.Should().Be(updatedPetDto.BreedId);
            existingPet.Age.Should().Be(updatedPetDto.Age);
            existingPet.Notes.Should().Be(updatedPetDto.Notes);
            existingPet.IsApproved.Should().BeFalse();

            _attachmentServiceMock.Verify(x => x.UploadAsync(fakeFile.Object, "PetImages"), Times.Once);
            _unitOfWorkMock.Verify(u => u.PetRepository.Update(existingPet), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeletePet_PetNotFound_ShouldReturnZero() 
        {
            int petId = 5;

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns((Pet)null);

            var res = _petService.DeletePet(petId);

            res.Should().Be(0);
        }

        [Fact]
        public void DeletePet_PetFound_ShouldReturnOne()
        {
            int petId = 5;

            var PetToBeDeleted = _fixture.Build<Pet>()
                .With(p => p.Id, petId)
                .Without(p => p.AdminPetMessages)
                .Without(p => p.CustomerAddedPets)
                .Without(p => p.CustomerPetAdoptions)
                .Without(p => p.ShelterAddedPets)
                .Without(p => p.ShelterPetAdoptions)
                .Without(p => p.Breed)
                .Create();

            _unitOfWorkMock.Setup(u => u.PetRepository.GetByID(petId))
                .Returns(PetToBeDeleted);

            _unitOfWorkMock.Setup(u => u.SaveChanges())
                .Returns(1);

            var res = _petService.DeletePet(petId);

            res.Should().Be(1);

            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
            _unitOfWorkMock.Verify(u => u.PetRepository.Delete(PetToBeDeleted), Times.Once);
        }




    }
}

