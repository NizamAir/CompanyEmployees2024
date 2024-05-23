using Entities.Models;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.ShiftDTOs;

namespace Service.Contracts
{
    public interface IShiftService
    {
        Task<IEnumerable<ShiftDto>> GetAllShifts(bool trackChanges);
        Task<IEnumerable<ShiftDto>> GetShiftsByDoctor(string doctorId, bool trackChanges);
        Task<IEnumerable<ShiftDto>> GetShiftsByAssistant(string assistantId, bool trackChanges);
        Task CreateShiftForDoctor(string userId, DateOnly date);
        Task UpdateShiftAssistant(string userId, string doctorId, DateOnly date, bool trackChanges);
        Task<IEnumerable<ShiftDoctorNameDto>> GetDoctorsForAssistant();
        Task<List<string>> GetTimesForDate(ShiftTimeForDateDto timeForDateDto, bool trackChanges);
        Task UpdateShiftClient(string userId, string doctorId, Guid productId, DateTime date, bool trackChanges);
        Task<ShiftDto> GetShift(Guid id, bool trackChanges);
    }
}
