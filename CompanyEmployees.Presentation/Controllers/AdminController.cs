using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DataTransferObjects.DoctorDTOs;
using Shared.DataTransferObjects.StatisticsDTOs;
using Shared.DataTransferObjects.UserDTOs;
using System.Data;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/userslist")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(IServiceManager service, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _service = service;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
            .Select(u => new { User = u, Roles = new List<string>() })
            .ToListAsync();

            //Fetch all the Roles
            var roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            foreach (var roleName in roleNames)
            {
                //For each role, fetch the users
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

                //Populate the roles for each user in memory
                var toUpdate = users.Where(u => usersInRole.Any(ur => ur.Id == u.User.Id));
                foreach (var user in toUpdate)
                {
                    user.Roles.Add(roleName);
                }
            }
            return Ok(users);
        }

        [HttpGet("doctorslist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var res = new List<DoctorListForAdminDto>();
            foreach (var doctor in doctors)
            {
                res.Add(new DoctorListForAdminDto { Id = doctor.Id, Name = doctor.FirstName + " " + doctor.LastName });
            }
            return Ok(res);

        }
        [HttpGet("rating")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorStat()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var resToRet = new List<DoctorStatDto>();
            foreach (var doctor in doctors)
            {
                var reviews = await _service.ReviewService.GetReviewsForDoctorStat(doctor.Id, trackChanges: false);
                var data = new List<DoctorStatDtoCoordinats>
                {
                    new DoctorStatDtoCoordinats { X = "04.05.2024", Y = 0 }
                };

                double sum = 0;
                int count = 0;
                foreach (var review in reviews)
                {
                    sum += review.StarsCount;
                    count += 1;
                    data.Add(new DoctorStatDtoCoordinats { X = review.CreationDate, Y = (sum / count)*(-1) });
                }

                var res = new DoctorStatDto
                {
                    Id = doctor.FirstName + " " + doctor.LastName,
                    Data = data
                };
                resToRet.Add(res);
            }

            return Ok(resToRet);

        }

        /*[HttpGet("rating/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorStat(string id)
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var resToRet = await
            var reviews = await _service.ReviewService.GetReviewsForDoctorStat(id, trackChanges: false);
            var data = new List<DoctorStatDtoCoordinats>
            {
                new DoctorStatDtoCoordinats { X = "04.05.2024", Y = 0 }
            };

            double sum = 0;
            int count = 0;
            foreach (var review in reviews)
            {
                sum += review.StarsCount;
                count += 1;
                data.Add(new DoctorStatDtoCoordinats { X = review.CreationDate, Y = sum / count });
            }

            var res = new DoctorStatDto
            {
                Id = doctor.FirstName + " " + doctor.LastName,
                Color = "hsl(103, 70%, 50%)",
                Data = data
            };
            return Ok(res);

        }*/

        [HttpGet("doctors")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctorsStatistics()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var res = new List<DoctorShiftStatisticDto>();
            foreach (var doctor in doctors)
            {
                var shifts = await _service.ShiftService.GetShiftsByDoctor(doctor.Id, trackChanges: false);
                var dates = new List<string>();
                foreach (var shift in shifts)
                {
                    if (dates.Contains(shift.ShiftDate))
                        continue;
                    else
                        dates.Add(shift.ShiftDate);
                }
                res.Add(new DoctorShiftStatisticDto { Id = doctor.FirstName + " " + doctor.LastName, Value = dates.Count });

            }
            return Ok(res);

        }

        [HttpGet("products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProductsStatistics()
        {
            var products = await _service.ProductService.GetAllProducts(trackChanges: false);
            var shifts = await _service.ShiftService.GetAllShifts(trackChanges: false);
            var normShifts = shifts.Where(s => s.ProductName != null).ToList();


            var res = new List<ProductShiftStatisticDto>();
            foreach (var product in products)
            {
                var count = 0;
                foreach (var shift in normShifts)
                {
                    if (product.Name == shift.ProductName)
                        count++;
                }
                res.Add(new ProductShiftStatisticDto { ProductName = product.Name, Shifts = count });
            }
            return Ok(res);

        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] UserForRoleDto roleDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            if (roleDto.addedRole == "Doctor")
            {
                await _userManager.RemoveFromRoleAsync(user, roleDto.removedRole);
                await _userManager.AddToRoleAsync(user, roleDto.addedRole);
                DoctorForInitialDto doctorInitial = new DoctorForInitialDto(user.FirstName, user.LastName, user.Id);
                await _service.DoctorService.CreateDoctor(doctorInitial);
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, roleDto.removedRole);
                await _userManager.AddToRoleAsync(user, roleDto.addedRole);
            }
            return NoContent();
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            await _userManager.DeleteAsync(user);
            return NoContent();
        }
    }
}
