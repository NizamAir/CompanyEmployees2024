using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects.ShiftDTOs;

namespace Service
{
    public class ShiftService : IShiftService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ShiftService(IRepositoryManager repository, IMapper mapper, UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task CreateShiftForDoctor(string userId, DateOnly date)
        {
            for (var i = 0; i < 8; i++)
            {
                var shiftEntity = new Shift();
                var dateTemp = date.ToDateTime(TimeOnly.Parse($"{10 + i}:00"));
                shiftEntity.ShiftDate = dateTemp;
                shiftEntity.DoctorId = userId;
                _repository.Shift.CreateShift(shiftEntity);
            }
            await _repository.SaveAsync();
        }
        public async Task UpdateShiftAssistant(string userId, string doctorId, DateOnly date, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(doctorId, trackChanges);
            foreach (var shift in shifts)
            {
                var shiftEntity = await _repository.Shift.GetShift(shift.Id, trackChanges);
                if (DateOnly.FromDateTime(shiftEntity.ShiftDate).Equals(date))
                {
                    var shiftUpdate = new ShiftForUpdateDto(shiftEntity.ShiftDate, shiftEntity.ProductId, shiftEntity.DoctorId, shiftEntity.ClientId, userId);
                    _mapper.Map(shiftUpdate, shiftEntity);
                }
            }
            await _repository.SaveAsync();
        }
        public async Task UpdateShiftClient(string userId, string doctorId, DateTime date, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(doctorId, trackChanges);
            var shift = shifts.FirstOrDefault(s => s.ShiftDate.Equals(date));
            var shiftUpdate = new ShiftForUpdateDto(shift.ShiftDate, shift.ProductId, shift.DoctorId, userId, shift.AssistentId);
            _mapper.Map(shiftUpdate, shift);
            await _repository.SaveAsync();
        }



        public async Task<IEnumerable<ShiftDto>> GetAllShifts(bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShifts(trackChanges);

            foreach (var shift in shifts)
            {
                shift.DoctorUser = await _userManager.FindByIdAsync(shift.DoctorId);
                if (shift.AssistentId != null)
                    shift.AssistentUser = await _userManager.FindByIdAsync(shift.AssistentId);
            }

            var shiftsDto = _mapper.Map<IEnumerable<ShiftDto>>(shifts);

            return shiftsDto;
        }

        public async Task<IEnumerable<ShiftDto>> GetShiftsByDoctor(string doctorId, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(doctorId, trackChanges);
            foreach (var shift in shifts)
            {
                shift.DoctorUser = await _userManager.FindByIdAsync(doctorId);
                if (shift.AssistentId != null)
                    shift.AssistentUser = await _userManager.FindByIdAsync(shift.AssistentId);
            }
            var shiftsDto = _mapper.Map<IEnumerable<ShiftDto>>(shifts);

            return shiftsDto;
        }

        public async Task<ShiftDto> GetShift(Guid id, bool trackChanges)
        {

            var shiftDb = await _repository.Shift.GetShift(id, trackChanges);

            if (shiftDb is null)
                throw new EmployeeNotFoundException(id);

            var shift = _mapper.Map<ShiftDto>(shiftDb);

            return shift;
        }


    }
}
