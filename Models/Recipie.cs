using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheKitchen.Models
{
    public class Recipie
    {
        public int Id { get; set; }


        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }


        public byte[]? ImageData { get; set; }

        public string? ImageType { get; set; }

        [NotMapped]
        public virtual IFormFile? ImageFile { get; set; }


        public int RecipieBookId { get; set; }


        //navigation properties


        public virtual RecipieBook? RecipieBook { get; set; }



        public virtual ICollection<Ingredient> Ingredients { get; set; } = new HashSet<Ingredient>();
    }
}
