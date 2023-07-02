using IDM.Data;
using IDM.Models.Domain;
using IDM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IDM.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ClientController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;         
        }

        public IActionResult Index(Employee employee)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> showProfile(Guid id)
        {
            var employee = await applicationDbContext.Employee.ToListAsync();
            return View(employee);
        }

        public ActionResult contact(){
            return View();
        }

    }
}
