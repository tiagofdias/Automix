using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automix.Models.Domain
{
    [Table("Alunos")]
    public class Alunos
    {
        [Key]
        public int IDAluno { get; set; }
        public int NIFAluno { get; set; }
        public int IDContacto { get; set; }
        public int IDMorada { get; set; }
        public int IDDocumento { get; set; }
        public string Nome { get; set; }
        public byte[]? Foto { get; set; }

        [ForeignKey("IDContacto")]
        public virtual Contactos Contactos { get; set; }

        [ForeignKey("IDMorada")]
        public virtual Moradas Moradas { get; set; }

        [ForeignKey("IDDocumento")]
        public virtual Documentos Documentos { get; set; }
    }
}
