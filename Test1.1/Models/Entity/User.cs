namespace Test1._1.Models.Entity
{
	public class User
	{
		public int Id { get; set; }
		public string Fname { get; set; }
		public string Lname { get; set; }
		public string HashedPassword { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsDeleted { get; set; } = false;

	}
}