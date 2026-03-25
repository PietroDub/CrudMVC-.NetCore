using System.ComponentModel.DataAnnotations;

namespace AspNetCoreCrud.Models
{
    public class Estudante
    {
        [Key]
        [Display(Name ="Estudante_id")]
        public int estudanteId { get; set; }
        [Required]
        [Display(Name ="Nome_estudante")]
        public string? Nome { get; set; }
        //[Display(Name ="Email_estudante")]
        public string? Email { get; set; }
        public string? Course { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
