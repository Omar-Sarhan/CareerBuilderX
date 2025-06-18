using CareerBuilderX.DTOs;
using CareerBuilderX.Models;

namespace CareerBuilderX.Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        public List<Portfolio> GetAllPortfoliosByUser(string userId);

        public Portfolio GetPortfolioById(int PortfolioId);

        public void CreatePortfolio(Portfolio portfolio);

        public void UpdatePortfolio(Portfolio portfolio);

        public void DeletePortfolio(int PortfolioId);

        public void DeleteProject(int projectId);

        public void DeleteSkill(int SkillId);

        public int NumberOfPortfolios();
        List<PortfolioCountByDateDTO> NumberOfPortfolioByDate();
    }
}
