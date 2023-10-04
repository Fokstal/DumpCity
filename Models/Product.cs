using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DumpCity.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        [Display(Name = "Short description")]
        public string? ShortDesc { get; set; }
        [Display(Name = "Description")]
        public string? Desc { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set; }

        public string? ImageURL { get; set; }


        #nullable disable
        [Display(Name = "Category Type")]
        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public virtual Category Category { get; set; }

        [Display(Name = "Application Type")]
        public int AppTypeID { get; set; }
        [ForeignKey(nameof(AppTypeID))]
        public virtual AppType AppType { get; set; }
    }
}
