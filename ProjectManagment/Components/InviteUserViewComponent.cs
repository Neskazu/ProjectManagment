using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Data;
using ProjectManagment.Models;

namespace ProjectManagment.Components
{
    public class InviteUserViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(int projectId, string projectName)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.ProjectName = projectName;
            return View();
        }
    }
}
