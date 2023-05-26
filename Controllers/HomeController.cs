﻿using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    { private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context= context;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e=>e.Position).ToListAsync();
            return View(employees);
        }

        
    }
}