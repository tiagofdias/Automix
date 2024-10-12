using System.ComponentModel.DataAnnotations;

namespace Automix.Models
{
    public class AddPercursosAlunosViewModel
    {
        [Required(ErrorMessage = "Introduza o Aluno")]
        public int IDAluno { get; set; }
        [Required(ErrorMessage = "Introduza o Percurso")]
        public int IDPercurso { get; set; }
    }
}
