using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Data;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<UserModel> userManager;
        public TaskController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            this.context = context;
            this.userManager = userManager;
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
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) 
            {
                return NotFound();
            }
            return View(task);
        }
        //Create Task and log errors 
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title,Description,Deadline,Status,ProjectId")] TaskModel task)
        {
            if (ModelState.IsValid)
            {
                context.TaskModels.Add(task);
                await context.SaveChangesAsync();
                return RedirectToAction("Details", "Project", new { id = task?.ProjectId });
            }
            else
            {
                // Log ModelState errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            ViewBag.ProjectId = task.ProjectId;
            return RedirectToAction("Details", "Project", new { id = task?.ProjectId });
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
            ViewBag.ProjectId = task.ProjectId;
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
        [HttpPost]
        public async Task<IActionResult> AssignUser(int taskId, string userId)
        {
            // Проверка на существование пользователя в задаче
            var existingAssignment = await context.TaskUsers
                .FirstOrDefaultAsync(tu => tu.TaskId == taskId && tu.UserId == userId);

            if (existingAssignment != null)
            {
                return Json(new { success = false, message = "User is already assigned to this task." });
            }

            
            var taskUser = new TaskUser
            {
                TaskId = taskId,
                UserId = userId
            };

            context.TaskUsers.Add(taskUser);
            await context.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpGet]
        public IActionResult DeleteTaskModal(int taskId, string taskTitle)
        {
            return ViewComponent("DeleteTask", new { taskId = taskId, taskTitle = taskTitle });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await context.TaskModels.FindAsync(id);
            if (task != null)
            {
                context.TaskModels.Remove(task);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Project", new { id = task?.ProjectId });
        }
        //Comments section
        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string Content)
        {
            var userId = userManager.GetUserId(User);
            var comment = new CommentModel
            { 
                Content = Content,
                TaskModelId = taskId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };
            Console.WriteLine(comment);
            Console.WriteLine(userId + "UserId");
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetComments(int taskId)
        {
            var task = await context.TaskModels
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            return PartialView("CommentsPartial", task);
        }
        private bool TaskExist(int id)
        {
            return context.TaskModels.Any(t => t.Id == id);
        }
    }
}
