
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using employee_management_agile.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace employee_management_agile.Controllers

{
    [Authorize]
    public class EmpController : Controller

    {

        private readonly EmpDbContext _context;


        public EmpController(EmpDbContext context)

        {

            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Employee")]
        [Route("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()

        {

            var employees = await _context.EmployeesTable.ToListAsync();

            return View(employees);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Employee")]
        [Route("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)

        {

            var employee = await _context.EmployeesTable.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)

            {

                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        [Route("CreateEmployee")]
        public IActionResult CreateEmployee()

        {

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        [Route("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(EmpModel employee)

        {

            if (ModelState.IsValid)

            {

                await _context.EmployeesTable.AddAsync(employee);

                await _context.SaveChangesAsync();

                return RedirectToAction("GetAllEmployees");
            }

            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        [Route("EditEmployee/{id}")]
        public IActionResult EditEmployee(int id)

        {

            var employee = _context.EmployeesTable.FirstOrDefault(e => e.Id == id);

            if (employee == null)

            {

                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        [Route("EditEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(EmpModel employee)

        {
            if (ModelState.IsValid)

            {
                var existingEmployee = await _context.EmployeesTable.FirstOrDefaultAsync(e => e.Id == employee.Id);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.Name = employee.Name;
                existingEmployee.Department = employee.Department;
                existingEmployee.Position = employee.Position;
                existingEmployee.Salary = employee.Salary;

                _context.EmployeesTable.Update(existingEmployee);

                await _context.SaveChangesAsync();

                return RedirectToAction("GetAllEmployees");
            }

            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)

        {

            var employee = _context.EmployeesTable.FirstOrDefault(e => e.Id == id);

            if (employee == null)

            {

                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmedDeleteEmployee(int id)

        {

            var employee = await _context.EmployeesTable.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)

            {

                return NotFound();
            }

            _context.EmployeesTable.Remove(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllEmployees");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        [Route("RoleUpdate/{id}")]
        public IActionResult RoleUpdate(int id)
        {
            var employee = _context.EmployeesTable.FirstOrDefault(e => e.Id == id);

            if (employee == null)

            {

                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        [Route("RoleUpdate/{id}")]
        public async Task<IActionResult> RoleUpdate(EmpModel employee)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = await _context.EmployeesTable.FirstOrDefaultAsync(e => e.Id == employee.Id);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.Role = employee.Role;

                _context.EmployeesTable.Update(existingEmployee);

                await _context.SaveChangesAsync();

                return RedirectToAction("GetAllEmployees");
            }

            return View(employee);
        }

    }
}