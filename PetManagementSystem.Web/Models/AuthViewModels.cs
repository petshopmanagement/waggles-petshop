using System.ComponentModel.DataAnnotations;

namespace PetManagementSystem.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "Customer";
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Customer";

        // Profile fields (role-conditional — NOT [Required] here; API FluentValidation enforces per-role)
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "First Name can contain only alphabets.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Last Name can contain only alphabets.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [StringLength(50, ErrorMessage = "Contact Person cannot exceed 50 characters.")]
        public string? ContactPerson { get; set; }

        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters.")]
        public string? Position { get; set; }

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone Number must contain exactly 10 digits.")]
        public string? PhoneNumber { get; set; }

        // Address fields
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        public string Role { get; set; } = "Customer";

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}