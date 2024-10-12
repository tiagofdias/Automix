using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automix.Models.Domain
{
    [Table("Moradas")]
    public class Moradas
    {
        [Key]
        public int IDMorada { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string CodigoPostal { get; set; }

        public List<Alunos> Alunos { get; set; }
    }
}
