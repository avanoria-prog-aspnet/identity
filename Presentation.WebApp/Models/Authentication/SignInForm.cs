using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication;

public class SignInForm
{
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    public string? ErrorMessage { get; set; } 
}
