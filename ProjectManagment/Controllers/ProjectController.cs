using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Data;

namespace ProjectManagment.Controllers
{
    public class ProjectController : Controller
    {

        private readonly ApplicationDbContext context;
        public ProjectController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var projects = await context.Projects.Include(p=>p.Owner).ToListAsync();
            return View(projects);
        }
    }
}
