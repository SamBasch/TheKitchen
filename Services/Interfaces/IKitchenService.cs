using TheKitchen.Models;

namespace TheKitchen.Services.Interfaces
{
    public interface IKitchenService
    {


        public Task<IEnumerable<RecipieBook>> GetRecipieBooksAsync(string userId);

        public Task<RecipieBook> GetRecipieBookAsync(int Id, string userId);

        public Task CreateRecipieBookAsync(RecipieBook recipeBook);


        public Task RemoveAllRecipiesFromBook(int bookId, string userId);

        public Task UpdateRecipieBook(RecipieBook recipeBook);

        public Task DeleteBookAsync(RecipieBook recipeBook);



        public Task<IEnumerable<Recipie>> GetRecipiesAsync();

        public Task<Recipie> GetRecipieByIdAsync(int Id);

        public Task CreateRecipie(Recipie recipie);

        public Task UpdateRecipie(Recipie recipie);

        public Task DeleteRecipieAsync(Recipie recipe);

        public Task<Ingredient> GetIngredientByIdAsync(int Id);

        public Task CreateIngredient(Ingredient ingredient);

        public Task UpdateIngredient(Ingredient ignredient);

        public Task DeleteIngredientAsync(Ingredient ignredient);

        public Task<IEnumerable<Ingredient>> GetIngredientsAsync();

        public Task AddIngredientToRecipe(IEnumerable<int> ingredientIds, int recipeId);

    }
}
