using IntranetGCM.Data;
using IntranetGCM.Models;
using Microsoft.EntityFrameworkCore;


namespace IntranetGCM.Services;

using Microsoft.EntityFrameworkCore;

public class NoticiaService
{
	private readonly IDbContextFactory<AppDbContext> _factory;

	public NoticiaService(IDbContextFactory<AppDbContext> factory)
	{
		_factory = factory;
	}

	public async Task<(bool Sucesso, string? Erro)> Criar(Noticia noticia)
	{
		try
		{
			using var context = _factory.CreateDbContext();

			context.Noticias.Add(noticia);

			await context.SaveChangesAsync();

			return (true, null);
		}
		catch (Exception ex)
		{
			return (false, ex.Message);
		}
	}

	public async Task<List<CategoriaNoticia>> ListarCategorias()
	{
		using var context = _factory.CreateDbContext();
		return await context.CategoriaNoticia.ToListAsync();
	}

	public async Task<List<Noticia>> ListarNoticia()
	{
		using var context = _factory.CreateDbContext();
		return await context.Noticias.ToListAsync();
	}

	public async Task<Noticia> GetNoticia(int id)
	{
		using var context = _factory.CreateDbContext();
		return await context.Noticias.FindAsync(id);
	}
}