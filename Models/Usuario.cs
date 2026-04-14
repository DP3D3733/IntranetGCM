using Microsoft.AspNetCore.Identity;

namespace IntranetGCM.Models;

public class Usuario : IdentityUser
{
    public string Nome { get; set; }
}
