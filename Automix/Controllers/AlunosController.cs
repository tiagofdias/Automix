using Automix.Data;
using Automix.Models;
using Automix.Models.Domain;
using ceTe.DynamicPDF.Viewer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using X.PagedList;

namespace Automix.Controllers
{
    public class AlunosController : Controller
    {
        private readonly AutomixDbContext automixDbContext;
        private readonly IWebHostEnvironment _hostenvironment;

        public AlunosController(AutomixDbContext AutomixDbContext, IWebHostEnvironment webHostEnvironment)
        {
            automixDbContext = AutomixDbContext;
            _hostenvironment = webHostEnvironment;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult IsValidContrib(string NIFAluno)
        {

            string[] s = new string[9];
            string Ss = null;
            string C = null;
            int i = 0;
            long checkDigit = 0;

            try
            {

                s[0] = Convert.ToString(NIFAluno[0]);
                s[1] = Convert.ToString(NIFAluno[1]);
                s[2] = Convert.ToString(NIFAluno[2]);
                s[3] = Convert.ToString(NIFAluno[3]);
                s[4] = Convert.ToString(NIFAluno[4]);
                s[5] = Convert.ToString(NIFAluno[5]);
                s[6] = Convert.ToString(NIFAluno[6]);
                s[7] = Convert.ToString(NIFAluno[7]);
                s[8] = Convert.ToString(NIFAluno[8]);

                if (NIFAluno.Length == 9)
                {
                    C = s[0];
                    if (s[0] == "1" || s[0] == "2" || s[0] == "5" || s[0] == "6" || s[0] == "9")
                    {
                        checkDigit = Convert.ToInt32(C) * 9;
                        for (i = 2; i <= 8; i++)
                        {
                            checkDigit = checkDigit + (Convert.ToInt32(s[i - 1]) * (10 - i));
                        }
                        checkDigit = 11 - (checkDigit % 11);
                        if ((checkDigit >= 10))
                            checkDigit = 0;
                        Ss = s[0] + s[1] + s[2] + s[3] + s[4] + s[5] + s[6] + s[7] + s[8];
                        if ((checkDigit == Convert.ToInt32(s[8])))
                            return Json(true);

                    }
                }
                return Json("Introduza um NIF válido");

            }
            catch
            {
                return Json("Introduza um NIF válido");

            }

        }

        public async Task<IActionResult> Index(string sortOrder = "", string currentFilter = "", string searchString = null, int? pageNumber = 1)
        {

            ViewData["Modulo"] = "Alunos";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IDAluno"] = sortOrder == "IDAluno_desc" ? "IDAluno" : "IDAluno_desc";
            ViewData["Nome"] = sortOrder == "Nome" ? "Nome_desc" : "Nome";
            ViewData["NIFAluno"] = sortOrder == "NIFAluno" ? "NIFAluno_desc" : "NIFAluno";
            ViewData["Contacto"] = sortOrder == "Contacto" ? "Contacto_desc" : "Contacto";
            ViewData["ContactoAlternativo"] = sortOrder == "ContactoAlternativo" ? "ContactoAlternativo_desc" : "ContactoAlternativo";
            ViewData["Email"] = sortOrder == "Email" ? "Email_desc" : "Email";

            //Paginação
            if (searchString != null) pageNumber = 1; else searchString = currentFilter;

            //Filtro
            ViewData["Filtro"] = searchString;

            var Alunos = from s in automixDbContext.Alunos.Include(x => x.Moradas).Include(x => x.Contactos) select s;

            //Filtrar tmb faz parte este if (para adicionar mais strings no futuro fazer ex:s => s.Primeiro campo.Contains(searchString) || s.Segundo Campo.Contains(searchString)); e por ai vai
            if (!String.IsNullOrEmpty(searchString)) Alunos = Alunos.Where(s => s.Nome.Contains(searchString));

            switch (sortOrder)
            {
                case "IDAluno":
                    Alunos = Alunos.OrderByDescending(s => s.IDAluno);
                    break;

                case "IDAluno_desc":
                    Alunos = Alunos.OrderByDescending(s => s.IDAluno);
                    break;

                case "Nome":
                    Alunos = Alunos.OrderBy(s => s.Nome);
                    break;

                case "Nome_desc":
                    Alunos = Alunos.OrderByDescending(s => s.Nome);
                    break;

                case "NIFAluno":
                    Alunos = Alunos.OrderBy(s => s.NIFAluno);
                    break;

                case "NIFAluno_desc":
                    Alunos = Alunos.OrderByDescending(s => s.NIFAluno);
                    break;

                case "Contacto":
                    Alunos = Alunos.OrderBy(s => s.Contactos.Contacto);
                    break;

                case "Contacto_desc":
                    Alunos = Alunos.OrderByDescending(s => s.Contactos.Contacto);
                    break;

                case "ContactoAlternativo":
                    Alunos = Alunos.OrderBy(s => s.Contactos.ContactoAlternativo);
                    break;

                case "ContactoAlternativo_desc":
                    Alunos = Alunos.OrderByDescending(s => s.Contactos.ContactoAlternativo);
                    break;

                case "Email":
                    Alunos = Alunos.OrderBy(s => s.Contactos.Email);
                    break;

                case "Email_desc":
                    Alunos = Alunos.OrderByDescending(s => s.Contactos.Email);
                    break;

                default:
                    Alunos = Alunos.OrderBy(s => s.IDAluno);
                    break;
            }

            ViewData["Registos"] = Alunos.Count();
            ViewData["PaginaAtual"] = pageNumber;

            //Paginação
            int pageSize = 5;

            double NumeroPaginas = Math.Ceiling((double)Alunos.Count() / pageSize);
            ViewData["NumeroPaginas"] = NumeroPaginas;


            return View(await PaginatedList<Alunos>.CreateAsync(Alunos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [BindProperty]
        public AddAlunoViewModel FileUpload { get; set; }

        [BindProperty]
        public UpdateAlunoViewModel FileUpload2 { get; set; }


        [HttpPost]
        public async Task<IActionResult> Add(AddAlunoViewModel AddAlunoRequest)
        {
            //Validação tipo try catch
            //if (ModelState.IsValid)
            //{

            //Contactos
            var Contactos = new Contactos()
            {
                Email = AddAlunoRequest.Email,
                Contacto = AddAlunoRequest.Contacto,
                ContactoAlternativo = AddAlunoRequest.ContactoAlternativo
            };
            await automixDbContext.Contactos.AddAsync(Contactos);
            await automixDbContext.SaveChangesAsync();

            //Moradas
            var Moradas = new Moradas()
            {
                Morada = AddAlunoRequest.Morada,
                Localidade = AddAlunoRequest.Localidade,
                CodigoPostal = AddAlunoRequest.CodigoPostal

            };
            await automixDbContext.Moradas.AddAsync(Moradas);
            await automixDbContext.SaveChangesAsync();

            //Documentos

            //upload file to folder
            if (FileUpload.Documento.Length > 0)
            {
                using (var stream = new FileStream(Path.Combine(_hostenvironment.WebRootPath, "uploadfiles", FileUpload.Documento.FileName), FileMode.Create))
                {
                    await FileUpload.Documento.CopyToAsync(stream);
                }
            }
            //save image to database.
            using (var memoryStream = new MemoryStream())
            {
                await FileUpload.Documento.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var Documentos = new Documentos()
                    {
                        Documento = memoryStream.ToArray(),
                        Extensao = FileUpload.Documento.FileName.Substring(FileUpload.Documento.FileName.Length - 4),
                        Nome = FileUpload.Documento.FileName
                    };
                    await automixDbContext.Documentos.AddAsync(Documentos);

                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }

            }

            await automixDbContext.SaveChangesAsync();

            //Obter ID do Contacto da tabela Contactos
            var QueryContacto = automixDbContext.Contactos.OrderByDescending(a => a.IDContacto).FirstOrDefault();
            int IDContacto = QueryContacto.IDContacto;

            //Obter ID do Contacto da tabela Contactos
            var QueryMoradas = automixDbContext.Moradas.OrderByDescending(a => a.IDMorada).FirstOrDefault();
            int IDMorada = QueryMoradas.IDMorada;

            //Obter ID do Documento da tabela Documentos
            var QueryDocumento = automixDbContext.Documentos.OrderByDescending(a => a.IDDocumento).FirstOrDefault();
            int IDDocumento = QueryDocumento.IDDocumento;

            //upload file to folder
            if (FileUpload.Foto.Length > 0)
            {
                using (var stream = new FileStream(Path.Combine(_hostenvironment.WebRootPath, "uploadfiles", FileUpload.Foto.FileName), FileMode.Create))
                {
                    await FileUpload.Foto.CopyToAsync(stream);
                }
            }
            //save image to database.
            using (var memoryStream2 = new MemoryStream())
            {
                await FileUpload.Foto.CopyToAsync(memoryStream2);

                // Upload the file if less than 2 MB
                if (memoryStream2.Length < 2097152)
                {
                    var Alunos = new Alunos()
                    {
                        NIFAluno = AddAlunoRequest.NIFAluno,
                        IDContacto = IDContacto,
                        IDMorada = IDMorada,
                        IDDocumento = IDDocumento,
                        Nome = AddAlunoRequest.Nome,
                        Foto = memoryStream2.ToArray(),
                    };

                    await automixDbContext.Alunos.AddAsync(Alunos);

                    await automixDbContext.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("Foto", "A foto é muito grande");
                }


            }
            await automixDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Alunos = await automixDbContext.Alunos.FirstOrDefaultAsync(x => x.IDAluno == id);

            if (Alunos != null)
            {
                var Moradas = await automixDbContext.Moradas.FirstOrDefaultAsync(x => x.IDMorada == Alunos.IDMorada);

                if (Moradas != null)
                {
                    var Contactos = await automixDbContext.Contactos.FirstOrDefaultAsync(x => x.IDContacto == Alunos.IDContacto);

                    if (Contactos != null)
                    {
                        var Documentos = await automixDbContext.Documentos.FirstOrDefaultAsync(x => x.IDDocumento == Alunos.IDDocumento);

                        if (Documentos != null)
                        {
                            if (Alunos.Foto != null)
                            {
                                var stream = new MemoryStream(Alunos.Foto);
                                IFormFile Foto = new FormFile(stream, 0, Alunos.Foto.Length, "Foto", "fileName");

                                var stream2 = new MemoryStream(Documentos.Documento);
                                IFormFile Documento = new FormFile(stream2, 0, Documentos.Documento.Length, "Documento", "fileName");

                                var base64 = Convert.ToBase64String(Alunos.Foto);

                                ViewBag.imgSrc = string.Format("data:image/jpg;base64,{0}", base64);

                                var viewModel = new UpdateAlunoViewModel()
                                {
                                    IDAluno = Alunos.IDAluno,
                                    NIFAluno = Alunos.NIFAluno,
                                    Nome = Alunos.Nome,
                                    Foto = Foto,
                                    Morada = Moradas.Morada,
                                    Localidade = Moradas.Localidade,
                                    CodigoPostal = Moradas.CodigoPostal,
                                    Email = Contactos.Email,
                                    Contacto = Contactos.Contacto,
                                    ContactoAlternativo = Contactos.ContactoAlternativo,
                                    Documento = Documento,
                                    NomeDocumento = Documentos.Nome

                                };

                                return await Task.Run(() => View("Update", viewModel));
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAlunoViewModel model)
        {
            var Alunos = await automixDbContext.Alunos.FindAsync(model.IDAluno);

            if (Alunos != null)
            {
                var Moradas = await automixDbContext.Moradas.FindAsync(Alunos.IDMorada);

                if (Moradas != null)
                {
                    var Contactos = await automixDbContext.Contactos.FindAsync(Alunos.IDContacto);

                    if (Contactos != null)
                    {
                        var Documentos = await automixDbContext.Documentos.FindAsync(Alunos.IDDocumento);

                        if (Documentos != null)
                        {
                            Alunos.NIFAluno = model.NIFAluno;
                            Alunos.Nome = model.Nome;
                            Moradas.Morada = model.Morada;
                            Moradas.Localidade = model.Localidade;
                            Moradas.CodigoPostal = model.CodigoPostal;
                            Contactos.Email = model.Email;
                            Contactos.Contacto = model.Contacto;
                            Contactos.ContactoAlternativo = model.ContactoAlternativo;

                            // DOCUMENTO 

                            if (FileUpload2.Documento != null)
                            {
                                //upload file to folder
                                if (FileUpload2.Documento.Length > 0)
                                {
                                    using (var stream = new FileStream(Path.Combine(_hostenvironment.WebRootPath, "uploadfiles", FileUpload2.Documento.FileName), FileMode.Create))
                                    {
                                        await FileUpload2.Documento.CopyToAsync(stream);
                                    }
                                }
                                //save image to database.
                                using (var memoryStream2 = new MemoryStream())
                                {
                                    await FileUpload2.Documento.CopyToAsync(memoryStream2);

                                    // Upload the file if less than 2 MB
                                    if (memoryStream2.Length < 2097152)
                                    {
                                        Documentos.Documento = memoryStream2.ToArray();
                                        Documentos.Extensao = FileUpload.Documento.FileName.Substring(FileUpload.Documento.FileName.Length - 4);
                                        Documentos.Nome = FileUpload.Documento.FileName;
                                    }
                                    else ModelState.AddModelError("Atestado", "O tamanho do ficheiro é muito grande");

                                }
                            }

                            //FOTO

                            if (FileUpload2.Foto != null)
                            {
                                //upload file to folder
                                if (FileUpload2.Foto.Length > 0)
                                {
                                    using (var stream = new FileStream(Path.Combine(_hostenvironment.WebRootPath, "uploadfiles", FileUpload2.Foto.FileName), FileMode.Create))
                                    {
                                        await FileUpload2.Foto.CopyToAsync(stream);
                                    }
                                }
                                //save image to database.
                                using (var memoryStream2 = new MemoryStream())
                                {
                                    await FileUpload2.Foto.CopyToAsync(memoryStream2);

                                    // Upload the file if less than 2 MB
                                    if (memoryStream2.Length < 2097152) Alunos.Foto = memoryStream2.ToArray(); else ModelState.AddModelError("Foto", "A foto é muito grande");

                                }
                            }

                            await automixDbContext.SaveChangesAsync();

                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var Alunos = await automixDbContext.Alunos.FirstOrDefaultAsync(x => x.IDAluno == id);

            if (Alunos != null)
            {
                var Moradas = await automixDbContext.Moradas.FirstOrDefaultAsync(x => x.IDMorada == Alunos.IDMorada);

                if (Moradas != null)
                {
                    var Contactos = await automixDbContext.Contactos.FirstOrDefaultAsync(x => x.IDContacto == Alunos.IDContacto);

                    if (Contactos != null)
                    {
                        var Documentos = await automixDbContext.Documentos.FirstOrDefaultAsync(x => x.IDDocumento == Alunos.IDDocumento);

                        if (Documentos != null)
                        {
                            if (Alunos.Foto != null)
                            {
                                var stream = new MemoryStream(Alunos.Foto);
                                IFormFile Foto = new FormFile(stream, 0, Alunos.Foto.Length, "Foto", "fileName");

                                var stream2 = new MemoryStream(Documentos.Documento);
                                IFormFile Documento = new FormFile(stream2, 0, Documentos.Documento.Length, "Documento", "fileName");

                                var base64 = Convert.ToBase64String(Alunos.Foto);

                                ViewBag.imgSrc = string.Format("data:image/jpg;base64,{0}", base64);

                                var viewModel = new DetailsAlunosViewModel()
                                {
                                    IDAluno = Alunos.IDAluno,
                                    NIFAluno = Alunos.NIFAluno,
                                    Nome = Alunos.Nome,
                                    Foto = Foto,
                                    Morada = Moradas.Morada,
                                    Localidade = Moradas.Localidade,
                                    CodigoPostal = Moradas.CodigoPostal,
                                    Email = Contactos.Email,
                                    Contacto = Contactos.Contacto,
                                    ContactoAlternativo = Contactos.ContactoAlternativo,
                                    Documento = Documento,
                                    NomeDocumento = Documentos.Nome

                                };

                                return await Task.Run(() => View("Details", viewModel));
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult DownloadFile(string fileName)
        {

            //Download direto
            //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            //Preview pdf 
            var stream = new FileStream(Path.Combine(_hostenvironment.WebRootPath, "uploadfiles", fileName), FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var Alunos = await automixDbContext.Alunos.FindAsync(id);

            if (Alunos != null)
            {
                var Moradas = await automixDbContext.Moradas.FirstOrDefaultAsync(x => x.IDMorada == Alunos.IDMorada);
                    var Contactos = await automixDbContext.Contactos.FirstOrDefaultAsync(x => x.IDContacto == Alunos.IDContacto);
                        var Documentos = await automixDbContext.Documentos.FirstOrDefaultAsync(x => x.IDDocumento == Alunos.IDDocumento);

                            if (Alunos != null)
                            {
                                automixDbContext.Alunos.Remove(Alunos);
                                automixDbContext.Moradas.Remove(Moradas);
                                automixDbContext.Contactos.Remove(Contactos);
                                automixDbContext.Documentos.Remove(Documentos);
                                await automixDbContext.SaveChangesAsync();

                                return RedirectToAction("Index");
                            }

                            return Content("Erro");
                                          
            }
            return Content("Erro");
        }
    }
}

