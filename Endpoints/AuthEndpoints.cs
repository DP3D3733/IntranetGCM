using IntranetGCM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/login", async (
            HttpContext ctx,
            SignInManager<Usuario> signInManager,
            UserManager<Usuario> userManager,
            LoginRequest request) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                Console.WriteLine("Verificando autorização");
                if (user is null)
                {
                    return Results.Unauthorized();
                }


                var result = await signInManager.PasswordSignInAsync(
                    user,
                    request.Password,
                    false,
                    false);

                if (!result.Succeeded)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok("Logado com sucesso");
            }
        );

        app.MapPost("/logout", async (SignInManager<Usuario> signInManager) =>
        {
            await signInManager.SignOutAsync();
        });
    }
}