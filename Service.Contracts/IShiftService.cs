using Entities.Models;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.ShiftDTOs;

namespace Service.Contracts
{
    public interface IShiftService
    {
        Task<IEnumerable<ShiftDto>> GetAllShifts(bool trackChanges);
        Task<IEnumerable<ShiftDto>> GetShiftsByDoctor(string doctorId, bool trackChanges);
        Task CreateShiftForDoctor(string userId, DateOnly date);
        Task UpdateShiftAssistant(string userId, string doctorId, DateOnly date, bool trackChanges);
        Task UpdateShiftClient(string userId, string doctorId, DateTime date, bool trackChanges);
        Task<ShiftDto> GetShift(Guid id, bool trackChanges);
    }
}
