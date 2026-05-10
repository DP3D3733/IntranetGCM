using System.ComponentModel.DataAnnotations;

namespace IntranetGCM.Models;

public class UpdateNoticiaRequest
{
	[Key]
	public int Id { get; set; }
	[Required (ErrorMessage = "O título é obrigatório")]
	[StringLength(200, ErrorMessage = "O tamanho máximo do título não pode exceder 200 caracteres.")]
	public string Titulo { get; set; }
	[Required (ErrorMessage = "O resumo é obrigatório")]
	[StringLength(500, ErrorMessage = "O tamanho máximo do resumo não pode exceder 500 caracteres.")]
	public string Resumo { get; set; }
	[Required (ErrorMessage = "O conteúdo da notícia é obrigatório")]
	public string Conteudo { get; set; }
	[Required (ErrorMessage = "O autor é obrigatório")]
	public string Autor { get; set; }
	[Required (ErrorMessage = "A data de publicação é obrigatória")]
	public bool Ativa { get; set; } = true;
	[Url]
    [StringLength(300)]
	public string ImagemUrl { get; set; }
	[Required]
	public int CategoriaId { get; set; }
}
