
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTO.Doctor;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.Data.Repositories.Interfaces;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.UnitTests
{
    public class DoctorServiceTest
    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DoctorService _doctorService;
        private readonly IFixture _fixture;

        public DoctorServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _doctorService = new DoctorService(_unitOfWorkMock.Object);
            _fixture = new Fixture();

            // Fix circular reference issue globally for this test class
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            //configure auto fixture to make a mock object of interface IFormFile
            _fixture.Register<IFormFile>(() =>
            {
                var fileMock = new Mock<IFormFile>();
                fileMock.Setup(f => f.FileName).Returns("test.jpg");
                fileMock.Setup(f => f.Length).Returns(100);
                fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
                return fileMock.Object;
            });
        }

        [Fact]
        public void GetAll_ShouldReturnOnlyValidDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
{
                // VALID Doctor 1
                new Doctor
                {
                    Id = "doc1",
                    FName = "Sarah",
                    LName = "Youssef",
                    Email = "sarah@example.com",
                    UserName = "sarah.y",
                    PhoneNumber = "0100000001",
                    IsApproved = true,
                    IsDeleted = false,
                    ImgUrl = null, // To test default image behavior
                    CertificateUrl = "certificates/sarah.pdf",
                    PetSpecialty = PetSpecialty.Dog,
                    Gender = Gender.Female,
                    PricePerHour = 200,
                    Address = new Address
                    {
                        Street = "Tahrir St",
                        City = "Cairo"
                    }
                },

                //  VALID Doctor 2
                new Doctor
                {
                    Id = "doc2",
                    FName = "Ali",
                    LName = "Kamel",
                    Email = "ali@example.com",
                    UserName = "ali.k",
                    PhoneNumber = "0100000002",
                    IsApproved = true,
                    IsDeleted = false,
                    ImgUrl = "images/ali.jpg",
                    CertificateUrl = "certificates/ali.pdf",
                    PetSpecialty = PetSpecialty.Cat,
                    Gender = Gender.Male,
                    PricePerHour = 150,
                    Address = new Address
                    {
                        Street = "Nile Ave",
                        City = "Giza"
                    }
                },

                //  INVALID Doctor 3 - Not approved
                new Doctor
                {
                    Id = "doc3",
                    FName = "Hana",
                    LName = "Tarek",
                    Email = "hana@example.com",
                    UserName = "hana.t",
                    PhoneNumber = "0100000003",
                    IsApproved = false,
                    IsDeleted = false,
                    ImgUrl = "images/hana.jpg",
                    CertificateUrl = "certificates/hana.pdf",
                    PetSpecialty = PetSpecialty.Bird,
                    Gender = Gender.Female,
                    PricePerHour = 180,
                    Address = new Address
                    {
                        Street = "Airport Rd",
                        City = "Alexandria"
                    }
                },

                //  INVALID Doctor 4 - Deleted
                new Doctor
                {
                    Id = "doc4",
                    FName = "Mohamed",
                    LName = "Fathy",
                    Email = "mohamed@example.com",
                    UserName = "mohamed.f",
                    PhoneNumber = "0100000004",
                    IsApproved = true,
                    IsDeleted = true,
                    ImgUrl = "images/mohamed.jpg",
                    CertificateUrl = "certificates/mohamed.pdf",
                    PetSpecialty = PetSpecialty.Bird,
                    Gender = Gender.Male,
                    PricePerHour = 220,
                    Address = new Address
                    {
                        Street = "Salah Salem",
                        City = "Nasr City"
                    }
                }
            };


            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetAll(false))
                .Returns(doctors.AsQueryable());

            // Act
            var result = _doctorService.GetAll().ToList();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].ImgUrl.Should().Be(@"/assets/img/default-doctor.jpg");// test if image is null set it to default image 
        }

        [Fact]
        public void GetByID_ShouldReturnDoctorDetailsDTO_WhenDoctorExists()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = "1",
                FName = "ahmed",
                LName = "khalid",
                IsApproved = true,
                IsDeleted = false,
                ImgUrl = null,
                PetSpecialty = PetSpecialty.Cat,
                Gender = Gender.Female,
                PricePerHour = 100,
                Address = new Address { Street = "Main St", City = "Cairo" }
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("1")).Returns(doctor);

            // Act
            var result = _doctorService.GetByID("1");

            // Assert
            result.Should().NotBeNull();
            result.ImgUrl.Should().Be(@"/assets/img/default-doctor.jpg");// test if image is null set it to default image 
            result!.FName.Should().Be("ahmed");
        }

        [Fact]
        public void GetByID_ShouldReturnNull_WhenDoctorNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("999")).Returns((Doctor?)null);

            // Act
            var result = _doctorService.GetByID("999");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Add_ShouldCallRepositoryAddAndSaveChanges()
        {
            // Arrange
            var doctor = new Doctor
            {
                PricePerHour = 100,
                CertificateUrl = "test.pdf",
                PetSpecialty = PetSpecialty.Cat
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.Add(doctor));
            // Act
            _doctorService.Add(doctor);

            // Assert
            _unitOfWorkMock.Verify(u => u.DoctorRepository.Add(doctor), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldUpdateDoctorDetails_WhenDoctorExists()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id="123",
                PricePerHour = 100,
                CertificateUrl = "test.pdf",
                PetSpecialty = PetSpecialty.Cat
            };

            var dto = new DoctorDetailsDTO
            {
                Id = doctor.Id,
                FName = "UpdatedFName",
                LName = "UpdatedLName",
                ImgUrl = "img.jpg",
                PricePerHour = 250,
                CertificateUrl = "certs/updated.pdf",
                PetSpecialty = PetSpecialty.Cat.ToString(),
                Gender = Gender.Female.ToString(),
                Street = "Updated St",
                City = "Updated City"
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID(doctor.Id)).Returns(doctor);

            // Act
            _doctorService.Update(dto);

            // Assert
            doctor.FName.Should().Be(dto.FName);
            doctor.LName.Should().Be(dto.LName);
            doctor.ImgUrl.Should().Be(dto.ImgUrl);
            doctor.PricePerHour.Should().Be(dto.PricePerHour);
            doctor.CertificateUrl.Should().Be(dto.CertificateUrl);
            doctor.PetSpecialty.Should().Be(PetSpecialty.Cat);
            doctor.Gender.Should().Be(Gender.Female);
            doctor.Address.Street.Should().Be(dto.Street);
            doctor.Address.City.Should().Be(dto.City);

            _unitOfWorkMock.Verify(u => u.DoctorRepository.Update(doctor), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var dto = _fixture.Build<DoctorDetailsDTO>()
                .With(d => d.Id, "notfound")
                .Create();

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("notfound"))
                .Returns((Doctor?)null);

            // Act
            var act = () => _doctorService.Update(dto);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public void Delete_ShouldCallDeleteAndSaveChanges_WhenDoctorExists()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id =  "delete123",
                PricePerHour = 100,
                CertificateUrl = "test.pdf",
                PetSpecialty = PetSpecialty.Cat
            };

            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("delete123")).Returns(doctor);

            // Act
            _doctorService.Delete("delete123");

            // Assert
            _unitOfWorkMock.Verify(u => u.DoctorRepository.Delete(doctor), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ShouldDoNothing_WhenDoctorDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.DoctorRepository.GetByID("missing")).Returns((Doctor?)null);

            // Act
            _doctorService.Delete("missing");

            // Assert
            _unitOfWorkMock.Verify(u => u.DoctorRepository.Delete(It.IsAny<Doctor>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Never);
        }
    }
}
