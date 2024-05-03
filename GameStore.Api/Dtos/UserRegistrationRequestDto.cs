using System.ComponentModel.DataAnnotations;

namespace GameStore.Api;

// DTO (Data Transfer Object) used for transferring user registration data from the client to the API.
// Includes properties for the user's name, email, and password, all of which are required fields.

public class UserRegistrationRequestDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

}
