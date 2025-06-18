using CareerBuilderX.Models;
using System.ComponentModel.DataAnnotations;

namespace CareerBuilderX.DTOs
{
    public class PortfolioDTO
    {

        [Required(ErrorMessage = "Bio is required")]
        public string About { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string? Address { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL")]
        public string? LinkedInLink { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL")]
        public string? GitHubLink { get; set; }

        [Url(ErrorMessage = "Invalid Twitter URL")]
        public string? TwitterLink { get; set; }

        [Url(ErrorMessage = "Invalid FaceBook URL")]
        public string? FaceBookLink { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "At least one service must be selected")]
        public List<int> ServiceIds { get; set; }

        [Required(ErrorMessage = "Profile image is required")]
        public IFormFile ProfileImage{ get; set; }

        public string? SkillsInfo { get; set; }
        public List<ProjectDTO> Projects { get; set; }


    }
}
