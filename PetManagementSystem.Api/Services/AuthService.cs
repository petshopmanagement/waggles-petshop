
//using AutoMapper;
//using BCrypt.Net;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using PetManagementSystem.Api.Data;
//using PetManagementSystem.Api.DTOs;
//using PetManagementSystem.Api.Exceptions;
//using PetManagementSystem.Api.Models;
//using PetManagementSystem.Api.Repositories;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Authentication;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace PetManagementSystem.Api.Services;

//public class AuthService : IAuthService
//{
//    private readonly ICustomerRepository _customerRepo;
//    //private readonly ISupplierRepository _supplierRepo;
//    //private readonly IEmployeeRepository _employeeRepo;
//    private readonly IConfiguration _configuration;
//    private readonly IMapper _mapper;

//    public AuthService(ICustomerRepository customerRepo, IConfiguration configuration, IMapper mapper)
//    {
//        _customerRepo = customerRepo;
//        //_supplierRepo = supplierRepo;
//        //_employeeRepo = employeeRepo;
//        _configuration = configuration;
//        _mapper = mapper;
//    }

//    public async Task<AuthResponse> LoginAsync(LoginRequest request)
//    {
//        string role = request.Role.ToLower();
//        string email = request.Email.ToLower();

//        if (role == "customer")
//        {
//            var user = await _customerRepo.GetByEmailAsync(email);
//            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//                throw new InvalidCredentialsException();

//            return GenerateToken(user.CustomerId.ToString(), user.Email, "Customer", $"{user.FirstName} {user.LastName}");
//        }
//        //else if (role == "supplier")
//        //{
//        //    var user = await _supplierRepo.GetByEmailAsync(email);
//        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//        //        throw new InvalidCredentialsException();

//        //    return GenerateToken(user.SupplierId.ToString(), user.Email, "Supplier", user.Name);
//        //}
//        //else if (role == "employee")
//        //{
//        //    var user = await _employeeRepo.GetByEmailAsync(email);
//        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//        //        throw new InvalidCredentialsException();

//        //    // Determine if the employee is an admin (e.g., manager)
//        //    string userRole = "Employee";
//        //    if (!string.IsNullOrEmpty(user.Position) && user.Position.ToLower() == "manager")
//        //    {
//        //        userRole = "Admin";
//        //    }

//        //    return GenerateToken(user.EmployeeId.ToString(), user.Email, userRole, $"{user.FirstName} {user.LastName}");
//        //}

//        throw new InvalidRoleException();
//    }

//    public async Task<string> RegisterAsync(RegisterRequest request)
//    {
//        string role = request.Role.ToLower();
//        string email = request.Email.ToLower();

//        if (role == "customer")
//        {
//            if (await _customerRepo.GetByEmailAsync(email) != null)
//                throw new EmailAlreadyExistsException();

//            var customer = new Customer
//            {
//                Email = email,
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
//                FirstName = request.FirstName,
//                LastName = request.LastName,
//                PhoneNumber = request.PhoneNumber,
//                Address = _mapper.Map<Address>(request.Address)
//            };

//            await _customerRepo.CreateAsync(customer);

//            return "Registration successful! Please login to continue.";
//        }
//        //else if (role == "supplier")
//        //{
//        //    if (await _supplierRepo.GetByEmailAsync(email) != null)
//        //        throw new EmailAlreadyExistsException();

//        //    var supplier = new Supplier
//        //    {
//        //        Email = email,
//        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
//        //        Name = request.Name,
//        //        ContactPerson = request.ContactPerson,
//        //        PhoneNumber = request.PhoneNumber,
//        //        Address = _mapper.Map<Address>(request.Address)
//        //    };

//        //    await _supplierRepo.CreateAsync(supplier);

//        //    return "Registration successful! Please login to continue.";
//        //}
//        //else if (role == "employee")
//        //{
//        //    if (await _employeeRepo.GetByEmailAsync(email) != null)
//        //        throw new EmailAlreadyExistsException();

//        //    var employee = new Employee
//        //    {
//        //        Email = email,
//        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
//        //        FirstName = request.FirstName,
//        //        LastName = request.LastName,
//        //        PhoneNumber = request.PhoneNumber,
//        //        Position = request.Position,
//        //        HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
//        //        Address = _mapper.Map<Address>(request.Address)
//        //    };

//        //    await _employeeRepo.CreateAsync(employee);

//        //    return "Registration successful! Please login to continue.";
//        //}

//        throw new InvalidRoleException();
//    }

//    private AuthResponse GenerateToken(string id, string email, string role, string? name)
//    {
//        var jwtSettings = _configuration.GetSection("Jwt");
//        var secretKey = jwtSettings["Key"] ?? "super_secret_key_that_is_long_enough_for_hmac_sha256_and_even_longer_now_for_hs256_algo";

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, id),
//            new Claim(ClaimTypes.Email, email ?? string.Empty),
//            new Claim(ClaimTypes.Role, role ?? string.Empty),
//            new Claim(ClaimTypes.Name, name ?? string.Empty)
//        };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(
//            issuer: jwtSettings["Issuer"] ?? "PetStoreApi",
//            audience: jwtSettings["Audience"] ?? "PetStoreClients",
//            claims: claims,
//            expires: DateTime.UtcNow.AddHours(2),
//            signingCredentials: creds
//        );

//        return new AuthResponse
//        {
//            Token = new JwtSecurityTokenHandler().WriteToken(token),
//            Role = role ?? string.Empty,
//            Email = email ?? string.Empty,
//            Name = name ?? string.Empty
//        };
//    }
//}

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
    private readonly ISupplierRepository _supplierRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IMapper _mapper;
    private readonly JwtHelper _jwtHelper;

    public AuthService(ICustomerRepository customerRepo, ISupplierRepository supplierRepo, IEmployeeRepository employeeRepo, IMapper mapper, JwtHelper jwtHelper)
    {
        _customerRepo = customerRepo;
        _supplierRepo = supplierRepo;
        _employeeRepo = employeeRepo;
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
        else if (role == "supplier")
        {
            var user = await _supplierRepo.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            return _jwtHelper.GenerateToken(user.SupplierId.ToString(), user.Email, "Supplier", user.Name);
        }
        else if (role == "employee")
        {
            var user = await _employeeRepo.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            // Determine if the employee is an admin (e.g., manager)
            string userRole = "Employee";
            if (!string.IsNullOrEmpty(user.Position) && user.Position.ToLower() == "manager")
            {
                userRole = "Admin";
            }

            return _jwtHelper.GenerateToken(user.EmployeeId.ToString(), user.Email, userRole, $"{user.FirstName} {user.LastName}");
        }

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
        else if (role == "supplier")
        {
            if (await _supplierRepo.GetByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            var supplier = new Supplier
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name,
                ContactPerson = request.ContactPerson,
                PhoneNumber = request.PhoneNumber,
                Address = _mapper.Map<Address>(request.Address)
            };

            await _supplierRepo.CreateAsync(supplier);

            return "Registration successful! Please login to continue.";
        }
        else if (role == "employee")
        {
            if (await _employeeRepo.GetByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            var employee = new Employee
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Position = request.Position,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Address = _mapper.Map<Address>(request.Address)
            };

            await _employeeRepo.CreateAsync(employee);

            return "Registration successful! Please login to continue.";
        }

        throw new InvalidRoleException();
    }
}

