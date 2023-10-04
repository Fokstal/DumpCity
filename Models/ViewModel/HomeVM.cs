namespace DumpCity.Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<DetailsVM> Products { get; set; } = new List<DetailsVM>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}