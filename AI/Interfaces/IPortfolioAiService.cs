using CareerBuilderX.Models;

namespace CareerBuilderX.AI.Interfaces
{
    public interface IPortfolioAiService
    {
        Task<Portfolio> ParseUserInputAsync(string userInput);
    }
}
