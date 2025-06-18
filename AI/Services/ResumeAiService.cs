using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CareerBuilderX.AI.Interfaces;
using CareerBuilderX.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CareerBuilderX.AI.Services
{
    public class ResumeAiService : IResumeAiService
    {
        private readonly Kernel _kernel;


        public ResumeAiService(Kernel kernel)
        {
            _kernel = kernel;

        }

        public async Task<Resume> ParseUserInputAsync(string userInput)
        {
            var prompt = @"
Return only a valid JSON object. Do not include any explanation, translation, notes, or text outside the JSON.
# Role: Resume Parser
You are a professional resume parser. Your task is to extract structured resume fields from a user's unstructured input and return a clean, professional JSON object matching the schema below.

## Rules:

🟦 General Rules:
- All output must be in **English only**, including names, locations, and job titles (even if the input was in another language).
- Improve grammar and formatting where needed, but **do not fabricate or assume** any data.
- Keep the JSON structure exactly as provided.
- Dates must be formatted as ""YYYY-MM-DD"" (ISO 8601).
- For these specific required fields only:
  - FName, LName
  - Experience.Title, Experience.CompanyName
  - Education.CollegeName, Education.Major, Education.Degree
  If the value is missing or unknown, return the string ""not provided"".
- For all other fields, if missing or unknown, return empty string """" or null (do not return ""not provided"").

🟦 Extract a `Title` (job title):
- If the user explicitly provides one, use it.
- If not, infer the most appropriate title based on their background and input.

🟦 Summary:
- Generate a brief, professional, and well-written summary based only on the user's input.
- The summary must be written in a neutral or first-person tone (no third-person references like ""Omar is..."").
- Do not invent or assume details not present in the input.

🟦 Name:
- Extract first name (FName) as the first word, and last name (LName) as the last word from the full name.
- If the name is in Arabic or another language, transliterate or translate it into English if possible.
- If missing, return ""not provided"".

🟦 Experience:
- If EndDate is missing or later than today's date, set ""IsCurrent"": true.
- If Title or CompanyName is missing, return ""not provided"".
- Professionally rephrase or summarize the Description to reflect relevant responsibilities based on the title and technologies mentioned by the user (e.g., ""Developed web applications using ASP.NET and SQL Server..."").
- Do not add information that is not clearly implied from the input.

🟦 Skills:
- If you conclude from all the information you received about any skills that are not included in the skills section, add them.

🟦 Education:
- If CollegeName, Degree, or Major is missing, return ""not provided"".
- Identify the ""Degree"" type (e.g., ""Bachelor's"", ""Master's"") automatically if mentioned or implied.
- If GPA is on a 100-scale or 5.0-scale, convert it to a 4.0 scale with appropriate precision.

🟦 Languages:
- The `Level` field should be a number from 1 (Basic) to 5 (Fluent/Native), based on user input.

## Input
{{$input}}

## Output (JSON format):
{
  ""FName"": ""string"",
  ""Title"": ""string"",
  ""LName"": ""string"",
  ""Email"": ""string"",
  ""PhoneNumber"": ""string"",
  ""Address"": ""string"",
  ""Summary"": ""string"",

  ""Education"": [
    {
      ""CollegeName"": ""string"",
      ""Degree"": ""string"",
      ""Major"": ""string"",
      ""GPA"": 0.0,
      ""StartDate"": ""YYYY-MM-DD"",
      ""EndDate"": ""YYYY-MM-DD""
    }
  ],

  ""Experience"": [
    {
      ""Title"": ""string"",
      ""Description"": ""string"",
      ""CompanyName"": ""string"",
      ""StartDate"": ""YYYY-MM-DD"",
      ""EndDate"": ""YYYY-MM-DD"",
      ""IsCurrent"": false
    }
  ],

  ""Skills"": [
    {
      ""SkillName"": ""string"",
      ""SkillType"": ""string""
    }
  ],

  ""Languages"": [
    {
      ""LanguageName"": ""string"",
      ""Level"": 0
    }
  ],

  ""Certifications"": [
    {
      ""Title"": ""string"",
      ""ProviderName"": ""string"",
      ""StartDate"": ""YYYY-MM-DD"",
      ""EndDate"": ""YYYY-MM-DD""
    }
  ]
}

Return only the JSON object. Do not include any explanations, descriptions, or extra text.";





            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);
            var result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = userInput
            });
            var json = result.ToString();
            Console.WriteLine(json);
            var jsonNode = JsonNode.Parse(json);

            // أنشئ كائن جديد من Resume وربط البيانات يدويًا
            Resume resume = new Resume
            {
                FName = jsonNode["FName"]?.ToString() ?? "",
                Lname = jsonNode["LName"]?.ToString() ?? "",
                Title = jsonNode["Title"]?.ToString() ?? "",
                Email = jsonNode["Email"]?.ToString() ?? "",
                PhoneNumber = jsonNode["PhoneNumber"]?.ToString() ?? "",
                Address = jsonNode["Address"]?.ToString() ?? "",
                Summary = jsonNode["Summary"]?.ToString() ?? "",
                CreatedDate = DateTime.Now,
                CreatedModifiedDate = DateTime.Now,

                Educations = jsonNode["Education"]?.AsArray()?.Select(e => new Education
                {
                    CollegeName = e["CollegeName"]?.ToString(),
                    Major = e["Major"]?.ToString(),
                    Degree = e["Degree"]?.ToString(),
                    GPA = e["GPA"]?.GetValue<double?>(),
                    StartDate = DateTime.TryParse(e["StartDate"]?.ToString(), out var s) ? s : null,
                    EndDate = DateTime.TryParse(e["EndDate"]?.ToString(), out var ed) ? ed : null
                }).ToList() ?? new(),

                Experiences = jsonNode["Experience"]?.AsArray()?.Select(e => new Experience
                {
                    Title = e["Title"]?.ToString(),
                    Description = e["Description"]?.ToString(),
                    CompanyName = e["CompanyName"]?.ToString(),
                    StartDate = DateTime.TryParse(e["StartDate"]?.ToString(), out var s) ? s : null,
                    EndDate = DateTime.TryParse(e["EndDate"]?.ToString(), out var ed) ? ed : null,
                    IsCurrent = e["IsCurrent"]?.GetValue<bool>() ?? false
                }).ToList() ?? new(),

                Skills = jsonNode["Skills"]?.AsArray()?.Select(s => new Skill
                {
                    SkillName = s["SkillName"]?.ToString(),
                    SkillType = s["SkillType"]?.ToString()
                }).ToList() ?? new(),

                Languages = jsonNode["Languages"]?.AsArray()?.Select(l => new Language
                {
                    LanguageName = l["LanguageName"]?.ToString(),
                    Level = l["Level"]?.GetValue<int>() ?? 0
                }).ToList() ?? new(),


                Certifications = jsonNode["Certifications"]?.AsArray()?.Select(c => new Certification
                {
                    Title = c["Title"]?.ToString(),
                    ProviderName = c["ProviderName"]?.ToString(),
                    StartDate = DateTime.TryParse(c["StartDate"]?.ToString(), out var s) ? s : null,
                    EndDate = DateTime.TryParse(c["EndDate"]?.ToString(), out var ed) ? ed : null
                }).ToList() ?? new()
            };

            // اربط كل كيان بالـ Resume
            resume.Educations.ForEach(e => e.Resume = resume);
            resume.Experiences.ForEach(e => e.Resume = resume);
            resume.Skills.ForEach(s => s.Resume = resume);
            resume.Languages.ForEach(l => l.Resume = resume);
            resume.Certifications.ForEach(c => c.Resume = resume);

            return resume;

        }
    }
}
