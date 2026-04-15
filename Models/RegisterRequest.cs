using System.ComponentModel.DataAnnotations;

namespace IntranetGCM.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(6)]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "Email obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}
