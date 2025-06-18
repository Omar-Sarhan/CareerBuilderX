using System.ComponentModel.DataAnnotations;

namespace CareerBuilderX.DTOs
{
    public class ResumeDTO
    {
        [Required(ErrorMessage = "About is required")]
        public string AboutYou { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string?Address { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL")]
        public string? LinkedInLink { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL")]
        public string? GitHubLink { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Career information is required")]
        public string CareerInfo { get; set; }

        [Required(ErrorMessage = "Education information is required")]
        public string EducationInfo { get; set; }

        [Required(ErrorMessage = "Language information is required")]
        public string LanguageInfo { get; set; }

        public string? CertificationInfo { get; set; }

        public string? SkillsInfo { get; set; }
    }
}
