namespace CareerBuilderX.Models
{
    public class ServicePortfolio
    {
        public int PortfolioId { get; set; }

        public int ServiceId { get; set; }

        public Portfolio Portfolio { get; set; }

        public Service Service { get; set; }
    }
}
