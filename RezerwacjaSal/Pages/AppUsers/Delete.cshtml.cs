﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RezerwacjaSal.Data;
using RezerwacjaSal.Models;

namespace RezerwacjaSal.Pages.AppUsers
{
    [Authorize(Roles = "administrator")]
    public class DeleteModel : PageModel
    {
        private readonly RezerwacjaSal.Data.RezerwacjaSalContext _context;

        public DeleteModel(RezerwacjaSal.Data.RezerwacjaSalContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public string ErrorMessage { get; set; }
        public string SortOrderRoute { get; set; }
        public string CurrentFilterRoute { get; set; }
        public string SearchStringRoute { get; set; }
        public int? PageIndexRoute { get; set; }
        public int? PageSizeRoute { get; set; }
        public string Role { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            if (id == null)
                return NotFound();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            ApplicationUser = await _context.AppUsers
                .Include(e => e.Department) 
                .AsNoTracking()               
                .FirstOrDefaultAsync(m => m.Id == id);  

            if (ApplicationUser == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(ApplicationUser);
            Role = roles.First();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string sortOrder, string currentFilter, string searchString, int? pageIndex, int? pageSize)
        {
            if (id == null)
                return NotFound();

            SortOrderRoute = sortOrder;
            CurrentFilterRoute = currentFilter;
            SearchStringRoute = searchString;
            PageIndexRoute = pageIndex;
            PageSizeRoute = pageSize;

            var appUser = await _context.AppUsers
                .Include(e => e.Department)
                .AsNoTracking()            
                .FirstOrDefaultAsync(m => m.Id == id); 

            if (appUser == null)
                return NotFound();

            try
            {
                _context.AppUsers.Remove(appUser);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { id = id, saveChangesError = true });
            }
        }
    }
}
