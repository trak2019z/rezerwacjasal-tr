﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;


namespace RezerwacjaSal.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }
        public IEnumerable<Pearson> People { get; set; }
        public async Task<IActionResult> OnGetAsync(int departmentid)
        {
            Department = await _context.Departments
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.DepartmentID == departmentid);

            People = await _context.People
                .AsNoTracking()
                .ToListAsync();

            if (Department == null || People == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int departmentid)
        {
            Department = await _context.Departments.FindAsync(departmentid);

            if (Department != null)
            {
                _context.Departments.Remove(Department);
                await _context.SaveChangesAsync();
            }
            else
                return NotFound();

            return RedirectToPage("./Index");
        }
    }
}
