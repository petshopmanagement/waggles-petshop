

using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using BCrypt.Net;

namespace PetManagementSystem.Api.Services;

public class AuthService : IAuthService
{
    private readonly ICustomerRepository _customerRepo;
    //private readonly ISupplierRepository _supplierRepo;
    //private readonly IEmployeeRepository _employeeRepo;
    private readonly IMapper _mapper;
    private readonly JwtHelper _jwtHelper;

    public AuthService(ICustomerRepository customerRepo,  IMapper mapper, JwtHelper jwtHelper)
    {
        _customerRepo = customerRepo;
        //_supplierRepo = supplierRepo;
        //_employeeRepo = employeeRepo;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        string role = request.Role.ToLower();
        string email = request.Email.ToLower();

        if (role == "customer")
        {
            var user = await _customerRepo.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            return _jwtHelper.GenerateToken(user.CustomerId.ToString(), user.Email, "Customer", $"{user.FirstName} {user.LastName}");
        }
        //else if (role == "supplier")
        //{
        //    var user = await _supplierRepo.GetByEmailAsync(email);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //        throw new InvalidCredentialsException();

        //    return _jwtHelper.GenerateToken(user.SupplierId.ToString(), user.Email, "Supplier", user.Name);
        //}
        //else if (role == "employee")
        //{
        //    // Hardcoded Admin login bypass
        //    if (email == "jennifer.davis@example.com" && request.Password == "Jennifer@123")
        //    {
        //        return _jwtHelper.GenerateToken("4", email, "Admin", "Jennifer Davis");
        //    }

        //    var user = await _employeeRepo.GetByEmailAsync(email);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //        throw new InvalidCredentialsException();

           
        //    return _jwtHelper.GenerateToken(user.EmployeeId.ToString(), user.Email, "Employee", $"{user.FirstName} {user.LastName}");
        //}

        throw new InvalidRoleException();
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        string role = request.Role.ToLower();
        string email = request.Email.ToLower();

        if (role == "customer")
        {
            if (await _customerRepo.GetByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            var customer = new Customer
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = _mapper.Map<Address>(request.Address)
            };

            await _customerRepo.CreateAsync(customer);

            return "Registration successful! Please login to continue.";
        }
        //else if (role == "supplier")
        //{
        //    if (await _supplierRepo.GetByEmailAsync(email) != null)
        //        throw new EmailAlreadyExistsException();

        //    var supplier = new Supplier
        //    {
        //        Email = email,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        //        Name = request.Name,
        //        ContactPerson = request.ContactPerson,
        //        PhoneNumber = request.PhoneNumber,
        //        Address = _mapper.Map<Address>(request.Address)
        //    };

        //    await _supplierRepo.CreateAsync(supplier);

        //    return "Registration successful! Please login to continue.";
        //}
        //else if (role == "employee")
        //{
        //    if (await _employeeRepo.GetByEmailAsync(email) != null)
        //        throw new EmailAlreadyExistsException();

        //    var employee = new Employee
        //    {
        //        Email = email,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        //        FirstName = request.FirstName,
        //        LastName = request.LastName,
        //        PhoneNumber = request.PhoneNumber,
        //        Position = request.Position,
        //        HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
        //        Address = _mapper.Map<Address>(request.Address)
        //    };

        //    await _employeeRepo.CreateAsync(employee);

        //    return "Registration successful! Please login to continue.";
        //}

        throw new InvalidRoleException();
    }
}

