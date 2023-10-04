namespace DumpCity.Models.ViewModel
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new();
        }

        public Product Product { get; set; }
        public bool IsExistsProductInCart { get; set; }
    }
}