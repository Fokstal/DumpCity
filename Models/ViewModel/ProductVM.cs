using Microsoft.AspNetCore.Mvc.Rendering;

namespace DumpCity.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; } = new();
        public IEnumerable<SelectListItem> CategorySelectList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AppTypeSelectList { get; set; } = new List<SelectListItem>();

    }
}
