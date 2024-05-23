using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DataTransferObjects.DoctorDTOs;
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
