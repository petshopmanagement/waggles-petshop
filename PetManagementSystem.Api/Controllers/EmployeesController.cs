using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService empService)
        {
            _employeeService = empService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var emps = await _employeeService.GetAllEmployeesAsync();
                return Ok(emps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetByEmpId(int id)
        {
            try
            {
                var emp = await _employeeService.GetEmpByIdAsync(id);
                if (emp == null)
                    return NotFound();
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee(WriteEmployeeDto dto)
        {
            try
            {
                var created = await _employeeService.CreateEmployeeAsync(dto);
                return CreatedAtAction(nameof(GetByEmpId), new { id = created.EmployeeId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, WriteEmployeeDto dto)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, dto);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchEmployee(int id, WriteEmployeeDto dto)
        {
            try
            {
                var updatedEmployee = await _employeeService.PatchEmployeeAsync(id, dto);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }


        [HttpGet("{id}/pets")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetPets(int id)
        {
            try
            {
                var pets = await _employeeService.GetPetsByEmpIdAsync(id);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

    
        [HttpGet("profile/me")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var empIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (empIdClaim == null || !int.TryParse(empIdClaim, out int empId))
                    return Unauthorized(new { message = "Invalid or missing token." });

                var emp = await _employeeService.GetEmpByIdAsync(empId);
                if (emp == null)
                    return NotFound(new { message = "Employee profile not found." });

                return Ok(emp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

    }
}
