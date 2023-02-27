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
    public class RecipiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IKitchenService _kitchenService;
        private readonly UserManager<RBUser> _userManager;
        private readonly IImageService _imageService;

        public RecipiesController(ApplicationDbContext context, IKitchenService kitchenService, UserManager<RBUser> userManager, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
            _kitchenService = kitchenService;   
        }

        // GET: Recipies
        public async Task<IActionResult> Index()
        {
           IEnumerable<Recipie> recipies = await _kitchenService.GetRecipiesAsync();
            return View(recipies);
        }

        // GET: Recipies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipie? recipie = await _kitchenService.GetRecipieByIdAsync(id.Value);


            if (recipie == null)
            {
                return NotFound();
            }

            return View(recipie);
        }

        // GET: Recipies/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable<Ingredient> ingredients = await _kitchenService.GetIngredientsAsync();

            ViewData["IngredientList"] = new MultiSelectList(ingredients, "Id", "Name");

            ViewData["RecipieBookId"] = new SelectList(_context.RecipieBooks, "Id", "Title");
            return View();
        }


        // POST: Recipies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Created,ImageFile,ImageData,ImageType,RecipieBookId")] Recipie recipie, IEnumerable<int> selectedIngredients)
        {



            


            if (ModelState.IsValid)
            {

                if (recipie.ImageFile != null)
                {
                    recipie.ImageData = await _imageService.ConvertFileToByteArrayAsync(recipie.ImageFile);
                    recipie.ImageType = recipie.ImageFile.ContentType;
                }


                await _kitchenService.CreateRecipie(recipie);

                await _kitchenService.AddIngredientToRecipe(selectedIngredients, recipie.Id);

      
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipieBookId"] = new SelectList(_context.RecipieBooks, "Id", "Title", recipie.RecipieBookId);
            return View(recipie);
        }

        // GET: Recipies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipie? recipie = await _kitchenService.GetRecipieByIdAsync(id.Value);
            if (recipie == null)
            {
                return NotFound();
            }
            ViewData["RecipieBookId"] = new SelectList(_context.RecipieBooks, "Id", "Title", recipie.RecipieBookId);
            return View(recipie);
        }

        // POST: Recipies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,ImageFile,ImageData,ImageType,RecipieBookId")] Recipie recipie)
        {
            if (id != recipie.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Created");
            if (ModelState.IsValid)
            {
                try
                {

                    if (recipie.ImageFile != null)
                    {
                        recipie.ImageData = await _imageService.ConvertFileToByteArrayAsync(recipie.ImageFile);
                        recipie.ImageType = recipie.ImageFile.ContentType;
                    }


                    recipie.Created = DataUtility.GetPostGresDate(DateTime.UtcNow);

                    await _kitchenService.UpdateRecipie(recipie);  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipieExists(recipie.Id))
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
            ViewData["RecipieBookId"] = new SelectList(_context.RecipieBooks, "Id", "Title", recipie.RecipieBookId);
            return View(recipie);
        }

        // GET: Recipies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipie? recipie = await _kitchenService.GetRecipieByIdAsync(id.Value);
            if (recipie == null)
            {
                return NotFound();
            }

            return View(recipie);
        }

        // POST: Recipies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Recipies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recipies'  is null.");
            }
            Recipie? recipie = await _kitchenService.GetRecipieByIdAsync(id.Value);
            if (recipie != null)
            {
                await _kitchenService.DeleteRecipieAsync(recipie);  
            }
            
          
            return RedirectToAction(nameof(Index));
        }

        private bool RecipieExists(int id)
        {
          return (_context.Recipies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
