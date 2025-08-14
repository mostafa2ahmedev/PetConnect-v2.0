using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PetConnect.BLL.Services.Classes;
using PetConnect.BLL.Services.DTOs.AppointmentDto;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Enums;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;

namespace PetConnect.UnitTests
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificationService> _notificationService;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _notificationService = new Mock<INotificationService>();
            _appointmentService = new AppointmentService(_unitOfWorkMock.Object , _notificationService.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { Id = Guid.NewGuid(), Status = AppointmentStatus.Pending },
                new Appointment { Id = Guid.NewGuid(), Status = AppointmentStatus.Confirmed }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAll(false))
                .Returns(appointments.AsQueryable());

            // Act
            var result = await _appointmentService.GetAllAppointmentsAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetByGuidAsync(appointmentId))
                .ReturnsAsync((Appointment)null);

            // Act
            var result = await _appointmentService.GetAppointmentByIdAsync(appointmentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAppointmentAsync_ShouldReturnAppointmentViewDTO()
        {
            // Arrange
            var dto = new AppointmentCreateDTO
            {
                DoctorId = "doc1",
                CustomerId = "cust1",
                SlotId = Guid.NewGuid(),
                PetId = 1
            };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.Add(It.IsAny<Appointment>()));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _appointmentService.AddAppointmentAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(AppointmentStatus.Pending.ToString());
        }

        [Fact]
        public async Task CancelAppointmentAsync_ShouldReturnFalse_WhenAlreadyCancelled()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment { Id = appointmentId, Status = AppointmentStatus.Cancelled };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetByGuidAsync(appointmentId))
                .ReturnsAsync(appointment);

            // Act
            var result = await _appointmentService.CancelAppointmentAsync(appointmentId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_ShouldReturnFalse_WhenSlotIsFull()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment
            {
                Id = appointmentId,
                Status = AppointmentStatus.Pending,
                TimeSlot = new TimeSlot
                {
                    BookedCount = 5,  // Equal to MaxCapacity (default is 5)
                    MaxCapacity = 5   // So IsFull will be true
                }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAllQueryable(false))
                .Returns(new List<Appointment> { appointment }.AsQueryable());

            // Act
            var result = await _appointmentService.ConfirmAppointmentAsync(appointmentId);

            // Assert
            result.Should().BeFalse();
            appointment.TimeSlot.IsFull.Should().BeTrue(); // Verify the slot is actually full
        }

        [Fact]
        public async Task CompleteAppointmentAsync_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment { Id = appointmentId, Status = AppointmentStatus.Confirmed };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetByGuidAsync(appointmentId))
                .ReturnsAsync(appointment);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _appointmentService.CompleteAppointmentAsync(appointmentId);

            // Assert
            result.Should().BeTrue();
            appointment.Status.Should().Be(AppointmentStatus.Completed);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_ShouldReturnFalse_WhenAppointmentIsNotFound()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetByGuidAsync(appointmentId))
                .ReturnsAsync((Appointment)null);

            // Act
            var result = await _appointmentService.CompleteAppointmentAsync(appointmentId);

            // Assert
            result.Should().BeFalse(); //Flent Assertion 
        }

        [Fact]
        public void GetAppointmentsForDoctorProfile_ShouldReturnDoctorAppointments()
        {
            // Arrange
            var doctorId = "doc1";
            var doctor = new Doctor { FName = "Dr.", LName = "Smith" };
            var customer = new Customer { FName = "John", LName = "Doe", ImgUrl = "customer.jpg", PhoneNumber = "1234567890" };
            var pet = new Pet { Name = "Fluffy", ImgUrl = "pet.jpg" };
            var timeSlot = new TimeSlot
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                MaxCapacity = 5,
                BookedCount = 1
            };

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                DoctorId = doctorId,
                Doctor = doctor,
                Customer = customer,
                CustomerId = customer.Id,
                Pet = pet,
                PetId = pet.Id,
                TimeSlot = timeSlot,
                SlotId = timeSlot.Id,
                Status = AppointmentStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAllQueryable(false))
                .Returns(new List<Appointment> { appointment }.AsQueryable());

            // Act
            var result = _appointmentService.GetAppointmentsForDoctorProfile(doctorId);

            // Assert
            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    CustomerName = "John Doe",
                    PetName = "Fluffy",
                    DoctorName = "Dr. Smith",
                    Status = "Confirmed",
                    PetImg = "pet.jpg",
                    CustomerImg = "customer.jpg",
                    CustomerPhone = "1234567890",
                    MaxCapacity = 5,
                    BookedCount = 1
                }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetCurrentTimeSlotsAvailableDocCustomer_ShouldReturnAvailableSlots()
        {
            // Arrange
            var doctorId = "doc1";
            var customerId = "cust1";
            var timeSlot = new TimeSlot
            {
                Id = Guid.NewGuid(),
                DoctorId = doctorId,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2),
                MaxCapacity = 5,
                BookedCount = 1,
                Appointments = new List<Appointment>()
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetAllQueryable(false))
                .Returns(new List<TimeSlot> { timeSlot }.AsQueryable());

            // Act
            var result = _appointmentService.GetCurrentTimeSlotsAvailableDocCustomer(doctorId, customerId);

            // Assert
            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    SlotId = timeSlot.Id,
                    SlotStartTime = timeSlot.StartTime,
                    SlotEndTime = timeSlot.EndTime,
                    MaxCapacity = 5,
                    BookedCount = 1,
                    Status = "Available",
                    CustomerId = customerId
                }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetCurrentTimeSlotsAvailableDocCustomer_ShouldReturnBookedStatus_WhenCustomerHasAppointment()
        {
            // Arrange
            var doctorId = "doc1";
            var customerId = "cust1";
            var timeSlot = new TimeSlot
            {
                Id = Guid.NewGuid(),
                DoctorId = doctorId,
                Appointments = new List<Appointment>
        {
            new Appointment
            {
                CustomerId = customerId,
                Status = AppointmentStatus.Confirmed,
                Customer = new Customer { FName = "John", LName = "Doe" }
            }
        }
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetAllQueryable(false))
                .Returns(new List<TimeSlot> { timeSlot }.AsQueryable());

            // Act
            var result = _appointmentService.GetCurrentTimeSlotsAvailableDocCustomer(doctorId, customerId);

            // Assert
            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    Status = "Confirmed",
                    CustomerName = "John Doe"
                }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetAppointmentsByDoctorAsync_ShouldReturnAppointments_ForSpecifiedDoctor()
        {
            // Arrange
            var doctorId = "doc1";
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    DoctorId = doctorId,
                    Status = AppointmentStatus.Confirmed,
                    CreatedAt = DateTime.UtcNow
                },
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    DoctorId = "doc2", // Different doctor
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAll(false))
                .Returns(appointments.AsQueryable());

            // Act
            var result = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId);

            // Assert
            result.Should().HaveCount(1);
            result.First().DoctorId.Should().Be(doctorId);
            result.First().Status.Should().Be("Confirmed");
        }

        [Fact]
        public async Task GetAppointmentsByCustomerAsync_ShouldReturnAppointments_ForSpecifiedCustomer()
        {
            // Arrange
            var customerId = "cust1";
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    Status = AppointmentStatus.Completed,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Appointment
                {
                    Id = Guid.NewGuid(),
                    CustomerId = "cust2", // Different customer
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAll(false))
                .Returns(appointments.AsQueryable());

            // Act
            var result = await _appointmentService.GetAppointmentsByCustomerAsync(customerId);

            // Assert
            result.Should().HaveCount(1);
            result.First().CustomerId.Should().Be(customerId);
            result.First().Status.Should().Be("Completed");
        }

        [Fact]
        public async Task GetAppointmentsByDoctorAsync_ShouldReturnEmpty_WhenNoAppointmentsExist()
        {
            // Arrange
            var doctorId = "doc1";
            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAll(false))
                .Returns(new List<Appointment>().AsQueryable());

            // Act
            var result = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAppointmentsByCustomerAsync_ShouldReturnEmpty_WhenNoAppointmentsExist()
        {
            // Arrange
            var customerId = "cust1";
            _unitOfWorkMock.Setup(u => u.AppointmentsRepository.GetAll(false))
                .Returns(new List<Appointment>().AsQueryable());

            // Act
            var result = await _appointmentService.GetAppointmentsByCustomerAsync(customerId);

            // Assert
            result.Should().BeEmpty();
        }




    }
}
