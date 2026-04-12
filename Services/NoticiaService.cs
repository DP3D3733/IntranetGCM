using IntranetGCM.Data;
using IntranetGCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IntranetGCM.Services;

public class NoticiaService
{
	private readonly AppDbContext _context;

	public NoticiaService(AppDbContext context)
	{
		_context = context;
	}

	public async Task Criar(Noticia noticia)
	{
		_context.Noticias.Add(noticia);
		await _context.SaveChangesAsync();
	}

	public async Task<List<CategoriaNoticia>> ListarCategorias()
	{
		return await _context.CategoriaNoticia.ToListAsync();
	}

	public async Task<List<Noticia>> ListarNoticia()
	{
		return await _context.Noticias.ToListAsync();
	}
}
