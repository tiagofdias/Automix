using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automix.Models.Domain
{
    [Table("Contactos")]
    public class Contactos
    {
        [Key]
        public int IDContacto { get; set; }
        public string Email { get; set; }
        public int Contacto { get; set; }
        public int ContactoAlternativo { get; set; }

        public List<Alunos> Alunos { get; set; }
    }
}
