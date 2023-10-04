using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DumpCity.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range (1, int.MaxValue, ErrorMessage = "Display order value must be greater than 0")]
        public int DisplayOrder { get; set; }
    }
}
