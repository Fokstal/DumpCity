namespace DumpCity.Models.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new();
        }

        public AppUser? AppUser { get; set; }
        public List<Product>? ProductList { get; set; }
    }
}