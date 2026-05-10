using IntranetGCM.Data;
using IntranetGCM.Models;
using Microsoft.EntityFrameworkCore;


namespace IntranetGCM.Services;

using Azure.Core;
using Microsoft.AspNetCore.Identity;
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

	public async Task<(bool success, List<string> errors)> AtualizarNoticia(UpdateNoticiaRequest request)
	{
		using var context = _factory.CreateDbContext();
		var noticia = await context.Noticias.FindAsync(request.Id);
		noticia.Titulo = request.Titulo;
		noticia.Resumo = request.Resumo;
		noticia.Conteudo = request.Conteudo;
		noticia.Autor = request.Autor;
		noticia.Ativa = request.Ativa;
		noticia.ImagemUrl = request.ImagemUrl;
		noticia.CategoriaId = request.CategoriaId;
		try
		{
			context.Noticias.Update(noticia);
			await context.SaveChangesAsync();

			return (true, new List<string>());
		}
		catch (Exception ex)
		{
			// Captura erros de banco (ex: violação de chave estrangeira ou campos nulos)
			return (false, new List<string> { ex.Message });
		}
	}

	public async Task<(bool Success, List<string> Errors)> ExcluirNoticia(int id)
	{
		using var context = _factory.CreateDbContext();
		var noticia = await context.Noticias.FindAsync(id);

		if (noticia == null)
			return (false, new List<string> { "Notícia não encontrada" });

		try
		{
			context.Noticias.Remove(noticia);
			await context.SaveChangesAsync();

			return (true, new List<string>());
		}
		catch (Exception ex)
		{
			// Captura erros de banco (ex: violação de chave estrangeira ou campos nulos)
			return (false, new List<string> { ex.Message });
		}
	}
}