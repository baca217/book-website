using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListMVC.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ListMVC.Pages.BookList
{
    public class UpsertModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; }


        public async Task<IActionResult> OnGet(int? id) //id can be null for creation
        {
            Book = new Book();
            if(id == null)
            {
                return Page(); //create
            }

            Book = await _db.Book.FirstOrDefaultAsync(u => u.Id == id); //update
            if(Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    _db.Book.Add(Book);
                }
                else
                {
                    _db.Book.Update(Book); //best for when updating all properties of a book
                }

                await _db.SaveChangesAsync();

                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}