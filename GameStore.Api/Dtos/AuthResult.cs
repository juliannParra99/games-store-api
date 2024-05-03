namespace GameStore.Api;

// Represents the result of an authentication operation, including a JWT token on success (Token),
// a boolean indicating the result of the operation (Result), and a list of errors as strings on failure (Errors).

public class AuthResult
{
    public string? Token { get; set; }
    public bool Result { get; set; }
    public List<string>? Errors { get; set; }

}
