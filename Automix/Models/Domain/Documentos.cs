using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automix.Models.Domain
{

    [Table("Documentos")]
    public class Documentos
    {
        [Key]
        public int IDDocumento { get; set; }

        public byte[] Documento { get; set; }

        public string Extensao { get; set; }

        public string Nome { get; set; }

        public List<Alunos> Alunos { get; set; }
    }
}
