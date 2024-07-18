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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var projects = await context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectUsers)
                .Where(p => p.ProjectUsers.Any(pu => pu.UserId == userId))
                .ToListAsync();
            return View(projects);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] ProjectModel project)
        {
            if(ModelState.IsValid)
            {
                context.Add(project);
                await context.SaveChangesAsync();
                var userId = userManager.GetUserId(User);
                var projectUser = new ProjectUser
                {
                    ProjectId = project.Id,
                    UserId = userId,
                    Role = ProjectRole.Owner

                };
                context.Add(projectUser);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            var project = await GetProjectIfUserHasAccess(id);
            if (project == null || !IsUserOwner(project)) 
            {
                return Forbid();
            }
            Console.WriteLine("Onwer " + !IsUserOwner(project));
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ProjectModel project)
        {
            if(id != project.Id) 
            {
                return NotFound();
            }
            var existingProject = await GetProjectIfUserHasAccess(id);
            if (existingProject == null|| !IsUserOwner(existingProject)) 
            {
                return Forbid();
            }
            if (ModelState.IsValid) 
            {
                try 
                {
                    existingProject.Name = project.Name;
                    existingProject.Description = project.Description;
                    context.Update(existingProject);
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

            var project = await GetProjectIfUserHasAccess(id);
            if (project == null||!IsUserOwner(project))
            {
                return Forbid();
            }

            return View(project);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await GetProjectIfUserHasAccess(id);
            if (project == null || !IsUserOwner(project))
            {
                return Forbid();
            }
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
            var project = await GetProjectIfUserHasAccess(id.Value);
            if (project == null)
            {
                return Forbid();
            }
            project = await context.Projects
                .Include(p => p.TaskModels)
                .Include(p => p.ProjectUsers).ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(project);
        }
        [HttpGet]
        public IActionResult InviteUserModal(int projectId, string projectName)
        {
            return ViewComponent("InviteUser", new {projectId = projectId, projectName = projectName });
        }
        [HttpPost]
        public async Task<IActionResult> InviteUser(int projectId, string email, ProjectRole role)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=>u.Email == email);
            if(user == null) 
            {
                return NotFound("No such user");
            }
            var existUser = context.ProjectUsers.FirstOrDefault(u=>u.UserId==user.Id);
            if (existUser!=null)
            {
                return NotFound("Already in project");
            }
            var projectUser = new ProjectUser
            {
                ProjectId = projectId,
                UserId = user.Id,
                Role = role
            };
            context.ProjectUsers.Add(projectUser);
            await context.SaveChangesAsync();
            return View("Index");
        }

        private bool ProjectExists(int id)
        {
            return context.Projects.Any(e => e.Id == id);
        }
        private async Task<ProjectModel?> GetProjectIfUserHasAccess(int? projectId)
        {
            var userId = userManager.GetUserId(User);
            var project = await context.Projects
                .Include (p => p.ProjectUsers)
                .FirstOrDefaultAsync(p=> p.Id == projectId);
            if (project == null) 
            {
                return null;
            }
            if (project.ProjectUsers.Any(pu => pu.UserId == userId))
            {
                return project;
            }
            return null;
        }
        private bool IsUserOwner(ProjectModel project)
        {
            var userId = userManager.GetUserId(User);
            return project.ProjectUsers.Any(pu => pu.UserId == userId && pu.Role == ProjectRole.Owner);
        }
      
    }

}
