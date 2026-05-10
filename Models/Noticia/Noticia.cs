using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntranetGCM.Models;

public class Noticia
{
	[Key]
	public int Id { get; set; }
	[Required(ErrorMessage = "O título é obrigatório")]
	[StringLength(200, ErrorMessage = "O tamanho máximo do título não pode exceder 200 caracteres.")]
	public string Titulo { get; set; }
	[Required(ErrorMessage = "O resumo é obrigatório")]
	[StringLength(500, ErrorMessage = "O tamanho máximo do resumo não pode exceder 500 caracteres.")]
	public string Resumo { get; set; }
	public string Conteudo { get; set; }
	[Required(ErrorMessage = "O autor é obrigatório")]
	public string Autor { get; set; }
	[Required(ErrorMessage = "A data de publicação é obrigatória")]
	public DateTime DataPublicacao { get; set; } = DateTime.Now;
	public DateTime? DataAtualizacao { get; set; }
	[Required(ErrorMessage = "A anotação de ativa ou não é obrigatória")]
	public bool Ativa { get; set; } = true;
	[Url]
	[StringLength(300)]
	public string ImagemUrl { get; set; }
	[Required]
	public int CategoriaId { get; set; }
	[ForeignKey(nameof(CategoriaId))]
	public CategoriaNoticia Categoria { get; set; }
}