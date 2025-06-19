namespace Test1._1.Models.Entity
{
	public class Question
	{
		public int Id { get; set;  }
        public string Text { get; set;  }
		public string Type { get; set;  }
		public bool IsShared { get; set; } = false;
		public int? JobAdvertismentId { get; set; }
		public JobAdvertisment? JobAdvertisment { get; set; }
		public ICollection<Answer> Answers { get; set; } = new List<Answer>();

	}
}
