using IntranetGCM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IntranetGCM.Services;

public class UsuarioService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioService(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterRequest request)
    {
        var user = new Usuario
        {
            Nome = request.Nome,
            UserName = request.Email,
            Email = request.Email
        };
        Console.WriteLine($"Nome antes de salvar: {user.Nome}");

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToList());
        }

        await _userManager.AddToRoleAsync(user, "User");

        return (true, new List<string>());
    }

    public async Task<List<Usuario>> ListarUsuarios()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<List<string>> GetRoles(Usuario user)
    {
        return (await _userManager.GetRolesAsync(user)).ToList();
    }

    public async Task<List<string>> GetTodasRoles()
    {
        return await _roleManager.Roles
            .Select(r => r.Name)
            .ToListAsync();
    }

    public async Task<(bool Success, List<string> Errors)> AtualizarRoles(List<string> roles, Usuario usuario)
    {
        var rolesAtuais = await _userManager.GetRolesAsync(usuario);

        var removeResult = await _userManager.RemoveFromRolesAsync(usuario, rolesAtuais);

        if (!removeResult.Succeeded)
        {
            return (false, removeResult.Errors.Select(e => e.Description).ToList());
        }

        var result = await _userManager.AddToRolesAsync(usuario, roles);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToList());
        }

        return (true, new List<string>());
    }

    public async Task<(bool Success, List<string> Errors)> AtualizarUsuario(UpdateCadastroRequest request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email);

        if (usuario == null)
            return (false, new List<string> { "Usuário não encontrado" });

        usuario.Nome = request.Nome;
        usuario.Email = request.Email;
        usuario.UserName = request.Email;

        var result = await _userManager.UpdateAsync(usuario);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToList());
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            await _userManager.RemovePasswordAsync(usuario);
            var resultTrocaPassword = await _userManager.AddPasswordAsync(usuario, request.Password);

            if (!resultTrocaPassword.Succeeded)
            {
                return (false, resultTrocaPassword.Errors.Select(e => e.Description).ToList());
            }
        }



        return (true, new List<string>());
    }

    public async Task<(bool Success, List<string> Errors)> ExcluirUsuario(string userId)
    {
        var usuario = await _userManager.FindByIdAsync(userId);

        if (usuario == null)
            return (false, new List<string> { "Usuário não encontrado" });

        if (usuario.Id == _userManager.GetUserId(_httpContextAccessor.HttpContext.User))
        {
            return (false, new List<string> { "Você não pode excluir seu próprio usuário" });
        }

        var result = await _userManager.DeleteAsync(usuario);

        if (!result.Succeeded)
        {
            return (false, result.Errors
            .Select(e => e.Description)
            .ToList());
        }

        return (true, new List<string>());
    }
}
