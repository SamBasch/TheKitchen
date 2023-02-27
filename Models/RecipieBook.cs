using System.ComponentModel.DataAnnotations;

namespace TheKitchen.Models
{
    public class RecipieBook
    {
        public int Id { get; set; }


        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string? Title { get; set; }


        [Required]
        public string? UserId { get; set; }



        //navigation properties

        public virtual RBUser? User { get; set; }

        public virtual ICollection<Recipie> Recipies { get; set; } = new HashSet<Recipie>();
    }
}
