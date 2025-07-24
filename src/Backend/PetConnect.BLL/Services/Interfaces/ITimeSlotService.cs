using PetConnect.BLL.Services.DTO.PetDto;
using PetConnect.BLL.Services.DTOs.TimeSlotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.BLL.Services.Interfaces
{
    interface ITimeSlotService
    {
        Task<int> AddTimeSlot(AddedTimeSlotDto addedTimeSlot);
        Task<int> UpdateTimeSlot(UpdatedTimeSlotDto UpdatedTimeSlot);
        int DeleteTimeSlot(int id);

        IEnumerable<DataTimeSlotsDto> GetAllTimeSlots();
        DataTimeSlotsDto? GetTimeSlot(int id);
        IEnumerable<DataTimeSlotsDto> GetAllActiveTimeSlots();

    }
}
