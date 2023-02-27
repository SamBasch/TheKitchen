using System.ComponentModel.DataAnnotations;

namespace TheKitchen.Models
{
    public class Ingredient
    {

        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string? Name { get; set; }


        [Required]
        public decimal Measure { get; set; }


        //navigation properties

        public virtual ICollection<Recipie> Recipies { get; set; } = new HashSet<Recipie>();
    }
}
