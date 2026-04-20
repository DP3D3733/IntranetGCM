using IntranetGCM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/login", async (
            HttpContext ctx,
            SignInManager<Usuario> signInManager,
            UserManager<Usuario> userManager) =>
        {
            var form = await ctx.Request.ReadFormAsync();

            var email = form["email"].ToString();
            var password = form["password"].ToString();

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Results.Redirect($"/login?erro={Uri.EscapeDataString("Credenciais inválidas")}");
            var result = await signInManager.PasswordSignInAsync(
                user,
                password,
                true,
                false);

            if (!result.Succeeded)
                return Results.Redirect($"/login?erro={Uri.EscapeDataString("Credenciais inválidas")}");

            return Results.Redirect("/");
        });

        app.MapPost("/logout", async (SignInManager<Usuario> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Redirect("/");

        });
    }
}