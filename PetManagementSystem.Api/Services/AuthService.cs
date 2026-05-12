
using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepo;
    private readonly IMapper _mapper;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
        IAuthRepository authRepo,
        IMapper mapper,
        JwtHelper jwtHelper)
    {
        _authRepo = authRepo;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        string role = request.Role.ToLower();
        string email = request.Email.ToLower();

        if (role == "customer")
        {
            var user = await _authRepo.GetCustomerByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            return _jwtHelper.GenerateToken(
                user.CustomerId.ToString(),
                user.Email,
                "Customer",
                $"{user.FirstName} {user.LastName}"
            );
        }

        if (role == "employee")
        {
            var user = await _authRepo.GetEmployeeByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            string finalRole = "Employee";

          
            if (user.Position.Equals("Manager", StringComparison.OrdinalIgnoreCase) &&
                email == "jennifer.davis@example.com" &&
                request.Password == "Jennifer@123")
            {
                finalRole = "Admin";
            }

            return _jwtHelper.GenerateToken(
                user.EmployeeId.ToString(),
                user.Email,
                finalRole,
                $"{user.FirstName} {user.LastName}"
            );
        }

        if (role == "supplier")
        {
            var user = await _authRepo.GetSupplierByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

            return _jwtHelper.GenerateToken(
                user.SupplierId.ToString(),
                user.Email,
                "Supplier",
                user.Name
            );
        }

        throw new InvalidRoleException();
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        string role = request.Role.ToLower();
        string email = request.Email.ToLower();

        if (role == "customer")
        {
            if (await _authRepo.GetCustomerByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            var customer = new Customer
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address == null ? null : _mapper.Map<Address>(request.Address)
            };

            await _authRepo.CreateCustomerAsync(customer);

            return "Customer registration successful. Please login to continue.";
        }

        if (role == "employee")
        {
            if (await _authRepo.GetEmployeeByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            // Manager Registration Protection
            if (request.Position.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                if (!email.Equals("jennifer.davis@example.com", StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException("Unauthorized manager registration.");
            }

            var employee = new Employee
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Position = request.Position,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Address = request.Address == null ? null : _mapper.Map<Address>(request.Address)
            };

            await _authRepo.CreateEmployeeAsync(employee);

            return "Employee registration successful. Please login to continue.";
        }

        if (role == "supplier")
        {
            if (await _authRepo.GetSupplierByEmailAsync(email) != null)
                throw new EmailAlreadyExistsException();

            var supplier = new Supplier
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name,
                ContactPerson = request.ContactPerson,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address == null ? null : _mapper.Map<Address>(request.Address)
            };

            await _authRepo.CreateSupplierAsync(supplier);

            return "Supplier registration successful. Please login to continue.";
        }

        throw new InvalidRoleException();
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        string role = request.Role.ToLower();
        string email = request.Email.ToLower();

        if (role == "customer")
        {
            var user = await _authRepo.GetCustomerByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                throw new InvalidCredentialsException();

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _authRepo.UpdateCustomerAsync(user);
            return;
        }

        if (role == "employee" || role == "admin")
        {
            var user = await _authRepo.GetEmployeeByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                throw new InvalidCredentialsException();

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _authRepo.UpdateEmployeeAsync(user);
            return;
        }

        if (role == "supplier")
        {
            var user = await _authRepo.GetSupplierByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                throw new InvalidCredentialsException();

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _authRepo.UpdateSupplierAsync(user);
            return;
        }

        throw new InvalidRoleException();
    }
}

