using Automix.Data;
using Automix.Models;
using Automix.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automix.Controllers
{
    public class PercursosAlunosController : Controller
    {
        private readonly AutomixDbContext automixDbContext;

        public PercursosAlunosController(AutomixDbContext AutomixDbContext)
        {
            automixDbContext = AutomixDbContext;

        }

        public async Task<IActionResult> Index(string sortOrder = "", string currentFilter = "", string searchString = null, int? pageNumber = 1)
        {

            ViewData["Modulo"] = "PercursosAlunos";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IDPercursosAluno"] = sortOrder == "IDPercursosAluno_desc" ? "IDPercursosAluno" : "IDPercursosAluno_desc";
            ViewData["Nome"] = sortOrder == "Nome" ? "Nome_desc" : "Nome";
            ViewData["NIFAluno"] = sortOrder == "NIFAluno" ? "NIFAluno_desc" : "NIFAluno";
            ViewData["Percurso"] = sortOrder == "Percurso" ? "Percurso_desc" : "Percurso";

            //Paginação
            if (searchString != null) pageNumber = 1; else searchString = currentFilter;

            //Filtro
            ViewData["Filtro"] = searchString;

            var PercursosAlunos = from s in automixDbContext.PercursosAlunos.Include(x => x.Alunos).Include(x => x.Percursos) select s;

            //Filtrar tmb faz parte este if (para adicionar mais strings no futuro fazer ex:s => s.Primeiro campo.Contains(searchString) || s.Segundo Campo.Contains(searchString)); e por ai vai
            if (!String.IsNullOrEmpty(searchString)) PercursosAlunos = PercursosAlunos.Where(s => s.Alunos.Nome.Contains(searchString));

            switch (sortOrder)
            {
                case "IDPercursosAluno":
                    PercursosAlunos = PercursosAlunos.OrderByDescending(s => s.IDPercursoAluno);
                    break;

                case "PercursosAlunos_desc":
                    PercursosAlunos = PercursosAlunos.OrderByDescending(s => s.IDPercursoAluno);
                    break;

                case "Nome":
                    PercursosAlunos = PercursosAlunos.OrderBy(s => s.Alunos.Nome);
                    break;

                case "Nome_desc":
                    PercursosAlunos = PercursosAlunos.OrderByDescending(s => s.Alunos.Nome);
                    break;

                case "NIFAluno":
                    PercursosAlunos = PercursosAlunos.OrderBy(s => s.Alunos.NIFAluno);
                    break;

                case "NIFAluno_desc":
                    PercursosAlunos = PercursosAlunos.OrderByDescending(s => s.Alunos.NIFAluno);
                    break;

                case "Percurso":
                    PercursosAlunos = PercursosAlunos.OrderBy(s => s.Percursos.Nome);
                    break;

                case "Percurso_desc":
                    PercursosAlunos = PercursosAlunos.OrderByDescending(s => s.Percursos.Nome);
                    break;

                default:
                    PercursosAlunos = PercursosAlunos.OrderBy(s => s.IDPercursoAluno);
                    break;
            }

            ViewData["Registos"] = PercursosAlunos.Count();
            ViewData["PaginaAtual"] = pageNumber;

            //Paginação
            int pageSize = 5;

            double NumeroPaginas = Math.Ceiling((double)PercursosAlunos.Count() / pageSize);
            ViewData["NumeroPaginas"] = NumeroPaginas;


            return View(await PaginatedList<PercursosAlunos>.CreateAsync(PercursosAlunos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Alunos = automixDbContext.Alunos;
            ViewBag.Percursos = automixDbContext.Percursos;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPercursosAlunosViewModel AddPercursosAlunosRequest)
        {
            //Validação tipo try catch
            //if (ModelState.IsValid)
            //{

            //PercursosAlunos
            var PercursosAlunos = new PercursosAlunos()
            {
                IDAluno = AddPercursosAlunosRequest.IDAluno,
                IDPercurso = AddPercursosAlunosRequest.IDPercurso,
            };
            await automixDbContext.PercursosAlunos.AddAsync(PercursosAlunos);

            await automixDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var PercursosAlunos = await automixDbContext.PercursosAlunos.FirstOrDefaultAsync(x => x.IDPercursoAluno == id);

            if (PercursosAlunos != null)
            {
                var viewModel = new UpdatePercursosAlunosViewModel()
                {
                    IDPercursoAluno = id,
                    IDAluno = PercursosAlunos.IDAluno,
                    IDPercurso = PercursosAlunos.IDPercurso
                };

                ViewBag.Alunos = automixDbContext.Alunos;
                ViewBag.Percursos = automixDbContext.Percursos;

                return await Task.Run(() => View("Update", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePercursosAlunosViewModel model)
        {
            var PercursosAlunos = await automixDbContext.PercursosAlunos.FindAsync(model.IDPercursoAluno);

            if (PercursosAlunos != null)
            {
                PercursosAlunos.IDAluno = model.IDAluno;
                PercursosAlunos.IDPercurso = model.IDPercurso;

                await automixDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var PercursosAlunos = await automixDbContext.PercursosAlunos.FirstOrDefaultAsync(x => x.IDPercursoAluno == id);

            if (PercursosAlunos != null)
            {
                var viewModel = new DetailsPercursosAlunosViewModel()
                {
                    IDPercursoAluno = id,
                    IDAluno = PercursosAlunos.IDAluno,
                    IDPercurso = PercursosAlunos.IDPercurso
                };

                ViewBag.Alunos = automixDbContext.Alunos;
                ViewBag.Percursos = automixDbContext.Percursos;

                return await Task.Run(() => View("Details", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var PercursosAlunos = await automixDbContext.PercursosAlunos.FindAsync(id);

            if (PercursosAlunos != null)
            {
                automixDbContext.PercursosAlunos.Remove(PercursosAlunos);
                await automixDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return Content("Erro");

        }
    }
}
