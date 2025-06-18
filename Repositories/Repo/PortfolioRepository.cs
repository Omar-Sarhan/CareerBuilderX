using CareerBuilderX.Data;
using CareerBuilderX.DTOs;
using CareerBuilderX.Models;
using CareerBuilderX.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerBuilderX.Repositories.Repo
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private ApplicationDbContext _context { get; set; }

        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Portfolio> GetAllPortfoliosByUser(string userId)
        {
            return _context.Portfolios.Where(x => x.EndUserId == userId && x.IsDeleted==false)
                .Include(x => x.Projects)
                .Include(x => x.servicePortfolios)
                .ThenInclude(x => x.Service)
                .Include(x => x.Skills)
                .ToList();
        }

        public Portfolio GetPortfolioById(int PortfolioId)
        {
            return _context.Portfolios
                .Include(x => x.Projects)
                .Include(x => x.servicePortfolios)
                .ThenInclude(x => x.Service)
                .Include(x => x.EndUser)
                .Include(x => x.Skills)
                .SingleOrDefault(x => x.PortfolioId == PortfolioId && x.IsDeleted == false);


        }
        public void CreatePortfolio(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();
        }
        public void UpdatePortfolio(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            _context.SaveChanges();
        }

        public void DeletePortfolio(int PortfolioId)
        {
            var ExistPortfolio = GetPortfolioById(PortfolioId);
            if (ExistPortfolio != null)
            {
                ExistPortfolio.IsDeleted = true;
            }
            _context.SaveChanges();
        }

        public void DeleteProject(int projectId)
        {
            var proj = _context.Projects.Find(projectId);
            if (proj != null)
            {
                _context.Projects.Remove(proj);
                _context.SaveChanges();
            }
        }

        public void DeleteSkill(int SkillId)
        {
            var Skill = _context.Skills.Find(SkillId);
            if (Skill != null)
            {
                _context.Skills.Remove(Skill);
                _context.SaveChanges();
            }
        }
        public int  NumberOfPortfolios()
        {
          int count =   _context.Portfolios.Where(r=> r.IsDeleted != true).ToList().Count;
            return count;
        }

        public List<PortfolioCountByDateDTO> NumberOfPortfolioByDate()
        {
            var today = DateTime.Today;
            var fromDate = today.AddDays(-30);

            var result = _context.Portfolios
                .Where(r => !r.IsDeleted && r.CreatedDate.Date >= fromDate)
                .AsEnumerable()
                .GroupBy(r => r.CreatedDate.Date)
                .Select(g => new PortfolioCountByDateDTO
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();



            return result;
        }
    }
}
