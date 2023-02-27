using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheKitchen.Data;
using TheKitchen.Models;
using TheKitchen.Services.Interfaces;

namespace TheKitchen.Controllers
{
    public class RecipieBooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IKitchenService _kitchenService;
        private readonly UserManager<RBUser> _userManager;

        public RecipieBooksController(ApplicationDbContext context, IKitchenService kitchenService, UserManager<RBUser> userManager)
        {
            _context = context;
            _kitchenService = kitchenService;
            _userManager = userManager;
        }

        // GET: RecipieBooks
        public async Task<IActionResult> Index()
        {

            string userId = _userManager.GetUserId(User)!;

            IEnumerable<RecipieBook> recipieBooks = await _kitchenService.GetRecipieBooksAsync(userId);

           
            return View(recipieBooks);
        }

        // GET: RecipieBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User)!;

            RecipieBook? recipieBook = await _kitchenService.GetRecipieBookAsync(id.Value, userId);

            //var recipieBook = await _context.RecipieBooks
            //    .Include(r => r.User)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (recipieBook == null)
            {
                return NotFound();
            }

            return View(recipieBook);
        }

        // GET: RecipieBooks/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: RecipieBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,UserId")] RecipieBook recipeBook)
        {

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {

                recipeBook.UserId = _userManager.GetUserId(User);


                await _kitchenService.CreateRecipieBookAsync(recipeBook);


                return RedirectToAction(nameof(Index));
            }
          
            return View(recipeBook);
        }

        // GET: RecipieBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User)!;

            RecipieBook? recipieBook = await _kitchenService.GetRecipieBookAsync(id.Value, userId);


            if (recipieBook == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipieBook.UserId);
            return View(recipieBook);
        }

        // POST: RecipieBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UserId")] RecipieBook recipieBook)
        {
            if (id != recipieBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _kitchenService.UpdateRecipieBook(recipieBook);   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipieBookExists(recipieBook.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipieBook.UserId);
            return View(recipieBook);
        }

        // GET: RecipieBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            string userId = _userManager.GetUserId(User)!;

            RecipieBook? recipieBook = await _kitchenService.GetRecipieBookAsync(id.Value, userId);

            if (recipieBook == null)
            {
                return NotFound();
            }

            return View(recipieBook);
        }

        // POST: RecipieBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string userId = _userManager.GetUserId(User)!;

            RecipieBook? recipieBook = await _kitchenService.GetRecipieBookAsync(id.Value, userId);

            if (recipieBook != null)
            {
                await _kitchenService.DeleteBookAsync(recipieBook); 
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipieBookExists(int id)
        {
          return (_context.RecipieBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
