using CareerBuilderX.AI.Interfaces;
using CareerBuilderX.Models;
using Microsoft.SemanticKernel;
using System.Text.Json.Nodes;

namespace CareerBuilderX.AI.Services
{
    
        public class PortfolioAiService : IPortfolioAiService
        {
            private readonly Kernel _kernel;


            public PortfolioAiService(Kernel kernel)
            {
                _kernel = kernel;

            }

        public async Task<Portfolio> ParseUserInputAsync(string userInput)
        {
            var prompt = @"# Role: Portfolio Generator
**Respond with **only** the JSON object—no explanations, no extra text.**

You are an AI assistant that helps generate clean and professional portfolio content based on the user's raw input.

## Tasks:
1. Extract full name and split it into `FName` (first word) and `LName` (last word).
2. Generate a well-written, first-person professional summary (Summary).
3. For each project mentioned in the input, generate a professional and concise project description.
4. Extract a `Title` (job title):
   - If the user explicitly provides one, use it.
   - If not, infer the most appropriate title based on their background and input.
   - If inference is impossible, return `""not provided""`.

## Rules:
- All output must be in **English only**.
- Summary must be written in first-person (e.g., “I have worked on…”).
- Do **NOT** fabricate any content that wasn’t mentioned in the user input.
- Keep the JSON structure exactly as provided.
- **For any required field** (`FName`, `LName`, `Title`, `Summary`, each `Project.Title`, each `Project.ProjectDescription`):
  - If it is missing or cannot be inferred, return the string `""not provided""`.
- **Do NOT** include any explanations, labels, greetings, or any text outside the JSON.
🟦 Skills:
If you conclude from all the information you received about any skills that are not included in the skills section, add them.

## Input:
{{$input}}

## Output (JSON):
{
  \""FName\"": \""string\"",
  \""LName\"": \""string\"",
  \""Title\"": \""string\"",
  \""Summary\"": \""string\"",
  \""Projects\"": [
    {
      \""Title\"": \""string\"",
      \""ProjectDescription\"": \""string\""
    }
  ],

  ""Skills"": [
    {
      ""SkillName"": ""string""
    }
  ]
}";


            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);
            var result = await _kernel.InvokeAsync(extractFunction, new() { ["input"] = userInput });

            var json = result.ToString();
            Console.WriteLine(json);
            var jsonNode = JsonNode.Parse(json);

            var portfolio = new Portfolio
            {
                FName = jsonNode["FName"]?.ToString() ?? "",
                Lname = jsonNode["LName"]?.ToString() ?? "",
                Title = jsonNode["Title"]?.ToString() ?? "",
                Summary = jsonNode["Summary"]?.ToString() ?? "",
                Projects = jsonNode["Projects"]?.AsArray()?.Select(p => new Project
                {
                    Title = p["Title"]?.ToString() ?? "",
                    ProjectDescription = p["ProjectDescription"]?.ToString() ?? ""
                }).ToList() ?? new(),
                Skills = jsonNode["Skills"]?.AsArray()?.Select(p => new Skill
                {
                    SkillName = p["SkillName"]?.ToString() ?? ""
                }).ToList() ?? new()
            };
            
            portfolio.Projects.ForEach(p => p.Portfolio = portfolio);
            portfolio.Skills.ForEach(p => p.Portfolio = portfolio);
            return portfolio;
        }

    }
}


