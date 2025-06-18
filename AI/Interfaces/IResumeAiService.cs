using CareerBuilderX.Models;

namespace CareerBuilderX.AI.Interfaces
{
    public interface IResumeAiService
    {
        Task<Resume> ParseUserInputAsync(string userInput);
    }
}

