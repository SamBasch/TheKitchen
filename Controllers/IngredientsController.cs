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
using TheKitchen.Services;
using TheKitchen.Services.Interfaces;

namespace TheKitchen.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IKitchenService _kitchenService;
        private readonly UserManager<RBUser> _userManager;
     

        public IngredientsController(ApplicationDbContext context, IKitchenService kitchenService, UserManager<RBUser> userManager)
        {
            _context = context;
               _userManager = userManager;
            _kitchenService = kitchenService;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
              IEnumerable<Ingredient> ingredients = await _kitchenService.GetIngredientsAsync();    
            return View(ingredients);
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ingredient ingredient = await _kitchenService.GetIngredientByIdAsync(id.Value);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Measure")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await _kitchenService.CreateIngredient(ingredient);    
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            Ingredient ingredient = await _kitchenService.GetIngredientByIdAsync(id.Value);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Measure")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _kitchenService.UpdateIngredient(ingredient);    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.Id))
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
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ingredient ingredient = await _kitchenService.GetIngredientByIdAsync(id.Value);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Ingredient ingredient = await _kitchenService.GetIngredientByIdAsync(id.Value);
            if (ingredient != null)
            {
                await _kitchenService.DeleteIngredientAsync(ingredient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
          return (_context.Ingredients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
