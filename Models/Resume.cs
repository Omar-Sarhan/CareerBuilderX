using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CareerBuilderX.Models
{
    public class Resume:PersonalInfo
    {
        public int ResumeId { get; set; }

        public DateTime CreatedDate { get; set; } 

        public DateTime CreatedModifiedDate { get; set; } 

        public bool IsDeleted { get; set; }

        public EndUser EndUser { get; set; }

        public string EndUserId { get; set; }

        public string CareerInfo { get; set; }
        public string EducationInfo { get; set; }
        public string LanguageInfo { get; set; }
        public string? CertificationInfo { get; set; }
        public string? SkillsInfo { get; set; }
        public string? About { get; set; }
        public List <Education> Educations { get; set; }

        public List<Experience> Experiences { get; set; }

        public List<Language> Languages { get; set; }

        public List<Skill> Skills { get; set; }

        public List<Certification> Certifications { get; set; }



    }
}
