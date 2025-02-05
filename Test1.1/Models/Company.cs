namespace test1._0.model
{
	public class Company:User
	{
		public int Company_Id { get; set; }
		public string? Logo {  get; set; }
		public int Current_num_employees { get; set; }
		public string? Filed_work {  get; set; }
		public string? Tax_card { get; set; }

		public string? Commercia_register { get; set; }
		public ICollection<Job_Adv> Job_Advs { get; set; } = new List<Job_Adv>();
	}
}
