using IntranetGCM.Models;
using Microsoft.EntityFrameworkCore;

namespace IntranetGCM.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	public DbSet<Noticia> Noticias { get; set; }
	public DbSet<CategoriaNoticia> CategoriaNoticia { get; set; }
}
