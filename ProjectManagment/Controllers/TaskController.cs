using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Data;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext context;
        public TaskController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //task details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            var task = await context.TaskModels
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) 
            {
                return NotFound();
            }
            return View(task);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title,Description,Deadline,ProjectId,AssignedUserId")] TaskModel task)
        {
            if (ModelState.IsValid) 
            {
                task.Status = Models.TaskStatus.New;
                context.Add(task);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(task);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) 
            {
                return NotFound();
            }
            var task = await context.TaskModels.FindAsync(id);
            if(task == null) 
            {
                return NotFound();
            }
            return View(task);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Desctiption,Deadline,Status,AssingnedUserId")] TaskModel task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(task);
                    await context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExist(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(task);
        }

        private bool TaskExist(int id)
        {
            return context.TaskModels.Any(t => t.Id == id);
        }
    }
}
