using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automix.Models
{
    public class AddPercursoViewModel
    {
        [Required(ErrorMessage = "Introduza um Nome")]
        [RegularExpression(@"^[a-z^à-ú A-ZÀ-ú]+$", ErrorMessage = "Introduza o nome corretamente")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

    }
}
