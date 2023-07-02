using IDM.Data;
using IDM.Models.Domain;
using IDM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace IDM.Controllers
{
    [Authorize(Roles = "Admin")] // LogActionFilters
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdminController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;         
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var employee = await applicationDbContext.Employee.ToListAsync();
            return View(employee);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            try{          
                var employee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    FirstName = addEmployeeRequest.FirstName,
                    LastName = addEmployeeRequest.LastName,
                    email = addEmployeeRequest.email,
                    userPassword = addEmployeeRequest.userPassword,
                    Salary = addEmployeeRequest.Salary,
                    Department = addEmployeeRequest.Department,
                    DateOfBirth = addEmployeeRequest.DateOfBirth,
                    AdhaarNumber = addEmployeeRequest.AdhaarNumber,
                    Address = addEmployeeRequest.Address,
                    MobileNumber = addEmployeeRequest.MobileNumber,
                    Gender = addEmployeeRequest.Gender,
                };
                if(applicationDbContext.Employee != null)
                {
                    await applicationDbContext.Employee.AddAsync(employee);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
            ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Show");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            
            var employee = await applicationDbContext.Employee.FirstOrDefaultAsync(x => x.Id == id);

            try{
                if (employee != null)
                {
                    var viewModel = new UpdateEmployeeViewModel()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        email = employee.email,
                        userPassword = employee.userPassword,
                        Salary = employee.Salary,
                        Department = employee.Department,
                        DateOfBirth = employee.DateOfBirth,
                        AdhaarNumber = employee.AdhaarNumber,
                        Address = employee.Address,
                        MobileNumber = employee.MobileNumber,
                        Gender = employee.Gender,


                    };
                    return await Task.Run(() => View("Update", viewModel));
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
            ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeViewModel model)
        {
            var employee = await applicationDbContext.Employee.FindAsync(model.Id);

            try{
                if (employee != null)
                {
                    employee.FirstName = model.FirstName;
                    employee.LastName = model.LastName;
                    employee.email = model.email;
                    employee.userPassword = model.userPassword;
                    employee.Salary = model.Salary;
                    employee.DateOfBirth = model.DateOfBirth;
                    employee.Department = model.Department;
                    employee.AdhaarNumber = model.AdhaarNumber;
                    employee.Address = model.Address;
                    employee.MobileNumber = model.MobileNumber;
                    employee.Gender = model.Gender;

                    await applicationDbContext.SaveChangesAsync();

                    return RedirectToAction("Show");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
            ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        
            return RedirectToAction("Show");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await applicationDbContext.Employee.FindAsync(model.Id);
            try{
                if (employee != null)
                {
                    applicationDbContext.Employee.Remove(employee);

                    await applicationDbContext.SaveChangesAsync();
                    return RedirectToAction("Show");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(String.Empty, $"Something Went Wrong{exception.Message}");
            }
            ModelState.AddModelError(String.Empty, $"Something Went Wrong Invalid Model");
        
            return RedirectToAction("Show");
        }
    }
}
