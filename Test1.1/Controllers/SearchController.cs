using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity; // Correct location for AppDBContext
using Test1._1.Models;         // For Applicant, Company, JobAdvertisment
using Test1._1.Models.ViewModels;

namespace Test1._1.Controllers
{
	public class SearchController : Controller
	{
		
		private readonly AppDBContext _context;

		public SearchController(AppDBContext context)
		{
			_context = context;
		}


		
		[HttpGet]
		public async Task<IActionResult> Index(string q, string filterFor, string city, string experience, string job, string salary, string workType)
		{
			var lower = q?.ToLower();

			// === 1. Applicants ===
			var applicantQuery = _context.Users.OfType<Applicant>().AsQueryable();

			if (!string.IsNullOrEmpty(lower))
			{
				applicantQuery = applicantQuery.Where(a =>
				
				 a.Field_work.ToLower().Contains(lower) ||
				 a.UserName.ToLower().Contains(lower) ||
				 a.lastName.ToLower().Contains(lower) ||
				 a.address.ToLower().Contains(lower));
			}

			if (!string.IsNullOrEmpty(city))
				applicantQuery = applicantQuery.Where(a => a.address == city);

			if (!string.IsNullOrEmpty(experience))
			{
				if (experience.StartsWith(">"))
				{
					if (int.TryParse(experience.Substring(1), out int minExp))
						applicantQuery = applicantQuery.Where(a => a.Years_experience > minExp);
				}
				else if (int.TryParse(experience, out int exp))
				{
					applicantQuery = applicantQuery.Where(a => a.Years_experience == exp);
				}
			}

			if (!string.IsNullOrEmpty(job))
			{
				if (job == "Others")
				{
					applicantQuery = applicantQuery.Where(a => !(new[] {
				"Software Engineer", "Front‑End Developer", "Back‑End Developer", "Full Stack Developer", "Mobile App Developer",
				"DevOps Engineer", "QA/Test Engineer", "UI/UX Designer", "Data Analyst", "Data Scientist", "Machine Learning Engineer",
				"Cybersecurity Specialist", "Cloud Engineer", "Game Developer", "IT Support Specialist", "System Administrator",
				"Network Engineer", "Embedded Systems Engineer", "AI Engineer", "Blockchain Developer", "Technical Writer",
				"IT Project Manager", "Database Administrator", "AR/VR Developer"
			}).Contains(a.Field_work));
				}
				else
				{
					applicantQuery = applicantQuery.Where(a => a.Field_work == job);
				}
			}

			var applicants = await applicantQuery.Select(a => new ApplicantCardHomeViewModel
			{
				Id = a.Id,
				Name = a.UserName,
				LastName = a.lastName,
				FieldWork = a.Field_work,
				ImagePath =  a.Profile_image
			}).ToListAsync();

			// === 2. Companies ===
			var companyQuery = _context.Users.OfType<Company>().AsQueryable();

			if (!string.IsNullOrEmpty(lower))
			{
				companyQuery = companyQuery.Where(c =>
					c.UserName.ToLower().Contains(lower) ||
					c.FiledWork.ToLower().Contains(lower));
			}

			var companies = await companyQuery.Select(c => new CompanyViewModel
			{
				Id = c.Id,
				UserName = c.UserName,
				Email = c.Email,
				PhoneNumber = c.PhoneNumber,
				Logo =  c.Logo,
				CurrentNumEmployees = c.CurrentNumEmployees,
				FiledWork = c.FiledWork,
				TaxCard = c.TaxCard,
				CommercialRegister = c.CommercialRegister,
				Description = c.Description,
				Status = c.status
			}).ToListAsync();

			// === 3. Advertisements ===
			var adQuery = _context.JobAdvertisments.Include(ad => ad.Company).AsQueryable();

			if (!string.IsNullOrEmpty(lower))
			{
				adQuery = adQuery.Where(ad =>
					ad.jobtitle.ToLower().Contains(lower) ||
					ad.governorate.ToLower().Contains(lower) ||
					ad.Company.UserName.ToLower().Contains(lower));
			}

			if (!string.IsNullOrEmpty(city))
				adQuery = adQuery.Where(ad => ad.governorate == city);

			if (!string.IsNullOrEmpty(job))
			{
				if (job == "Others")
				{
					adQuery = adQuery.Where(ad => !(new[] {
				"Software Engineer", "Front‑End Developer", "Back‑End Developer", "Full Stack Developer", "Mobile App Developer",
				"DevOps Engineer", "QA/Test Engineer", "UI/UX Designer", "Data Analyst", "Data Scientist", "Machine Learning Engineer",
				"Cybersecurity Specialist", "Cloud Engineer", "Game Developer", "IT Support Specialist", "System Administrator",
				"Network Engineer", "Embedded Systems Engineer", "AI Engineer", "Blockchain Developer", "Technical Writer",
				"IT Project Manager", "Database Administrator", "AR/VR Developer"
			}).Contains(ad.jobtitle));
				}
				else
				{
					adQuery = adQuery.Where(ad => ad.jobtitle == job);
				}
			}

			if (!string.IsNullOrEmpty(workType))
				adQuery = adQuery.Where(ad => ad.Job_time == workType);

			var adList = await adQuery.ToListAsync();

		
			if (!string.IsNullOrEmpty(salary))
			{
				adList = adList.Where(ad =>
				{
					if (string.IsNullOrEmpty(ad.salary) || !ad.salary.Contains("-")) return false;

					var parts = ad.salary.Split("-");
					if (parts.Length != 2) return false;

					if (int.TryParse(parts[0], out int adMin) && int.TryParse(parts[1], out int adMax))
					{
						if (salary.StartsWith(">"))
						{
							if (int.TryParse(salary.Substring(1), out int min))
							{
								return adMin > min; // Only show if full ad range is above the min
							}
						}
						else if (salary.Contains("-"))
						{
							var rangeParts = salary.Split("-");
							if (rangeParts.Length == 2 &&
								int.TryParse(rangeParts[0], out int filterMin) &&
								int.TryParse(rangeParts[1], out int filterMax))
							{
								// ✅ Only include if entire ad salary is within the selected range
								return adMin >= filterMin && adMax <= filterMax;
							}
						}
					}

					return false;
				}).ToList();
			}



			var ads = adList.Select(ad => new CompanyAdvHomeViewModel
			{
				AdvertisementId = ad.Id,
				CompanyName = ad.Company.UserName,
				CompanyDescription = ad.Company.Description,
				LogoPath =  ad.Company.Logo,
				JobTitle = ad.jobtitle,
				Salary = ad.salary,
				Location = ad.governorate,
				JobTime = ad.Job_time,
				CreatedDate = ad.CreatedDate,
				CompanyId = ad.CompanyId,
				JobDescription = ad.Jobdetail,
				Requirements = ad.JobRequirements
			}).ToList();

			// ✅ Apply "Filter For" to hide unwanted sections
			if (!string.IsNullOrEmpty(filterFor))
			{
				switch (filterFor.ToLower())
				{
					case "applicant":
						companies = new List<CompanyViewModel>();
						ads = new List<CompanyAdvHomeViewModel>();
						break;
					case "company":
						applicants = new List<ApplicantCardHomeViewModel>();
						ads = new List<CompanyAdvHomeViewModel>();
						break;
					case "ads":
						applicants = new List<ApplicantCardHomeViewModel>();
						companies = new List<CompanyViewModel>();
						break;
				}
			}

			var viewModel = new SearchPageViewModel
			{
				Applicants = applicants,
				Companies = companies,
				Ads = ads
			};

			return View("SearchPage", viewModel);
		}





	}
}