using Entities.Models;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.DoctorDTOs;

namespace Service.Contracts
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctors(bool trackChanges);
        Task<DoctorDto> GetDoctor(Guid doctorId, bool trackChanges);
        Task CreateDoctor(DoctorForInitialDto doctorInitialDto);
        Task DeleteDoctor(Guid doctorId, bool trackChanges);
        Task UpdateDoctor(Guid doctorId, DoctorForUpdateDto doctorForUpdate, bool trackChanges);
    }
}
