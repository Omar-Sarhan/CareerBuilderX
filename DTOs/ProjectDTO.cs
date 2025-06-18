using System.ComponentModel.DataAnnotations;

namespace CareerBuilderX.DTOs
{
    public class ProjectDTO
    {

        public int? Id { get; set; }

        [Required(ErrorMessage = "Project Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Project desc is required")]
        public string ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Url]
        public string? ProjectLink { get; set; }

        public int? ServiceId { get; set; }  // عشان نربطه بخدمة جاهزة
        public IFormFile? ProjectImage { get; set; }
    }
}
