using System.ComponentModel.DataAnnotations;

namespace Automix.Models
{
    public class UpdatePercursoViewModel
    {
       
        public int IDPercurso { get; set; }

        [Required(ErrorMessage = "Introduza um Nome")]
        [RegularExpression(@"^[a-z^à-ú A-ZÀ-ú]+$", ErrorMessage = "Introduza o nome corretamente")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }
    }
}
