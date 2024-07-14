using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Data;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<UserModel> userManager;
        public ProjectController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var projects = await context.Projects
                                        .Where(p => p.OwnerId == userId)
                                        .ToListAsync();
            return View(projects);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] ProjectModel project)
        {
            if (ModelState.IsValid)
            {
                project.OwnerId = userManager.GetUserId(User);/* current user id */;
                context.Add(project);
                Console.WriteLine("Ownerid: "+project.OwnerId);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(project);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = await context.Projects.FindAsync(id);
            if(project == null) 
            {
                return NotFound();
            }
            return View(project);  
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ProjectModel project)
        {
            if(id!=project.Id) 
            {
                return NotFound();
            }
            if (ModelState.IsValid) 
            {
                try 
                {
                    context.Update(project);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await context.Projects.FindAsync(id);
            context.Projects.Remove(project);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await context.Projects
                .Include(p => p.TaskModels)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        private bool ProjectExists(int id)
        {
            return context.Projects.Any(e => e.Id == id);
        }
    }

}
