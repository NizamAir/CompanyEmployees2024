﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects.ShiftDTOs;
using System.ComponentModel.Design;

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

        /* Common methods*/

        public async Task<IEnumerable<ShiftDoctorNameDto>> GetDoctorsForAssistant()
        {
            var doctorsEntity = await _userManager.GetUsersInRoleAsync("Doctor");
            var doctors = new List<ShiftDoctorNameDto>();

            foreach (var doctor in doctorsEntity)
            {
                doctors.Add(new ShiftDoctorNameDto() { DoctorId = doctor.Id, FirstName = doctor.FirstName, LastName = doctor.LastName });
            }
            return doctors;
        }

        public async Task<IEnumerable<ShiftDto>> GetShiftsByDoctor(string doctorId, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(doctorId, trackChanges);
            foreach (var shift in shifts)
            {
                shift.DoctorUser = await _userManager.FindByIdAsync(doctorId);
                if (shift.AssistentId != null)
                    shift.AssistentUser = await _userManager.FindByIdAsync(shift.AssistentId);
                if (shift.ClientId != null)
                    shift.ClientUser = await _userManager.FindByIdAsync(shift.ClientId);
                if (shift.ProductId != null)
                    shift.Product = await _repository.Product.GetProduct(shift.ProductId.Value, trackChanges);
            }
            var shiftsDto = _mapper.Map<IEnumerable<ShiftDto>>(shifts);

            return shiftsDto;
        }

        /* Doctor methods*/
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

        public async Task DeleteShiftDoctor(string userId, string date, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(userId, trackChanges);

            foreach (var shift in shifts)
            {
                if (DateOnly.FromDateTime(shift.ShiftDate) == DateOnly.Parse(date))
                {
                    _repository.Shift.DeleteShift(shift);
                    await _repository.SaveAsync();
                }
            }
        }

        /* Assistant methods*/
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

        public async Task<IEnumerable<ShiftDto>> GetShiftsByAssistant(string assistantId, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByAssistant(assistantId, trackChanges);
            foreach (var shift in shifts)
            {
                shift.AssistentUser = await _userManager.FindByIdAsync(assistantId);
                if (shift.DoctorId != null)
                    shift.DoctorUser = await _userManager.FindByIdAsync(shift.DoctorId);
                if (shift.ClientId != null)
                    shift.ClientUser = await _userManager.FindByIdAsync(shift.ClientId);
            }
            var shiftsDto = _mapper.Map<IEnumerable<ShiftDto>>(shifts);
            return shiftsDto;
        }

        public async Task DeleteShiftAssistant(string userId, string date, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByAssistant(userId, trackChanges);
            
            foreach(var shift in shifts)
            {
                if(DateOnly.FromDateTime(shift.ShiftDate) == DateOnly.Parse(date))
                {
                    var shiftUpdate = new ShiftForUpdateDto(shift.ShiftDate, shift.ProductId, shift.DoctorId, shift.ClientId, null);
                    _mapper.Map(shiftUpdate, shift);
                    await _repository.SaveAsync();
                }
            }
        }

        /* Client methods*/

        public async Task<IEnumerable<ShiftDto>> GetShiftsByClient(string clientId, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByClient(clientId, trackChanges);
            foreach (var shift in shifts)
            {
                shift.ClientUser = await _userManager.FindByIdAsync(clientId);
                if (shift.AssistentId != null)
                    shift.AssistentUser = await _userManager.FindByIdAsync(shift.AssistentId);
                if (shift.DoctorId != null)
                    shift.DoctorUser = await _userManager.FindByIdAsync(shift.DoctorId);
                if (shift.ProductId != null)
                    shift.Product = await _repository.Product.GetProduct(shift.ProductId.Value, trackChanges);
            }
            var shiftsDto = _mapper.Map<IEnumerable<ShiftDto>>(shifts);

            return shiftsDto;
        }
        public async Task UpdateShiftClient(string userId, string doctorId, Guid productId, DateTime date, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(doctorId, trackChanges);
            var shift = shifts.FirstOrDefault(s => s.ShiftDate.Equals(date));
            var shiftUpdate = new ShiftForUpdateDto(shift.ShiftDate, productId, shift.DoctorId, userId, shift.AssistentId);
            _mapper.Map(shiftUpdate, shift);
            await _repository.SaveAsync();
        }

        public async Task<List<string>> GetTimesForDate(ShiftTimeForDateDto timeForDateDto, bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShiftsByDoctor(timeForDateDto.DoctorId, trackChanges);

            var resShifts = shifts.Where(s => DateOnly.FromDateTime(s.ShiftDate).Equals(DateOnly.FromDateTime(DateTime.Parse(timeForDateDto.Date)))).ToList();
            var times = new List<string>();
            foreach (var shift in resShifts)
            {
                if (shift.ClientId != null || shift.ShiftDate < DateTime.Now)
                    continue;
                else
                    times.Add(TimeOnly.FromDateTime(shift.ShiftDate).ToString());
            }
            return times;
        }
        public async Task DeleteShiftClient(Guid shiftId, bool trackChanges)
        {
            var shift = await _repository.Shift.GetShift(shiftId, trackChanges);
            if (shift is null)
                throw new CompanyNotFoundException(shiftId);

            var shiftUpdate = new ShiftForUpdateDto(shift.ShiftDate, null, shift.DoctorId, null, shift.AssistentId);
            _mapper.Map(shiftUpdate, shift);
            await _repository.SaveAsync();
        }
        /* Not in use methods*/

        public async Task<IEnumerable<ShiftDto>> GetAllShifts(bool trackChanges)
        {
            var shifts = await _repository.Shift.GetShifts(trackChanges);

            foreach (var shift in shifts)
            {
                shift.DoctorUser = await _userManager.FindByIdAsync(shift.DoctorId);
                if (shift.AssistentId != null)
                    shift.AssistentUser = await _userManager.FindByIdAsync(shift.AssistentId);
                if (shift.ProductId != null)
                    shift.Product = await _repository.Product.GetProduct(shift.ProductId.Value, trackChanges);
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
