namespace IntranetGCM.Models;

public class Noticia
{
	public int Id { get; set; }
	public string Titulo { get; set; }
	public string Resumo { get; set; }
	public string Conteudo { get; set; }
	public string Autor { get; set; }
	public DateTime DataPublicacao { get; set; }
	public DateTime? DataAtualizacao { get; set; }
	public bool Ativa { get; set; } = true;
	public string ImagemUrl { get; set; }
	public int CategoriaId { get; set; }
	public CategoriaNoticia Categoria { get; set; }
}