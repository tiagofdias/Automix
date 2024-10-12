using System.ComponentModel.DataAnnotations;

namespace Automix.Models.Domain
{
    public class DetailsAlunosViewModel
    {
        public int IDAluno { get; set; }
        public int NIFAluno { get; set; }
        public string Nome { get; set; }
        public IFormFile Foto { get; set; }
        public string Morada { get; set; }
        public string Localidade { get; set; }
        public string CodigoPostal { get; set; }
        public string Email { get; set; }
        public int Contacto { get; set; }
        public int ContactoAlternativo { get; set; }
        public IFormFile? Documento { get; set; }

        public string NomeDocumento { get; set; }
    }
}
