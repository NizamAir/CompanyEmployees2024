using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared.DataTransferObjects.DoctorDTOs;

namespace Service
{
    public sealed class DoctorService : IDoctorService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DoctorService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctors(bool trackChanges)
        {
            var doctors = await _repository.Doctor.GetAllDoctors(trackChanges);

            var doctorsDto = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

            return doctorsDto;
        }

        public async Task<DoctorDto> GetDoctor(Guid doctorId, bool trackChanges)
        {
            var doctor = await _repository.Doctor.GetDoctor(doctorId, trackChanges);
            if (doctor is null)
                throw new CompanyNotFoundException(doctorId);

            var doctorDto = _mapper.Map<DoctorDto>(doctor);

            return doctorDto;
        }

        public async Task CreateDoctor(DoctorForInitialDto doctorInitialDto)
        {
            var doctorEntity = _mapper.Map<Doctor>(doctorInitialDto);

            _repository.Doctor.CreateDoctor(doctorEntity);
            await _repository.SaveAsync();

        }

        public async Task UpdateDoctor(Guid doctorId, DoctorForUpdateDto doctorForUpdate, bool trackChanges)
        {
            var doctorEntity = await _repository.Doctor.GetDoctor(doctorId, trackChanges);
            if (doctorEntity is null)
                throw new CompanyNotFoundException(doctorId);
            if(doctorEntity.ImageName is null && doctorForUpdate.ImageFile != null)
            {
                doctorForUpdate.ImageName = await SaveImage(doctorForUpdate.ImageFile);
            }
            else if (doctorEntity.ImageName != null && doctorForUpdate.ImageFile != null)
            {
                DeleteImage(doctorEntity.ImageName);
                doctorForUpdate.ImageName = await SaveImage(doctorForUpdate.ImageFile);
            }
            else
            {
                doctorForUpdate.ImageName = doctorEntity.ImageName;
            }
            

            _mapper.Map(doctorForUpdate, doctorEntity);
            await _repository.SaveAsync();
        }

        public async Task DeleteDoctor(Guid doctorId, bool trackChanges)
        {
            var doctor = await _repository.Doctor.GetDoctor(doctorId, trackChanges);
            if (doctor is null)
                throw new CompanyNotFoundException(doctorId);
            DeleteImage(doctor.ImageName);
            _repository.Doctor.DeleteDoctor(doctor);
            await _repository.SaveAsync();
        }


        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine("C:\\Users\\Мой компьютер\\source\\repos\\CompanyEmployees2024\\CompanyEmployees2024", "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine("C:\\Users\\Мой компьютер\\source\\repos\\CompanyEmployees2024\\CompanyEmployees2024", "Images", imageName);
            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }
    }
}
