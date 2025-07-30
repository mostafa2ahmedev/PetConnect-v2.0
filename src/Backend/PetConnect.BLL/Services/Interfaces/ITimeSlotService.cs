using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.TimeSlotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    public interface ITimeSlotService
    {
        Task<int> AddTimeSlot(AddedTimeSlotDto addedTimeSlot);
        Task<int> UpdateTimeSlot(UpdatedTimeSlotDto UpdatedTimeSlot);
        Task<int> ChangeTimeSlotState(ChangeActiveTimeSlotStateDTO timeSlot);

        int DeleteTimeSlot(int id);

        IEnumerable<DataTimeSlotsDto> GetAllTimeSlots(string doctorId);
        IEnumerable<TimeSlotsViewDTOcs> GetAllTimeSlotsIncludingStatus(string doctorId);
        DataTimeSlotsDto? GetTimeSlot(int id);
        IEnumerable<DataTimeSlotsDto> GetAllActiveTimeSlots(string doctorId);
        Task<bool> IsBookable(CheckTimeSlotsForCustomerDoctorDTO timeSlot);

    }
}
