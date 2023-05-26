using Mamba.Areas.AdminPanel.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Mamba.Utiliters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e => e.Position).ToListAsync();
            return View(employees);
        }
        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM createEmployeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = _context.Positions;
                return View();
            }
            if (!createEmployeeVM.Photo.CheckFileType(createEmployeeVM.Photo.ContentType))
            {
                ViewBag.Positions = _context.Positions;
                ModelState.AddModelError("Photo", "Faylin formati uygun deyil");
                return View();
            }
            if (!createEmployeeVM.Photo.CheckFileSize(200))
            {
                ViewBag.Positions = _context.Positions;
                ModelState.AddModelError("Photo", "Faylin hecmi boyukdur");
                return View();
            }
            bool result = await _context.Positions.AnyAsync(p => p.Id == createEmployeeVM.PositionId);
            if (!result)
            {
                ViewBag.Positions = _context.Positions;
                ModelState.AddModelError("PositionId", "Bele id'li position yoxdur");
                return View();
            }
            Employee employee = new Employee()
            {
                Name = createEmployeeVM.Name,
                Description = createEmployeeVM.Description,
                PositionId = createEmployeeVM.PositionId,
                Image = await createEmployeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team")
            };
            employee.Image = await createEmployeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (employee == null) return NotFound();
            UpdateEmployeeVM updateEmployeeVM = new UpdateEmployeeVM()
            {
                Name = employee.Name,
                Description = employee.Description,
                PositionId = employee.PositionId,
                Image = employee.Image
            };
            ViewBag.Positions = _context.Positions;
            return View(updateEmployeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateEmployeeVM updateEmployeeVM)
        {
            if (id == null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (employee == null) return NotFound();
            bool result = await _context.Positions.AnyAsync(p => p.Id == updateEmployeeVM.PositionId);

            if (!result)
            {
                ViewBag.Positions = _context.Positions;
                ModelState.AddModelError("PositionId", "Bele id'li position yoxdur");
                updateEmployeeVM.Image = employee.Image;
                return View(updateEmployeeVM);
            }
            if (updateEmployeeVM == null)
            {
                if (!updateEmployeeVM.Photo.CheckFileType(updateEmployeeVM.Photo.ContentType))
                {
                    ViewBag.Positions = _context.Positions;
                    ModelState.AddModelError("Photo", "Faylin formati uygun deyil");
                    updateEmployeeVM.Image = employee.Image;
                    return View(updateEmployeeVM);
                }
                if (!updateEmployeeVM.Photo.CheckFileSize(200))
                {
                    ViewBag.Positions = _context.Positions;
                    ModelState.AddModelError("Photo", "Faylin hecmi boyukdur");
                    updateEmployeeVM.Image = employee.Image;
                    return View(updateEmployeeVM);
                }
                employee.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
                employee.Image = await updateEmployeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            }
            employee.Name = updateEmployeeVM.Name;
            employee.Description = updateEmployeeVM.Description;
            employee.PositionId = updateEmployeeVM.PositionId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (employee == null) return NotFound();
            employee.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
