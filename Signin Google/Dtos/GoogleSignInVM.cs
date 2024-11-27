using System.ComponentModel.DataAnnotations;

namespace Signin_Google.Dtos;

public class GoogleSignInVM
{
    [Required]
    public string IdToken { get; set; }
}
