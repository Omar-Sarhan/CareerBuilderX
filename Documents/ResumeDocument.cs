using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CareerBuilderX.Models;
using System.Globalization;

public class ResumeDocument : IDocument
{
    private readonly Resume model;

    public ResumeDocument(Resume model)
    {
        this.model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(40);
            page.DefaultTextStyle(x => x.FontSize(11));
            page.Content().Column(column =>
            {
                // Header
                column.Item().AlignCenter().Text($"{model.FName} {model.Lname}")
                    .FontSize(22).Bold().FontColor(Colors.Blue.Darken2);

                // Contact Info
                column.Item().AlignCenter().Text(text =>
                {
                    if (!string.IsNullOrWhiteSpace(model.Email))
                        text.Span(model.Email + " ").FontSize(10).FontColor(Colors.Grey.Darken1);
                    if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                        text.Span("| " + model.PhoneNumber + " ").FontSize(10).FontColor(Colors.Grey.Darken1);
                    if (!string.IsNullOrWhiteSpace(model.Address))
                        text.Span("| " + model.Address).FontSize(10).FontColor(Colors.Grey.Darken1);
                });

                // Links
                if (!string.IsNullOrWhiteSpace(model.LinkedInLink) || !string.IsNullOrWhiteSpace(model.GitHubLink))
                {
                    column.Item().AlignCenter().Text(text =>
                    {
                        if (!string.IsNullOrWhiteSpace(model.LinkedInLink))
                            text.Span(model.LinkedInLink + " ").FontSize(10).FontColor(Colors.Blue.Medium);
                        if (!string.IsNullOrWhiteSpace(model.GitHubLink))
                            text.Span("| " + model.GitHubLink).FontSize(10).FontColor(Colors.Blue.Medium);
                    });
                }

                void AddSectionTitle(string title) =>
                    column.Item().PaddingTop(20).Text(title).Bold().FontSize(14).FontColor(Colors.Black);

                // Summary
                if (!string.IsNullOrWhiteSpace(model.Summary))
                {
                    AddSectionTitle("Professional Summary");
                    column.Item().Text(model.Summary).FontSize(11);
                }

                // Education
                if (model.Educations?.Any() == true)
                {
                    AddSectionTitle("Education");

                    foreach (var edu in model.Educations)
                    {
                        if (!string.IsNullOrWhiteSpace(edu.Degree) || !string.IsNullOrWhiteSpace(edu.Major))
                            column.Item().Text($"{edu.Degree} of {edu.Major}").Bold();

                        if (!string.IsNullOrWhiteSpace(edu.CollegeName))
                            column.Item().Text(edu.CollegeName);

                        if (edu.StartDate.HasValue || edu.EndDate.HasValue)
                            column.Item().Text($"{FormatDate(edu.StartDate)} – {FormatDate(edu.EndDate)}");

                        if (edu.GPA.HasValue)
                            column.Item().Text($"GPA: {edu.GPA.Value:0.00}");

                        column.Item().PaddingBottom(10);
                    }
                }

                // Experience
                if (model.Experiences?.Any() == true)
                {
                    AddSectionTitle("Experience");

                    foreach (var exp in model.Experiences)
                    {
                        if (!string.IsNullOrWhiteSpace(exp.Title) || !string.IsNullOrWhiteSpace(exp.CompanyName))
                            column.Item().Text($"{exp.Title} – {exp.CompanyName}").Bold();

                        if (exp.StartDate.HasValue || exp.EndDate.HasValue || exp.IsCurrent)
                        {
                            var end = exp.IsCurrent ? "Present" : FormatDate(exp.EndDate);
                            column.Item().Text($"{FormatDate(exp.StartDate)} – {end}");
                        }

                        if (!string.IsNullOrWhiteSpace(exp.Description))
                            column.Item().Text(exp.Description);

                        column.Item().PaddingBottom(10);
                    }
                }

                // Certifications
                if (model.Certifications?.Any(c => !string.IsNullOrWhiteSpace(c.Title)) == true)
                {
                    AddSectionTitle("Certifications");

                    foreach (var cert in model.Certifications.Where(c => !string.IsNullOrWhiteSpace(c.Title)))
                    {
                        column.Item().Text(cert.Title).Bold();

                        if (!string.IsNullOrWhiteSpace(cert.ProviderName))
                            column.Item().Text(cert.ProviderName);

                        if (cert.StartDate.HasValue || cert.EndDate.HasValue)
                            column.Item().Text($"{FormatDate(cert.StartDate)} – {FormatDate(cert.EndDate)}");

                        column.Item().PaddingBottom(10);
                    }
                }

                // Languages
                if (model.Languages?.Any() == true)
                {
                    AddSectionTitle("Languages");
                    column.Item().Text(string.Join(" | ", model.Languages.Select(l => $"{l.LanguageName} – {l.Level}")));
                }

                // Skills
                if (model.Skills?.Any() == true)
                {
                    AddSectionTitle("Skills");

                    foreach (var group in model.Skills.GroupBy(s => s.SkillType))
                    {
                        column.Item().Text($"{group.Key} Skills:").Bold();
                        column.Item().Text(string.Join(", ", group.Select(s => s.SkillName)));
                    }
                }
            });
        });
    }

    private string FormatDate(DateTime? date)
    {
        return date?.ToString("MMM yyyy", CultureInfo.InvariantCulture) ?? "";
    }
}
