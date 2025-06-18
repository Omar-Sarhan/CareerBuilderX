using System.Globalization;

namespace CareerBuilderX.Models
{
    public class Portfolio : PersonalInfo
    {
        public int PortfolioId { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime CreatedModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string About { get; set; }
        public List<ServicePortfolio> servicePortfolios { get; set; }
        public List<Project> Projects { get; set; }

        public string EndUserId { get; set; }

        public EndUser EndUser { get; set; }

        public string? profileImgName { get; set; }

        public string ?profileImgType { get; set;}

        public byte[] ?ProfileImg { get; set; }
        public string? TwitterLink { get; set; }
        public string? FaceBookLink { get; set; }

        public List<Skill> Skills { get; set; }
        public string? SkillsInfo { get; set; }
    }
}
