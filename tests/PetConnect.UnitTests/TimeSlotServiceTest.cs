
using Moq;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.BLL.Services.Classes;
using PetConnect.DAL.UnitofWork;
using PetConnect.BLL.Services.DTOs.TimeSlotDto;
using PetConnect.DAL.Data.Models;
using FluentAssertions;
using PetConnect.DAL.Data.Enums;

namespace PetConnect.UnitTests
{
    public class TimeSlotServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ITimeSlotService _timeSlotService;

        public TimeSlotServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _timeSlotService = new TimeSlotService(_unitOfWorkMock.Object);
        }


        [Fact]
        public async Task AddTimeSlot_ShouldReturn1_WhenSuccessful()
        {
            // Arrange
            var dto = new AddedTimeSlotDto
            {
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2),
                DoctorId = "doc1",
                IsActive = true,
                MaxCapacity = 5,
                BookedCount = 0
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.Add(It.IsAny<TimeSlot>()));
            _unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(1);

            // Act
            var result = await _timeSlotService.AddTimeSlot(dto);

            // Assert
            result.Should().Be(1);
            _unitOfWorkMock.Verify(u => u.TimeSlotsRepository.Add(It.IsAny<TimeSlot>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteTimeSlot_ShouldReturn1_WhenTimeSlotExists()
        {
            // Arrange
            var timeSlot = new TimeSlot { Id = new Guid() };
            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetByID(1)).Returns(timeSlot);
            _unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(1);

            // Act
            var result = _timeSlotService.DeleteTimeSlot(1);

            // Assert
            result.Should().Be(1);
            _unitOfWorkMock.Verify(u => u.TimeSlotsRepository.Delete(timeSlot), Times.Once);
        }

        [Fact]
        public void GetAllActiveTimeSlots_ShouldReturnActiveSlots_ForDoctor()
        {
            // Arrange
            var doctorId = "doc1";
            var slotId1 = Guid.NewGuid();
            var slotId2 = Guid.NewGuid();
            var slotId3 = Guid.NewGuid();

            var timeSlots = new List<TimeSlot>
            {
                new TimeSlot { Id = slotId1, DoctorId = doctorId, IsActive = true },
                new TimeSlot { Id = slotId2, DoctorId = doctorId, IsActive = false },
                new TimeSlot { Id = slotId3, DoctorId = "doc2", IsActive = true }
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetAll(false)).Returns(timeSlots.AsQueryable());

            // Act
            var result = _timeSlotService.GetAllActiveTimeSlots(doctorId);

            // Assert
            result.Should().ContainSingle().Which.Id.Should().Be(slotId1);
        }

        [Fact]
        public void GetAllTimeSlotsIncludingStatus_ShouldReturnSlotsWithStatus()
        {
            // Arrange
            var doctorId = "doc1";
            var slotId = Guid.NewGuid();
            var timeSlots = new List<TimeSlot>
            {
                new TimeSlot
                {
                    Id = slotId,
                    DoctorId = doctorId,
                    IsActive = true,
                    Appointments = new List<Appointment>
                    {
                        new Appointment { SlotId = slotId, Status = AppointmentStatus.Confirmed }
                    }
                }
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetAllQueryable(false))
                .Returns(timeSlots.AsQueryable());

            // Act
            var result = _timeSlotService.GetAllTimeSlotsIncludingStatus(doctorId);

            // Assert
            result.Should().ContainSingle();
            result.First().Status.Should().Be("Confirmed");
        }

        [Fact]
        public async Task UpdateTimeSlot_ShouldReturn1_WhenTimeSlotExists()
        {
            // Arrange
            var slotId = Guid.NewGuid();
            var timeSlot = new TimeSlot { Id = slotId };
            var dto = new UpdatedTimeSlotDto
            {
                Id = slotId.ToString(),
                DoctorId = "doc1",
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2),
                MaxCapacity = 5,
                BookedCount = 0,
                IsActive = true
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetByID(slotId.ToString())).Returns(timeSlot);
            _unitOfWorkMock.Setup(u => u.SaveChanges()).Returns(1);

            // Act
            var result = await _timeSlotService.UpdateTimeSlot(dto);

            // Assert
            result.Should().Be(1);
            timeSlot.DoctorId.Should().Be(dto.DoctorId);
            timeSlot.StartTime.Should().Be(dto.StartTime);
            timeSlot.EndTime.Should().Be(dto.EndTime);
            timeSlot.MaxCapacity.Should().Be(dto.MaxCapacity);
            timeSlot.BookedCount.Should().Be(dto.BookedCount);
            timeSlot.IsActive.Should().Be(dto.IsActive);
            _unitOfWorkMock.Verify(u => u.TimeSlotsRepository.Update(timeSlot), Times.Once);
        }

        [Fact]
        public async Task IsBookable_ShouldReturnFalse_WhenDatePassed()
        {
            // Arrange
            var dto = new CheckTimeSlotsForCustomerDoctorDTO
            {
                StartTime = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = await _timeSlotService.IsBookable(dto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ChangeTimeSlotState_ShouldToggleActiveState()
        {
            // Arrange
            var slotId = Guid.NewGuid();
            var timeSlot = new TimeSlot { Id = slotId, IsActive = true };
            var dto = new ChangeActiveTimeSlotStateDTO { Id = slotId };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetAll(false))
                .Returns(new List<TimeSlot> { timeSlot }.AsQueryable());
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _timeSlotService.ChangeTimeSlotState(dto);

            // Assert
            result.Should().Be(1);
            timeSlot.IsActive.Should().BeFalse();
        }

        [Fact]
        public void GetALlTimeSlots_ShouldReturnAllTheTimeSlots_ForDoctor()
        {
            //Arrange
            var doctorId = "doc";
            var today = DateTime.Today;
            var slots = new List<TimeSlot>
            {
                new TimeSlot
                {
                    Id = Guid.NewGuid(),
                    DoctorId = doctorId,
                    StartTime = today.AddHours(9),
                    EndTime = today.AddHours(10),
                    MaxCapacity = 5,
                    BookedCount = 2,
                    IsActive = true
                },
                new TimeSlot
                {
                    Id = Guid.NewGuid(),
                    DoctorId = doctorId,
                    StartTime = today.AddDays(-1).AddHours(9), // Past timeslot
                    EndTime = today.AddDays(-1).AddHours(10),
                    MaxCapacity = 3,
                    BookedCount = 1,
                    IsActive = true
                },
                new TimeSlot
                {
                    Id = Guid.NewGuid(),
                    DoctorId = doctorId,
                    StartTime = today.AddHours(11),
                    EndTime = today.AddHours(12),
                    MaxCapacity = 4,
                    BookedCount = 0,
                    IsActive = false // Manually inactive
                }
            };

            _unitOfWorkMock.Setup(u => u.TimeSlotsRepository.GetAll(false)).Returns(slots.AsQueryable());

            //Act
            var result = _timeSlotService.GetAllTimeSlots(doctorId);

            //Assert
            result.Should().HaveCount(3);

        }

    }
}
