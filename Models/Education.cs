namespace CareerBuilderX.Models
{
    public class Education
    {
        public int EducationId { get; set; }

        public string CollegeName { get; set; } 

        public string Major { get; set; }
        public string Degree { get; set; }    

        public double? GPA { get; set; } 

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set;}

        public Resume Resume { get; set; }

        public int ResumeId { get; set; }
    }
}
