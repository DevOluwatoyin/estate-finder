using System;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
