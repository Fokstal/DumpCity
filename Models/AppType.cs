using System.ComponentModel.DataAnnotations;

namespace DumpCity.Models
{
	public class AppType
	{
		[Key]
		public int ID { get; set; }

		[Required]
		public string? Name { get; set; }
	}
}
