using Microsoft.AspNetCore.Identity;

namespace Test1._1.Models.Entity
{
	public class ApplicationUser:IdentityUser <int>
	{
		public string address { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}