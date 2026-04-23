namespace IntranetGCM.Models;

public class CategoriaNoticia
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public List<Noticia> Noticias { get; set; }
}
