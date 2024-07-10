using Microsoft.AspNetCore.Mvc;

namespace ProjectManagment.Components
{
    public class DeleteTaskViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int taskId, string taskTitle) 
        {
            ViewBag.TaskId = taskId;
            ViewBag.TaskTitle = taskTitle;
            return View(); 
        }
    }
}
