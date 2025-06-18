using CareerBuilderX.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CareerBuilderX.DTOs
{
    public class EditPortfolioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Lname { get; set; }

        [Url(ErrorMessage = "Invalid Twitter URL")]
        public string? TwitterLink { get; set; }

        [Url(ErrorMessage = "Invalid FaceBook URL")]
        public string? FaceBookLink { get; set; }
        public string? Title { get; set; }

        [Required(ErrorMessage = "Your Bio Title Name is required")]
        public string Summary { get; set; }
        public byte[]? ExistingProfileImg { get; set; }
        public string? ExistingProfileImgType { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL")]
        public string? LinkedInLink { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL")]
        public string? GitHubLink { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "At least one service must be selected")]
        public List<int> ServiceIds { get; set; }
        public string? SkillsInfo { get; set; }
        public IFormFile? ProfileImage { get; set; }
        
        [AllowNull]
        public List<ProjectEditDTO>? Projects { get; set; }

        [AllowNull]
        public List<SkillsEditDTO>? Skills { get; set; }


        public class SkillsEditDTO
        {
            public int? Id { get; set; }

            public string skillName { get; set; }   
        }

        public class ProjectEditDTO
        {
            public int? Id { get; set; }  // existing-project ID for update/delete detection

            [Required(ErrorMessage = "Project title is required")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Project description is required")]
            public string ProjectDescription { get; set; }

            // for preview
            public byte[]? ExistingProjectImg { get; set; }
            public string? ExistingProjectImgType { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            [Url]
            public string? ProjectLink { get; set; }

            public int? ServiceId { get; set; }  // عشان نربطه بخدمة جاهزة

            // optional new upload
            public IFormFile? ProjectImage { get; set; }
        }
    }
}
