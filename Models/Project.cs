namespace CareerBuilderX.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string Title { get; set; }

        public string ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set;}

        public string? ProjectLink { get; set; }

        public Portfolio Portfolio { get; set; }
        public int PortfolioId { get; set; }

        public Service? Service { get; set; }

        public int? ServiceId { get; set; }

        public string? projectImgName { get; set; }

        public string? prjectImgType { get; set; }

        public byte[]? ProjectImg { get; set; }

    }
}
