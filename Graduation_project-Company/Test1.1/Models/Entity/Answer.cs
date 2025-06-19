namespace Test1._1.Models.Entity
{
	public class Answer
	{
		public int Id { get; set; }
		public int ApplicantAdvertismentId { get; set; }
		public ApplicantAdvertisment ApplicantAdvertisments { get; set; }
		public int QuestionId { get; set; }
		public Question Questions { get; set; }
		public string Response { get; set; }
	}
}
