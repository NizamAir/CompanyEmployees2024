using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class DoctorRepository : RepositoryBase<Doctor>, IDoctorRepository
    {
        public DoctorRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(d => d.LastName).ToListAsync();

        public async Task<Doctor> GetDoctor(Guid doctorId, bool trackChanges) =>
            await FindByCondition(d => d.Id.Equals(doctorId), trackChanges).SingleOrDefaultAsync();

        public void CreateDoctor(Doctor doctor) => Create(doctor);

        public void DeleteDoctor(Doctor doctor) => Delete(doctor);
    }
}
