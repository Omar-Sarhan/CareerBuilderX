namespace CareerBuilderX.Models
{
    public class Certification
    {
        public int CertificationId { get; set; }

        public string Title { get; set;}

        public string ProviderName { get; set;}

        public DateTime? StartDate { get; set;}

        public DateTime? EndDate { get; set;}

        public Resume Resume { get; set;}

        public int ResumeId { get; set; }





    }
}
