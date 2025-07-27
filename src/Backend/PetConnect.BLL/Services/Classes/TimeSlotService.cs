using PetConnect.BLL.Services.DTOs.TimeSlotDto;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Models;
using PetConnect.DAL.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Classes
{
    class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TimeSlotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddTimeSlot(AddedTimeSlotDto addedTimeSlot)
        {
            TimeSlot ts = new TimeSlot() 
            { StartTime = addedTimeSlot.StartTime,  EndTime= addedTimeSlot.EndTime,
               DoctorId = addedTimeSlot.DoctorId , IsActive = addedTimeSlot.IsActive,
               MaxCapacity = addedTimeSlot.MaxCapacity,BookedCount = addedTimeSlot.BookedCount
            };

            _unitOfWork.TimeSlotsRepository.Add(ts);
                return _unitOfWork.SaveChanges();
        }

        public int DeleteTimeSlot(int id)
        {
            TimeSlot foundedTimeSlot = _unitOfWork.TimeSlotsRepository.GetByID(id);
            if (foundedTimeSlot != null)
            {
                _unitOfWork.TimeSlotsRepository.Delete(foundedTimeSlot);
                return _unitOfWork.SaveChanges();
            }
            return 0;
        }

        public IEnumerable<DataTimeSlotsDto> GetAllActiveTimeSlots(string doctorId)
        {
            IEnumerable<TimeSlot> activeTimeSlots = _unitOfWork.TimeSlotsRepository.GetAll().Where(e => e.IsActive == true && e.DoctorId== doctorId);
            return activeTimeSlots.Select(ts =>
                new DataTimeSlotsDto
                {
                    BookedCount = ts.BookedCount,
                    EndTime = ts.EndTime,
                    StartTime = ts.StartTime,
                    IsActive = ts.IsActive,
                    MaxCapacity = ts.MaxCapacity
            });
        }

        public IEnumerable<DataTimeSlotsDto> GetAllTimeSlots(string doctorId)
        {
            IEnumerable<TimeSlot> allTimeSlots = _unitOfWork.TimeSlotsRepository.GetAll().Where(e=> e.DoctorId == doctorId);

            return allTimeSlots.Select(ts =>
            new DataTimeSlotsDto
            {
                BookedCount = ts.BookedCount,
                EndTime = ts.EndTime,
                StartTime = ts.StartTime,
                IsActive = ts.IsActive,
                MaxCapacity = ts.MaxCapacity
            });
        }

        public DataTimeSlotsDto? GetTimeSlot(int id)
        {
            TimeSlot ts = _unitOfWork.TimeSlotsRepository.GetByID(id);
            if (ts != null)
                return
                    new DataTimeSlotsDto
                    {
                        BookedCount = ts.BookedCount,
                        EndTime = ts.EndTime,
                        StartTime = ts.StartTime,
                        IsActive = ts.IsActive,
                        MaxCapacity = ts.MaxCapacity
                    };
            return null;
        }
        public async Task<int> UpdateTimeSlot(UpdatedTimeSlotDto updatedTimeSlot)
        {
            TimeSlot ts = _unitOfWork.TimeSlotsRepository.GetByID(updatedTimeSlot.Id);
            if (ts == null)
                return 0;

            ts.DoctorId = updatedTimeSlot.DoctorId;
            ts.StartTime = updatedTimeSlot.StartTime;
            ts.EndTime = updatedTimeSlot.EndTime;
            ts.MaxCapacity = updatedTimeSlot.MaxCapacity;
            ts.BookedCount = updatedTimeSlot.BookedCount;
            ts.IsActive = updatedTimeSlot.IsActive;

            _unitOfWork.TimeSlotsRepository.Update(ts);
            return _unitOfWork.SaveChanges();
        }
    }
}
