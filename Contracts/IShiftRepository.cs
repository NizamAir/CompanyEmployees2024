﻿using Entities.Models;

namespace Contracts
{
    public interface IShiftRepository
    {
        Task<IEnumerable<Shift>> GetShifts(bool trackChanges);
        Task<IEnumerable<Shift>> GetShiftsByDoctor(string doctorId, bool trackChanges);
        Task<Shift> GetShift(Guid id, bool trackChanges);
        void CreateShift(Shift shift);
        void DeleteShift(Shift shift);
    }
}