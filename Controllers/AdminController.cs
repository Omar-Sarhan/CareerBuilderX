using CareerBuilderX.AI.Interfaces;
using CareerBuilderX.Repositories.Interfaces;
using CareerBuilderX.Repository.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CareerBuilderX.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPortfolioRepository _portfolioRepository;

        private IResumeRepository _resumeRepository;

        private IServiceRepository _serviceRepository { get; set; }

        public AdminController(IPortfolioRepository portfolioRepository, IResumeRepository resumeRepository, IServiceRepository serviceRepository)
        {
            _portfolioRepository = portfolioRepository;

            _resumeRepository = resumeRepository;

            _serviceRepository = serviceRepository;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            ViewBag.NumberOFResumes = _resumeRepository.NumberOfResumes();
            ViewBag.NumberOFPortfolios = _portfolioRepository.NumberOfPortfolios();
            ViewBag.NumberOFUsers = _resumeRepository.NumberOfUsers();
            var ResumechartData = _resumeRepository.NumberOfResumesByDate();
            ViewBag.ResumeChartData = JsonConvert.SerializeObject(ResumechartData);
            var PortfoliochartData = _portfolioRepository.NumberOfPortfolioByDate();
            ViewBag.PortfolioChartData = JsonConvert.SerializeObject(PortfoliochartData);
            ViewBag.ResumePortfolioChart = JsonConvert.SerializeObject(new
            {
                Labels = new[] { "Resumes", "Portfolios" },
                Series = new[] {
        _resumeRepository.NumberOfResumes(),
        _portfolioRepository.NumberOfPortfolios()
    }
            });

            return View();
        }
    }
}
