using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Services;

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
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            var created = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetByEmpId), new { id = created.EmployeeId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(updatedEmployee);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(int id, UpdateEmployeeDto dto)
        {
            var updatedEmployee = await _employeeService.PatchEmployeeAsync(id, dto);
            return updatedEmployee == null ? NotFound() : Ok(updatedEmployee);
        }


        [HttpGet("{id}/pets")]
        public async Task<IActionResult> GetPets(int id)
        {
            var pets = await _employeeService.GetPetsByEmpIdAsync(id);
            if (pets == null) return NotFound();
            return Ok(pets);

        }

    }
}
