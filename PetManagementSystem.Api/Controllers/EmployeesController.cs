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

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService empService)
        {
            _employeeService = empService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var emps = await _employeeService.GetAllEmployeesAsync();
            return Ok(emps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByEmpId(int id)
        {
            var emp = await _employeeService.GetEmpByIdAsync(id);
            return Ok(emp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(WriteEmployeeDto dto)
        {
            var created = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetByEmpId), new { id = created.EmployeeId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, WriteEmployeeDto dto)
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(updatedEmployee);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(int id, WriteEmployeeDto dto)
        {
            var updatedEmployee = await _employeeService.PatchEmployeeAsync(id, dto);
            return Ok(updatedEmployee);
        }


        [HttpGet("{id}/pets")]
        public async Task<IActionResult> GetPets(int id)
        {
            var pets = await _employeeService.GetPetsByEmpIdAsync(id);
            return Ok(pets);
        }

        // Login POST action in MVC
        [HttpGet("profile/me")]
        public async Task<IActionResult> GetProfile()
        {
            // Extract employee ID from JWT claims
            var empIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (empIdClaim == null || !int.TryParse(empIdClaim, out int empId))
                return Unauthorized(new { message = "Invalid or missing token." });

            var emp = await _employeeService.GetEmpByIdAsync(empId);
            if (emp == null)
                return NotFound(new { message = "Employee profile not found." });

            return Ok(emp);
        }

    }
}
