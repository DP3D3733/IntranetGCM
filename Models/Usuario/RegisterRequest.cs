using System.ComponentModel.DataAnnotations;

namespace IntranetGCM.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Nome obrigatório")]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "Email obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 dígitos")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "A senha deve conter letra maiúscula, minúscula, número e caractere especial")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    [Compare("Password", ErrorMessage = "As senhas devem coincidir")]
    public string? ConfirmPassword { get; set; }
}
