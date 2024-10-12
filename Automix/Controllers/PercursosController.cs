using Automix.Data;
using Automix.Models;
using Automix.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Automix.Controllers
{
    public class PercursosController : Controller
    {
        private readonly AutomixDbContext automixDbContext;

        public PercursosController(AutomixDbContext AutomixDbContext)
        {
            automixDbContext = AutomixDbContext;
        }

        //Carregar Percursos pa tabela
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder = "", string currentFilter = "", string searchString = null, int? pageNumber = 1)
        {
            ViewData["Modulo"] = "Percursos";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IDPercurso"] = sortOrder == "IDPercurso_desc" ? "IDPercurso" : "IDPercurso_desc";
            ViewData["Nome"] = sortOrder == "Nome_desc" ? "Nome" : "Nome_desc";
            ViewData["Status"] = sortOrder == "Status" ? "Status_desc" : "Status";

            //Paginação
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Filtro
            ViewData["Filtro"] = searchString;

            var Percursos = from s in automixDbContext.Percursos
                           select s;

            //Filtrar tmb faz parte este if (para adicionar mais strings no futuro fazer ex:s => s.Primeiro campo.Contains(searchString) || s.Segundo Campo.Contains(searchString)); e por ai vai
            if (!String.IsNullOrEmpty(searchString))
            {
                Percursos = Percursos.Where(s => s.Nome.Contains(searchString));
            }

            switch (sortOrder)
            {

                case "IDPercurso":
                    Percursos = Percursos.OrderBy(s => s.IDPercurso);
                    break;

                case "IDPercurso_desc":
                    Percursos = Percursos.OrderByDescending(s => s.IDPercurso);
                    break;

                case "Nome":
                    Percursos = Percursos.OrderBy(s => s.Nome);
                    break;

                case "Nome_desc":
                    Percursos = Percursos.OrderByDescending(s => s.Nome);
                    break;

                case "Status":
                    Percursos = Percursos.OrderBy(s => s.Status);
                    break;

                case "Status_desc":
                    Percursos = Percursos.OrderByDescending(s => s.Status);
                    break;

                default:
                    Percursos = Percursos.OrderBy(s => s.IDPercurso);
                    break;
            }

            ViewData["Registos"] = Percursos.Count();
            ViewData["PaginaAtual"] = pageNumber;

            //Paginação
            int pageSize = 5;

            double NumeroPaginas = Math.Ceiling((double) Percursos.Count() / pageSize);
            ViewData["NumeroPaginas"] = NumeroPaginas;

            return View(await PaginatedList<Percursos>.CreateAsync(Percursos.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPercursoViewModel AddPercursoRequest)
        {
            //Validação tipo try catch
            //if (ModelState.IsValid)
            //{

            var Percurso = new Percursos()
                {
                    //IDPercurso = Guid.NewGuid(),
                    Nome = AddPercursoRequest.Nome,
                    Status = AddPercursoRequest.Status
                };

                await automixDbContext.Percursos.AddAsync(Percurso);
                await automixDbContext.SaveChangesAsync();

            //}

            return RedirectToAction("Index");
        }

       [HttpGet]
       public async Task<IActionResult> Update(int id)
        {
           var Percurso = await automixDbContext.Percursos.FirstOrDefaultAsync(x => x.IDPercurso == id);

            if(Percurso != null)
            {
                var viewModel = new UpdatePercursoViewModel()
                {
                    IDPercurso = Percurso.IDPercurso,
                    Nome = Percurso.Nome,
                    Status = Percurso.Status
                };

                return await Task.Run(() => View("View", viewModel));
            }
          
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePercursoViewModel model)
        {
            var percurso = await automixDbContext.Percursos.FindAsync(model.IDPercurso);

            if (percurso != null)
            {
                percurso.Nome = model.Nome;
                percurso.Status = model.Status;
              
                await automixDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> Delete(UpdatePercursoViewModel model)
        //{
        //    var Percursos = await automixDbContext.Percursos.FindAsync(model.IDPercurso);

        //    if(Percursos != null)
        //    {
        //        automixDbContext.Percursos.Remove(Percursos);
        //        await automixDbContext.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");

        //}

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var Percursos = await automixDbContext.Percursos.FindAsync(id);

            if (Percursos != null)
            {
                automixDbContext.Percursos.Remove(Percursos);
                await automixDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }
    }
}
