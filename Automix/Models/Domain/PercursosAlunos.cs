using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automix.Models.Domain
{
    [Table("PercursosAlunos")]
    public class PercursosAlunos
    {
        [Key]
        public int IDPercursoAluno { get; set; }
        public int IDAluno { get; set; }
        public int IDPercurso { get; set; }

        [ForeignKey("IDAluno")]
        public virtual Alunos Alunos { get; set; }

        [ForeignKey("IDPercurso")]
        public virtual Percursos Percursos { get; set; }
    }
}
