namespace CareerBuilderX.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        public bool IsDeleted { get; set; }
        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        public string? ServiceImgName { get; set; }

        public string? ServiceImgType { get; set; }

        public byte[]? ServiceImg { get; set; }
        public List<Project> Projects { get; set; }
        public List<ServicePortfolio> servicePortfolios { get; set; }
    }
}
