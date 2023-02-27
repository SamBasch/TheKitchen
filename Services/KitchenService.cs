using Microsoft.EntityFrameworkCore;
using TheKitchen.Data;
using TheKitchen.Models;
using TheKitchen.Services.Interfaces;

namespace TheKitchen.Services
{
    public class KitchenService : IKitchenService
    {
		private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;   


        public KitchenService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }


        public async Task<IEnumerable<RecipieBook>> GetRecipieBooksAsync(string userId)
        {
            try
            {
                IEnumerable<RecipieBook> recipieBooks = await _context.RecipieBooks.Where(rb => rb.UserId == userId).Include(rb => rb.Recipies).ToListAsync();

                return recipieBooks;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<RecipieBook> GetRecipieBookAsync(int Id, string userId)
        {


            try
            {

                RecipieBook? recipieBook = await _context.RecipieBooks.Include(rb => rb.Recipies).FirstOrDefaultAsync(rb => rb.UserId == userId && rb.UserId == userId);
                return recipieBook;

            }
            catch (Exception)
            {

                throw;
            }
           
        }



        public async Task CreateRecipieBookAsync(RecipieBook recipeBook)
        {

            try
            {

                await _context.AddAsync(recipeBook);

                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }


        }



        public async Task RemoveAllRecipiesFromBook(int bookId, string userId)
        {


            RecipieBook? recipeBook = await _context.RecipieBooks.Include(rb => rb.Recipies).FirstOrDefaultAsync(rb => rb.UserId == userId && rb.Id == bookId);



            recipeBook!.Recipies.Clear();
            _context.Update(recipeBook);
            await _context.SaveChangesAsync();  

        }


        public async Task UpdateRecipieBook(RecipieBook recipeBook)
        {

            try
            {
                _context.Update(recipeBook);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }



        }


        public async Task DeleteBookAsync(RecipieBook recipeBook)
        {
            try
            {

                _context.Remove(recipeBook);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }


        }



        public async Task<IEnumerable<Recipie>> GetRecipiesAsync()
        {

            try
            {
                IEnumerable<Recipie> recipies = await _context.Recipies.Include(r => r.Ingredients).Include(r => r.RecipieBook).ToListAsync();

                return recipies;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<Recipie> GetRecipieByIdAsync(int Id)
        {



            try
            {
                Recipie? recipie = await _context.Recipies.Include(r => r.Ingredients).Include(r => r.RecipieBook).FirstOrDefaultAsync(r => r.Id == Id);   

                return recipie;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task CreateRecipie(Recipie recipie)
        {

            try
            {

                await _context.AddAsync(recipie);

                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task UpdateRecipie(Recipie recipie)
        {

            try
            {

                 _context.Update(recipie);

                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task DeleteRecipieAsync(Recipie recipe)
        {
            try
            {

                _context.Remove(recipe);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }


        }



        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
        {

            try
            {
                IEnumerable<Ingredient> ingredients = await _context.Ingredients.Include(i => i.Recipies).ToListAsync();

                return ingredients;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task<Ingredient> GetIngredientByIdAsync(int Id)
        {



            try
            {
                Ingredient? ingredient = await _context.Ingredients.Include(i => i.Recipies).FirstOrDefaultAsync(i => i.Id == Id);

                return ingredient;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task CreateIngredient(Ingredient ingredient)
        {

            try
            {

                await _context.AddAsync(ingredient);

                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task UpdateIngredient(Ingredient ignredient)
        {

            try
            {

                _context.Update(ignredient);

                await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task DeleteIngredientAsync(Ingredient ignredient)
        {
            try
            {

                _context.Remove(ignredient);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task AddIngredientToRecipe(IEnumerable<int> ingredientIds, int recipeId)
        {

            try
            {

                Recipie? recipie = await _context.Recipies.Include(r => r.Ingredients).Include(r => r.RecipieBook).FirstOrDefaultAsync(r => r.Id == recipeId);

                foreach(int ingredientId in ingredientIds)
                {
                    Ingredient? ingredient = await _context.Ingredients.FindAsync(ingredientId);

                    if (recipie != null && ingredient != null)
                    {
                        recipie.Ingredients.Add(ingredient);
                    }
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
