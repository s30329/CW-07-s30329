using System.ComponentModel.DataAnnotations;

namespace APBD07.Models;

public class Client
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Telephone { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must be 11 digits")]
    public string Pesel { get; set; } = null!;
}