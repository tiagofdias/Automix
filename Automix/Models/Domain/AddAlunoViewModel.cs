using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Automix.Models.Domain
{
    public class AddAlunoViewModel
    {
    
        [Required(ErrorMessage = "Introduza o NIF")]
        [Remote(action: "IsValidContrib", controller: "Alunos")]
        public int NIFAluno { get; set; }

        [Required(ErrorMessage = "Introduza um Nome")]
        [RegularExpression(@"^[a-z^à-ú A-ZÀ-ú]+$", ErrorMessage = "Introduza o nome corretamente")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Introduza a Foto")]
        [DataType(DataType.Upload)]
        [Display(Name = "Foto")]
        public IFormFile Foto { get; set; }

        [Required(ErrorMessage = "Introduza a Morada")]
        public string Morada { get; set; }

        [Required(ErrorMessage = "Introduza a Localidade")]
        [RegularExpression(@"^[a-z^à-ú A-ZÀ-ú]+$", ErrorMessage = "Introduza a Localidade corretamente")]
        public string Localidade { get; set; }

        [Required(ErrorMessage = "Introduza um Código Postal")]
        [RegularExpression(@"^\d{4}-\d{3}?$", ErrorMessage = "Introduza um Código Postal válido")]
        [DataType(DataType.PostalCode)]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Introduza um Email")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"+ "@"+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Introduza um email válido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Introduza um email válido")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Introduza um contacto telefónico")]
        [RegularExpression("^(9[1236][0-9]) ?([0-9]{3}) ?([0-9]{3})", ErrorMessage = "Introduza um contacto válido")]
        public int Contacto { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Introduza um contacto alternativo")]
        [RegularExpression("^(9[1236][0-9]) ?([0-9]{3}) ?([0-9]{3})", ErrorMessage = "Introduza um contacto válido")]
        public int ContactoAlternativo { get; set; }

        [Required(ErrorMessage = "Introduza o Atestado Médico")]
        [DataType(DataType.Upload)]
        [Display(Name = "Documento")]
        public IFormFile Documento { get; set; }
    }
}
