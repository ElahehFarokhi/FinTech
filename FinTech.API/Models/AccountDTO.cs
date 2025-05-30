using System.ComponentModel.DataAnnotations;

namespace FinTech.API.Models;

public class CreateAccountRequest
{
    [Required(ErrorMessage = "Owner name is required.")]
    [StringLength(100, ErrorMessage = "Owner name cannot be longer than 100 characters.")]
    public required string OwnerName { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Initial balance must be greater than zero.")]
    public decimal InitialBalance { get; set; }
}
