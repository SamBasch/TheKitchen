using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheKitchen.Models;

namespace TheKitchen.Data
{
    public class ApplicationDbContext : IdentityDbContext<RBUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Recipie> Recipies { get; set; } = default!;
        public virtual DbSet<RecipieBook> RecipieBooks { get; set; } = default!;

        public virtual DbSet<Ingredient> Ingredients { get; set; } = default!;
    }
}