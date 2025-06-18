namespace CareerBuilderX.Models
{
    public class Skill
    {
        public int SkillId { get; set; }

        public string SkillName { get; set; }

        public string? SkillType { get; set; }
        public Portfolio? Portfolio { get; set; }
        public int? PortfolioId { get; set; }
        public Resume? Resume { get; set; }

        public int? ResumeId { get; set; }
    }
}
