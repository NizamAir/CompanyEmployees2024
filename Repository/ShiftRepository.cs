using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ShiftRepository : RepositoryBase<Shift>, IShiftRepository
    {
        public ShiftRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Shift>> GetShifts(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(w => w.ShiftDate.Day)
            .OrderBy(w => w.ShiftDate.Hour)
            .ToListAsync();

        public async Task<IEnumerable<Shift>> GetShiftsByDoctor(string doctorId, bool trackChanges) =>
            await FindByCondition(s => s.DoctorId.Equals(doctorId), trackChanges)
            .OrderBy(w => w.ShiftDate.Date).ToListAsync();

        public void CreateShift(Shift shift) => Create(shift);
        public void DeleteShift(Shift shift) => Delete(shift);

        public async Task<Shift> GetShift(Guid id, bool trackChanges) =>
            await FindByCondition(s => s.Id.Equals(id), trackChanges).SingleOrDefaultAsync();



    }
}
